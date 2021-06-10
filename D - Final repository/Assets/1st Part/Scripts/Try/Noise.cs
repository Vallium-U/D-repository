using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
   public static float Get2DPerlin(Vector2 position, float offset, float scale)
   {
      position.x += (offset + VoxelData.seed + VoxelData.overalOffset);
      position.y += (offset + VoxelData.seed + VoxelData.overalOffset);
      return Mathf.PerlinNoise(position.x  / VoxelData.ChunkWidth * scale,
         position.y  / VoxelData.ChunkWidth * scale);
   }

   public static bool Get3DPerlin(Vector3 position, float offset, float scale, float threshold)
   {
      float x = (position.x + VoxelData.seed + offset + VoxelData.overalOffset) * scale;
      float y = (position.y + VoxelData.seed + offset + VoxelData.overalOffset) * scale;
      float z = (position.z + VoxelData.seed + offset + VoxelData.overalOffset) * scale;

      float AB = Mathf.PerlinNoise(x, y);
      float BC = Mathf.PerlinNoise(y, z);
      float AC = Mathf.PerlinNoise(x, z);
      float BA = Mathf.PerlinNoise(y, x);
      float CB = Mathf.PerlinNoise(z, y);
      float CA = Mathf.PerlinNoise(z, x);

      if ((AB + BC + AC + BA + CB + CA) / 6f > threshold)
         return true;
      else
         return false; 
   }
    
}
