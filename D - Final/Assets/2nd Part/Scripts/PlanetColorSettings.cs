﻿using UnityEngine;

namespace _2nd_Part
{
    [CreateAssetMenu]
    public class PlanetColorSettings : ScriptableObject
    {
        public Material planetMaterial;
        public BiomeColourSettings biomeColourSettings;
        public Gradient oceanColor;
        [System.Serializable]
        public class BiomeColourSettings
        {
            public Biome[] biomes;
            public NoiseSettings noise;
            public float noiseOffset;
            public float noiseStrength;
            [Range(0,1)]
            public float blendAmount;
            [System.Serializable]
            public class Biome
            {
                public Gradient gradient;
                public Color tint;
                [Range(0, 1)] 
                public float startHeight;
                [Range(0, 1)] 
                public float tintPercent;
            }
        }
        
    }
}