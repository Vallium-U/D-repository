using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Structure {

   public static Queue<VoxelMod> GenerateMajorFlora(int index, Vector3 position, int minTrunkHeight, int maxTrunkHeight)
   {

      switch (index)
      {

         case 0:
            return MakeTree(position, minTrunkHeight, maxTrunkHeight);
         case 1:
            return MakeCacti(position, minTrunkHeight, maxTrunkHeight);
         case 2:
            return MakeHouses(position, minTrunkHeight, maxTrunkHeight);
         case 3:
            return MakeTriangleTree(position, minTrunkHeight, maxTrunkHeight);
      }

      return new Queue<VoxelMod>();
   }
   
private static float MapValueBetween(float value, float oldMin, float oldMax, float newMin, float newMax){
   var divider = oldMax != oldMin ? oldMax - oldMin : 1; // do not divide by zero
   return (newMax - newMin) * (value - oldMin) / divider + newMin;
}
public static Queue<VoxelMod> MakeTriangleTree(Vector3 position, int minTrunkHeight, int maxTrunkHeight)
{
   System.Random rand = new System.Random((int) (position.x * 100000 + position.z));
   Queue<VoxelMod> queue = new Queue<VoxelMod>();
   
   int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 250f, 3f));

   if (height < minTrunkHeight)
      height = minTrunkHeight;
   
   for (int i = 1; i < height; i++)
      queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 6));

   // fastNoise.GetSimplex(position.x, position.y, position.z);
   float leavesCoefX = (MapValueBetween((float) rand.NextDouble(), 0, 1, 0.1f, 0.45f));
   float leavesCoefY = (MapValueBetween((float) rand.NextDouble(), 0, 1, 0.1f, 0.8f));
   float leavesCoefZ = (MapValueBetween((float) rand.NextDouble(), 0, 1, 0.1f, 0.45f));
   int start = -8;
   int end = 10;
   for (int y = 0; y < 14; y++) {
      if (start < end)
      {
         start++;
         end--;
      }
      for (int x = start; x < end; x++) {
         for (int z = start; z < end; z++) {
            queue.Enqueue(new VoxelMod(new Vector3(position.x + x * leavesCoefX , position.y + height + y * leavesCoefY, position.z + z * leavesCoefZ ), 11));
            //queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y, position.z + z), 11));
         }
      }
   }
   return queue;
}
public static Queue<VoxelMod> MakeTree(Vector3 position, int minTrunkHeight, int maxTrunkHeight)
{
   System.Random rand = new System.Random((int) (position.x * 100000 + position.z));
   Queue<VoxelMod> queue = new Queue<VoxelMod>();
   
   int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 250f, 3f));

   if (height < minTrunkHeight)
      height = minTrunkHeight;
   
   for (int i = 1; i < height; i++)
      queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 6));
   
   float leavesCoeffX = (MapValueBetween((float) rand.NextDouble(), 0, 1, 0.1f, 0.5f));
   float leavesCoeffY = (MapValueBetween((float) rand.NextDouble(), 0, 1, 0.1f, 0.5f));
   float leavesCoeffZ = (MapValueBetween((float) rand.NextDouble(), 0, 1, 0.1f, 0.5f));
   int start = -3;
   int end = 4;
   for (int y = 0; y < 2; y++) {
      for (int x = start; x < end; x++) {
         for (int z = start; z < end; z++) {
            queue.Enqueue(new VoxelMod(new Vector3(position.x + x * leavesCoeffX , position.y + height + y * leavesCoeffY, position.z + z * leavesCoeffZ ), 11));
            
         }
      }
   }
   return queue;
}


   public static Queue<VoxelMod> MakeCacti (Vector3 position, int minTrunkHeight, int maxTrunkHeight) {

      Queue<VoxelMod> queue = new Queue<VoxelMod>();

      int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 23456f, 2f));

      if (height < minTrunkHeight)
         height = minTrunkHeight;

      for (int i = 1; i <= height; i++)
      {
         queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 12));
         if (i == height)
         {
            queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 13));
         }
      }
         
      return queue;

   }

   public static Queue<VoxelMod> MakeHouses(Vector3 position, int minTrunkHeight, int maxTrunkHeight)
   {

      Queue<VoxelMod> queue = new Queue<VoxelMod>();
      
      int heightRoof = 4;
      int height = 4;
      int houseFoundationSizeX = 10;
      int houseFoundationSizeZ = 11;
      int houseSizeX = 10;
      int houseSizeZ = 11;
      
      
      //BUILD Left & Right Walls
      for (int k = 0; k <= 10; k++)
      {
         if (k==0 || k == 10)
         {
            for (int i = 1; i <= height; i++)
            {
               for (int j = 0; j <= 10; j++)
               {
                  queue.Enqueue(new VoxelMod(new Vector3(position.x + k, position.y + i, position.z + j), 8));
               }
            }
         }
      }
      //BUILD Front & Back Walls
      for (int k = 0; k <= 10; k++)
      {
         if (k==0 || k == 10)
         {
            for (int i = 1; i <= height; i++)
            {
               for (int j = 0; j <= 10; j++)
               {
                  queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + i, position.z+ k), 8));
                  if (k == 0)
                  {
                     if (j==5 && i <3)
                     {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + i, position.z+ k), 0));
                     }
                     if (j == 2 && i == 2)
                     {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + i, position.z+ k), 10));
                     }
                     if (j == 8 && i == 2)
                     {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + i, position.z+ k), 10));
                     }
                  }
                  if (k == 10)
                  {
                     if (j==5 && i==2)
                     {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + i, position.z+ k), 10));
                     }
                     if (j==4 && i==2)
                     {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + i, position.z+ k), 10));
                     }
                     if (j==6 && i==2)
                     {
                        queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + i, position.z+ k), 10));
                     }
                  }
               }
            }
         }
      }
      
      //AIRBLOCKS Inside house
      for (int k = 1; k <= height; k++)
      {
         for (int j = 1; j < houseSizeZ-1; j++)
         {
            for (int i = 1; i < houseSizeX; i++)
            {
               queue.Enqueue(new VoxelMod(new Vector3(position.x + i, position.y +k, position.z + j), 0));
            }
         }
      }
      //Airblocks in front door
      for (int k = 1; k <= height; k++)
      {
         for (int j = 0; j <= 10; j++)
         {
            for (int i = 1; i <= 3; i++)
            {
               queue.Enqueue(new VoxelMod(new Vector3(position.x + j, position.y + k, position.z - i), 0));
            }
         }
      }
      //BUILD Foundation
      for (int a = 0; a <= houseFoundationSizeX; a++)
      {
          for (int b = 0; b <= houseFoundationSizeZ; b++)
          {
             for (int c = 0; c <= houseFoundationSizeZ; c++)
              {
                 queue.Enqueue(new VoxelMod(new Vector3(position.x + a, position.y - c, position.z + b), 2));
              }
          }
      }
      
      //Build roof
      for (int a = 0; a <= houseFoundationSizeX; a++)
      {
         for (int b = 0; b <= houseFoundationSizeZ; b++)
         {
            queue.Enqueue(new VoxelMod(new Vector3(position.x + a, position.y + height, position.z + b), 7));
            if (b==1 || b == 10)
            {
               int zz = (int) (position.y + height);
               for (int c = zz; c >= position.y + heightRoof; c--)
               {
                  if (c == zz)
                  {
                     continue;
                  }
                  if (a == houseFoundationSizeX / 2)
                  {
                     queue.Enqueue(new VoxelMod(new Vector3(position.x + a, position.y + 6, position.z+ b), 10));// window in the roof
                  }
                  queue.Enqueue(new VoxelMod(new Vector3(position.x + a, c, position.z+b), 7));
               }
            }
         }
         if (a < houseFoundationSizeX/2)
         {
            height++;
         }
         else if (a < houseFoundationSizeX)
         {
            height--;
         }
         // for (int b = 0; b <= blocksOnFloorZ; b++)
         // {
         //    queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + heightRoof, position.z + b), 7));
         // }
      }
      //Build floor
      
      for (int posZ = 0; posZ <= 8; posZ++)
      {
         for (int iX = 0; iX <= 10; iX++)
         {
            queue.Enqueue(new VoxelMod(new Vector3(position.x + iX, position.y, position.z + posZ), 8));
         }
      }
      return queue;

   }

}