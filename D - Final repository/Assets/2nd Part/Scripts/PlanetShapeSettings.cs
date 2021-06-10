using UnityEngine;

namespace _2nd_Part
{
    [CreateAssetMenu]
    public class PlanetShapeSettings : ScriptableObject
    {
        public float planetRadius;
        public NoiseLayer[] noiseLayers;
        
        [System.Serializable]
        public class NoiseLayer
        {
            public bool enabled = true;
            public bool useFirstLayerAsMask;
            public NoiseSettings noiseSettings;
        }
    }
}
