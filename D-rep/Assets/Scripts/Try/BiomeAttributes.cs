﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "BiomeAttributes", menuName = "Minecampf/Biome Attributes")]
public class BiomeAttributes : ScriptableObject
{
    
    [Header("Biome")] 
    public string biomeName;
    public int offset;
    public float scale;
    
    public int terrainHeight;
    public float terrainScale;

    public byte surfaceBlock;
    public byte subSurfaceBlock;

    [Header("Major Flora")] 
    public int majorFloraIndex;
    public float majorFloraZoneScale =1.3f;
    [Range(0f,1f)]
    public float majorFloraZoneThreshold = 0.6f;
    public float majorFloraPlacementScale = 15f;
    [Range(0f, 1f)]
    public float majorFloraPlacementThreshold = 0.8f;

    public bool placeMajorFlora = true;

    public int maxHeight = 12;
    public int minHeight = 5;

    public Lode[] lodes;

    public void randomBiome()
    {
    offset = (int)Random.value;
    scale = (int)Random.Range(0.001f,0.999f);
    }
}

public class BiomeGen
{
    
}

[System.Serializable]
public class Lode
{
    public string nodeName;
    public byte blockID;
    public int minHeight;
    public int maxHeight;
    public float scale;
    public float threshold;
    public float noiseOffset;
}