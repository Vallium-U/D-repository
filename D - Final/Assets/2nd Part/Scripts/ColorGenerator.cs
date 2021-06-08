using System.Collections;
using System.Collections.Generic;
using _2nd_Part;
using UnityEngine;


namespace _2nd_Part
{

    public class ColorGenerator
    {
        private PlanetColorSettings settings;
        private Texture2D texture;
        private const int textureResolution = 50;
        private INoiseFilter biomeNoiseFilter;

        public void UpdateSettings(PlanetColorSettings settings)
        {
            this.settings = settings;
            if (texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
            {
                texture = new Texture2D(textureResolution * 2, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32,false);
            }

            biomeNoiseFilter = NoiseFilterFactory.createNoiseFilter(settings.biomeColourSettings.noise);
        }

        public void UpdateElevation(PlanetMinMax elevationMinMax)
        {
            settings.planetMaterial.SetVector("_elevationMinMax",
                new Vector4(elevationMinMax.Min, elevationMinMax.Max));
        }

        public float BiomePercentFromPoint(Vector3 pointOnSphereUnit)
        {
            float heightPecent = (pointOnSphereUnit.y + 1) / 2f;
            heightPecent += (biomeNoiseFilter.Evaluate(pointOnSphereUnit) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
            float biomeIndex = 0;
            int numBiomes = settings.biomeColourSettings.biomes.Length;
            float blendRange = settings.biomeColourSettings.blendAmount / 2f + 0.001f;

            for (int i = 0; i < numBiomes; i++)
            {
                float distance = heightPecent - settings.biomeColourSettings.biomes[i].startHeight;
                float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
                biomeIndex *= (1 - weight);
                biomeIndex += i * weight;
            }

            return biomeIndex / Mathf.Max(1, numBiomes - 1);
        }

        public void UpdateColors()
        {
            Color[] colors = new Color[texture.width * texture.height];
            int colorIndex = 0;
            foreach (var biome in settings.biomeColourSettings.biomes)
            {
                for (int i = 0; i < textureResolution *2; i++)
                {
                    Color gradietnColor;
                    if (i<textureResolution)
                    {
                        gradietnColor = settings.oceanColor.Evaluate(i / (textureResolution - 1f));
                    }
                    else
                    {
                        gradietnColor = biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                    }
                    
                    Color tintColor = biome.tint;
                    colors[colorIndex] = gradietnColor * (1-biome.tintPercent) + tintColor * biome.tintPercent;
                    colorIndex++;
                }
            }

            texture.SetPixels(colors);
            texture.Apply();
            settings.planetMaterial.SetTexture("_planetTexture",texture);
        }
        
    }
}