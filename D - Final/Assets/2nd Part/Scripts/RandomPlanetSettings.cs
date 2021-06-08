using UnityEngine;

namespace _2nd_Part
{
    [CreateAssetMenu()]
    public class RandomPlanetSettings : ScriptableObject
    {
        [Header("Standard Attributes")]
        public RandomInt resolution;
        public RandomInt planetRadius;
        
        [Header("Terrain Attributes")]
        public RandomInt noiseLayers;
        public RandomSimpleNoiseSettings terrainSimpleNoiseSettings;
        public RandomRidgidNoiseSettings terrainRidgidNoiseSettings;
        public RandomFloat terrainNoiseStrength;
        public RandomVector2 terrainTextureScale;
        
        [Header("Biome Attributes")]
        public RandomColor oceanDepth;
        public RandomColor oceanSurface;
        public RandomColor sand;
        public RandomColor ground;
        public RandomColor mountain;
        public RandomColor mountainPeak;
        public RandomInt biomeCount;
        public RandomFloat biomeTintPercent;
        public RandomSimpleNoiseSettings biomeSimpleNoiseSettings;
        public RandomRidgidNoiseSettings biomeRidgidNoiseSettings;
        public RandomFloat biomeNoiseStrength;
        public RandomFloat biomeNoiseOffset;
        public RandomFloat biomeBlendAmount;

        [Header("Water Attributes")]
        public RandomFloat waterSmoothness;
        public RandomFloat waterNoiseStrength;
        public RandomVector2 waterScroll;
        public RandomVector2 waterTextureScale;
    }
}