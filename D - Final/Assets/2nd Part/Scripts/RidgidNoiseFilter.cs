using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _2nd_Part
{

    public class RidgidNoiseFilter : INoiseFilter 
    {
        private NoiseSettings.RidgidNoiseSettings settings;
        Noise noise = new Noise();

        public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings settings)
        {
            this.settings = settings;
        }

        public float Evaluate(Vector3 point)
        {
            float noiseVal = 0;
            float frequency = settings.baseRoughness;
            float amplitude = 1;
            float weight = 1;
            
            for (int i = 0; i < settings.numberOfLayers; i++)
            {
                float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
                v *= v;
                v *= weight;
                weight = Mathf.Clamp01(v * settings.weightMultiplier);
                noiseVal += v * amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistance;
            }

            noiseVal = noiseVal - settings.minVal;
            return noiseVal* settings.strength;
        }
    }
}
