using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    public Settings settings;
    
    [Header("World Generation Values")]
    public BiomeAttributes[] biomes;
    
    public Transform player;
    public Vector3 spawnPosition;
    
    public Material material;
    public Material transparentMaterial;
    
    public BlockType[] blockTypes;
    
    //???private Dictionary<ChunkCoord, Chunk> chunks = new Dictionary<ChunkCoord, Chunk>();
    Chunk[,] chunks = new Chunk[VoxelData.WorldSizeInChunks,VoxelData.WorldSizeInChunks];

    private List<ChunkCoord> activeChunks = new List<ChunkCoord>();
    public ChunkCoord playerChunkCoord;
    private ChunkCoord playerLastChunkCoord;
    
    List<ChunkCoord> chunksToCreate = new List<ChunkCoord>();
    public List<Chunk> chunksToUpdate = new List<Chunk>();
    public Queue<Chunk> chunksToDraw = new Queue<Chunk>();

    private bool applyingModifications = false;
    
    Queue<Queue<VoxelMod>> modifications = new Queue<Queue<VoxelMod>>();

    Thread ChunkUpdateThread;
    public object ChunkUpdateThreadLock = new object();

    private void Start()
    {
        Debug.Log("Generating new world using seed " + VoxelData.seed);
        Debug.Log("Generating new world using overall offset " + VoxelData.overalOffset);
        // string jsonExport = JsonUtility.ToJson(settings);
        // Debug.Log(jsonExport);
        //
        // File.WriteAllText(Application.dataPath + "/settings.cfg",jsonExport);
        //var cfgLoad = Resources.Load(Application.dataPath + "/Resources/settings.cfg");
        string jsonImport = File.ReadAllText(Application.dataPath + "/Resources/settings.cfg");
        settings = JsonUtility.FromJson<Settings>(jsonImport);
        
        Random.InitState(VoxelData.seed);
        if (settings.enableThreading)
        {
            ChunkUpdateThread = new Thread(new ThreadStart(ThreadedUpdate));
            ChunkUpdateThread.Start();  
        }
        
        spawnPosition = new Vector3((VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth)/2f,VoxelData.ChunkHeight + 10f,(VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth)/2f);

        GenerateWorld();

        playerLastChunkCoord = GetChunkCoordFromVector3(player.position);

        

    }

    private void Update()
    {
        playerChunkCoord = GetChunkCoordFromVector3(player.position);
        if (!playerChunkCoord.Equals(playerLastChunkCoord))
        {
            CheckViewDistance();
        }
        
        
        if(chunksToCreate.Count>0)
            CreateChunk();

        if(chunksToDraw.Count>0)
            if (chunksToDraw.Peek().isEditable)
                    chunksToDraw.Dequeue().CreateMesh();
            
        if (!settings.enableThreading) {

            if (!applyingModifications)
                ApplyModifications();

            if (chunksToUpdate.Count > 0)
                UpdateChunks();

        }
    }

    void GenerateWorld()
    {
        for (int x = (VoxelData.WorldSizeInChunks/2) - settings.viewDistance; x < (VoxelData.WorldSizeInChunks/2) + settings.viewDistance; x++)
        {
            for (int z = (VoxelData.WorldSizeInChunks/2) - settings.viewDistance; z < (VoxelData.WorldSizeInChunks/2) + settings.viewDistance; z++)
            {
                ChunkCoord newChunk = new ChunkCoord(x,z);
                chunks[x,z] = new Chunk(newChunk,this);
                chunksToCreate.Add(newChunk);
            }
        }
        
        player.position = spawnPosition;
        CheckViewDistance();
    }

    void CreateChunk()
    {
        ChunkCoord c = chunksToCreate[0];
        chunksToCreate.RemoveAt(0);
        chunks[c.x,c.z].Init();
        
    }

    void UpdateChunks()
    {
        bool updated = false;
        int index = 0;

        lock (ChunkUpdateThreadLock)
        {
            
        
            while (!updated && index<chunksToUpdate.Count-1)
            {
                if (chunksToUpdate[index].isEditable)
                {
                    chunksToUpdate[index].UpdateChunk();
                    if (!activeChunks.Contains(chunksToUpdate[index].coord))
                    {
                        activeChunks.Add(chunksToUpdate[index].coord);  
                    }
                    chunksToUpdate.RemoveAt(index);
                    updated = true;
                }
                else
                {
                    index++;
                }
            }
            
        }
    }

    void ThreadedUpdate()
    {
        while (true)
        {
            if (!applyingModifications)
                ApplyModifications();
            if (chunksToUpdate.Count>0)
                UpdateChunks();
        }
    }

    private void OnDisable()
    {
        if (settings.enableThreading) 
        {
            ChunkUpdateThread.Abort();
        }
    }

    void ApplyModifications()
    {
        applyingModifications = true;

        while (modifications.Count>0)
        {
            Queue<VoxelMod> queue = modifications.Dequeue();
            while (queue.Count>0)
            { 
                VoxelMod v = queue.Dequeue();
                ChunkCoord c = GetChunkCoordFromVector3(v.position);
                
                if (chunks[c.x, c.z] == null)
                {
                 chunks[c.x,c.z] = new Chunk(c, this);
                 chunksToCreate.Add(c);
                }
                chunks[c.x,c.z].modifications.Enqueue(v);
                
            }
        }
        applyingModifications = false;
    }
    

    ChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
        return new ChunkCoord(x,z);
    }

    public Chunk GetChunckFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
        return chunks[x,z];
    }
    
    void CheckViewDistance()
    {
        ChunkCoord coord = GetChunkCoordFromVector3(player.position);
        playerLastChunkCoord = playerChunkCoord;
        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);
        
        activeChunks.Clear();
        
        for (int x = coord.x-settings.viewDistance; x < coord.x+settings.viewDistance; x++)
        {
            for (int z = coord.z - settings.viewDistance; z < coord.z + settings.viewDistance; z++)
            {
                ChunkCoord chunkCoord = new ChunkCoord(x, z);
                
                if (IsChunkInWorld(new ChunkCoord(x,z)))
                {
                    if (chunks[x, z] == null)
                    {
                        chunks[x,z] = new Chunk(new ChunkCoord(x,z),this);
                        chunksToCreate.Add(new ChunkCoord(x,z));
                    }else if (!chunks[x, z].isActive)
                    {
                        chunks[x, z].isActive = true;
                        
                    }
                    activeChunks.Add(new ChunkCoord(x,z));
                }
                for (int i = 0; i < previouslyActiveChunks.Count; i++)
                {
                    if(previouslyActiveChunks[i].Equals(new ChunkCoord(x,z)))
                        previouslyActiveChunks.RemoveAt(i);
                    
                 
                
                }
            }
        }

         foreach (ChunkCoord c in previouslyActiveChunks)
         {
             chunks[c.x, c.z].isActive = false;
             
        
         }
        
    }

    public bool CheckForVoxel(Vector3 pos)
    {
       
        ChunkCoord thisChunk = new ChunkCoord(pos);
        
        if (!IsChunkInWorld(thisChunk) || pos.y<0 || pos.y>VoxelData.ChunkHeight)
            return false;
        if (chunks[thisChunk.x, thisChunk.z] != null && chunks[thisChunk.x, thisChunk.z].isEditable)
            return blockTypes[chunks[thisChunk.x, thisChunk.z].GetVoxelFromGlobalVector3(pos)].isSolid;
        return blockTypes[GetVoxel(pos)].isSolid;

    }
    
    public bool CheckIfVoxelTransparent(Vector3 pos)
    {
        ChunkCoord thisChunk = new ChunkCoord(pos);

        if (!IsChunkInWorld(thisChunk) || pos.y<0 || pos.y>VoxelData.ChunkHeight)
            return false;
        if (chunks[thisChunk.x, thisChunk.z] != null && chunks[thisChunk.x, thisChunk.z].isEditable)
            return blockTypes[chunks[thisChunk.x, thisChunk.z].GetVoxelFromGlobalVector3(pos)].isTransparent;
        return blockTypes[GetVoxel(pos)].isTransparent;

    }
    

    public byte GetVoxel(Vector3 pos)
    {
        int yPos = Mathf.FloorToInt(pos.y);
        /* IMMUTABLE PASS*/
        //If outside world, return aid
        if (!IsVoxelInWorld(pos))
            return 0;
        //If bottom block of chunk, return bedrock.
        if (yPos == 0)
            return 1;
        
        /* BIOME SELECTION PASS*/

        int solidGroundHeight = 42;
        float sumOfHeights = 0f;
        int count = 0;
        float strongestWeight = 0f;
        int strongestBiomeIndex = 0;

        for (int i = 0; i < biomes.Length; i++) {

            float weight = Noise.Get2DPerlin(new Vector2(pos.x, pos.z), biomes[i].offset, biomes[i].scale);

            // Keep track of which weight is strongest.
            if (weight > strongestWeight) {

                strongestWeight = weight;
                strongestBiomeIndex = i;

            }

            // Get the height of the terrain (for the current biome) and multiply it by its weight.
            float height = biomes[i].terrainHeight * Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biomes[i].terrainScale) * weight;

            // If the height value is greater 0 add it to the sum of heights.
            if (height > 0) {

                sumOfHeights += height;
                count++;

            }

        }

        // Set biome to the one with the strongest weight.
        BiomeAttributes biome = biomes[strongestBiomeIndex];

        // Get the average of the heights.
        sumOfHeights /= count;

        int terrainHeight = Mathf.FloorToInt(sumOfHeights + solidGroundHeight);


        //BiomeAttributes biome = biomes[index];
        /* BASIC TERRAIN PASS*/
        byte voxelValue = 0;
        if (yPos == terrainHeight)
            voxelValue = biome.surfaceBlock;
        else if (yPos < terrainHeight && yPos > terrainHeight - 4)
            voxelValue = biome.subSurfaceBlock;
        else if (yPos > terrainHeight)
            return 0;
        else
            voxelValue = 2;
        
        /*SECOND PASS*/
        if (voxelValue == 2)
        {
            foreach (Lode lode in biome.lodes)
            {
                if (yPos > lode.minHeight && yPos < lode.maxHeight)
                    if (Noise.Get3DPerlin(pos, lode.noiseOffset, lode.scale, lode.threshold))
                        voxelValue = lode.blockID;
            }
        }
        
        /*TREE PASS*/
        if (yPos == terrainHeight && biome.placeMajorFlora)
        {
            if (Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biome.majorFloraZoneScale) > biome.majorFloraZoneThreshold)
            {
                if (Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biome.majorFloraPlacementScale) >
                    biome.majorFloraPlacementThreshold)
                {
                    modifications.Enqueue(Structure.GenerateMajorFlora(biome.majorFloraIndex, pos, biome.minHeight, biome.maxHeight));
                }
            }
            
        }

        return voxelValue;

    }



    bool IsChunkInWorld(ChunkCoord coord)
    {
        if (coord.x > 0 && coord.x < VoxelData.WorldSizeInChunks - 1 && coord.z > 0 &&
            coord.z < VoxelData.WorldSizeInChunks - 1)
            return true;
        else
        {
            return false;
        }
    }

    bool IsVoxelInWorld(Vector3 pos)
        {
            if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 &&
                pos.y < VoxelData.ChunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
                return true;
            else
                return false;
        }
    }
    




[System.Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;
    public bool isTransparent;
    public Sprite icon;

    [Header("Texture Values")] 
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;
    //back,front,top,bottom, left, right

    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Error in GetTextureID; invalid face index");
                return 0;
        }
        
    }
}

public class VoxelMod
{
    public Vector3 position;
    public byte id;

    public VoxelMod()
    {
        position = new Vector3();
        id = 0;
    }
    public VoxelMod(Vector3 _position, byte _id)
    {
        position = _position;
        id = _id;
    }
}

[System.Serializable]
public class Settings
{
    [Header("Perfomance")]
    public int viewDistance = 8;
    public bool enableThreading = true;

    [Header("Controls")]
    [Range(0.1f,10f)]
    public float mouseSensetivity = 2.0f;

}