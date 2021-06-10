using UnityEngine;

namespace _2nd_Part
{
    public class SimpleNoiseFilter : INoiseFilter
    {
        private NoiseSettings.SimpleNoiseSettings settings;
        Noise noise = new Noise();

        public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
        {
            this.settings = settings;
        }

        public float Evaluate(Vector3 point)
        {
            float noiseVal = 0;
            float frequency = settings.baseRoughness;
            float amplitude = 1;
            for (int i = 0; i < settings.numberOfLayers; i++)
            {
                float v = noise.Evaluate(point * frequency + settings.centre);
                noiseVal += (v + 1) * 0.5f * amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistance;
            }

            noiseVal = noiseVal - settings.minVal;
            return noiseVal* settings.strength;
        }
    }
}
