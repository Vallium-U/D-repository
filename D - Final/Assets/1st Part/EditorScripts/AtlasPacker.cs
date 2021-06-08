// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using Object = UnityEngine.Object;
//
// public class AtlasPacker : EditorWindow
// {
//     public int blockSize = 16; // block size in pixels
//     public int atlasSizeInBlock = 16;
//     public int atlasSize;
//
//     Object[] rawTextures = new Object[256];
//     List<Texture2D> sortedTextures = new List<Texture2D>();
//     Texture2D atlas;
//     
//     [MenuItem("NotMine and NotCraft/Atlas Packer")]
//     public static void ShowWindow()
//     {
//         EditorWindow.GetWindow(typeof(AtlasPacker));
//         
//     }
//
//     private void OnGUI()
//     {
//         atlasSize = blockSize * atlasSizeInBlock;
//         GUILayout.Label("NotMine and NotCraft Texture Atlas Packer", EditorStyles.boldLabel);
//
//         blockSize = EditorGUILayout.IntField("Block size", blockSize);
//         atlasSizeInBlock = EditorGUILayout.IntField("Atlas size(in blocks)", atlasSizeInBlock);
//         
//         GUILayout.Label(atlas);
//         
//         if (GUILayout.Button("Load Textures"))
//         {
//             LoadTextures();
//             PackAtlas();
//             
//             Debug.Log("Atlas Packer: Textures loaded!");
//         }
//
//         if (GUILayout.Button("Clear Textures"))
//         {
//             atlas = new Texture2D(atlasSize, atlasSize);
//             Debug.Log("Atlas Packer: Textures cleared!");
//         }
//
//         if (GUILayout.Button("Save Atlas"))
//         {
//             byte[] bytes = atlas.EncodeToPNG();
//             try
//             {
//                 Debug.Log("Saved");
//                 File.WriteAllBytes(Application.dataPath + "/1st Part/Textures/Packed_Atlas1.png", bytes);
//             }
//             catch
//             {
//                 Debug.Log("Atlas Packer: Couldn't save atlas to file ERROR!");
//                 
//             }
//         }
//
//     }
//
//     void LoadTextures()
//     {
//         sortedTextures.Clear();//clear list
//         
//         rawTextures = Resources.LoadAll("AtlasPacker", typeof(Texture2D));
//         int index = 0;
//         foreach (var tex in rawTextures)
//         {
//
//             var t = tex as Texture2D;
//
//             if (!(t is null) && t.width == blockSize && t.height == blockSize)
//             {
//              sortedTextures.Add(t);   
//             }
//             else
//             {
//                 if (!(t is null)) Debug.Log("Atlas Packer: " + t.name + " incorrect size. Texture not loaded!");
//             }
//             index++;
//
//
//         }
//
//         Debug.Log("Atlas Packer: " + sortedTextures.Count + " successfully load!");
//     }
//
//     void PackAtlas()
//     {
//         atlas = new Texture2D(atlasSize, atlasSize);
//         Color[] pixels = new Color[atlasSize * atlasSize];
//         for (int x = 0; x < atlasSize; x++)
//         {
//             for (int y = 0; y < atlasSize; y++)
//             {
//                 //Get current block that we're looking at.
//                 int currentBlockX = x / blockSize;
//                 int currentBlockY = y / blockSize;
//
//                 int index = currentBlockY * atlasSizeInBlock + currentBlockX;
//                 //Get the pixels in the current block
//                 int currentPixelX = x - (currentBlockX * blockSize);
//                 int currentPixelY = y - (currentBlockY * blockSize);
//
//                 if (index < sortedTextures.Count)
//                 {
//                     pixels[(atlasSize - y - 1) * atlasSize + x] = sortedTextures[index].GetPixel(x, blockSize - y - 1);
//                 }
//                 else
//                 {
//                     pixels[(atlasSize - y - 1) * atlasSize + x] = new Color(0f, 0f, 0f);
//                 }
//             }
//             atlas.SetPixels(pixels);
//             atlas.Apply();
//         }
//     }
//     
// }
