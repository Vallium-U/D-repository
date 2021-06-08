using UnityEngine;

namespace _2nd_Part
{
    public class ShapeGenerator
    {
        private PlanetShapeSettings settings;
        private INoiseFilter[] noiseFilters;
        public PlanetMinMax elevationMinMax;

        public void UpdateSettings(PlanetShapeSettings settings)
        {
            this.settings = settings;
            noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
            for (int i = 0; i < noiseFilters.Length; i++)
            {
                noiseFilters[i] = NoiseFilterFactory.createNoiseFilter(settings.noiseLayers[i].noiseSettings);
            }

            elevationMinMax = new PlanetMinMax();
        }

        public float CalculateUnscaleElevation(Vector3 pointOnSphereUnit)
        {
            float firstLayerVal = 0;
            float elevation = 0;
            if (noiseFilters.Length>0)
            {
                firstLayerVal = noiseFilters[0].Evaluate(pointOnSphereUnit);
                if (settings.noiseLayers[0].enabled)
                {
                    elevation = firstLayerVal;
                }
            }
            for (int i = 0; i < noiseFilters.Length; i++)
            {
                if (settings.noiseLayers[i].enabled)
                {
                    float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerVal : 1;
                    elevation += noiseFilters[i].Evaluate(pointOnSphereUnit) * mask;   
                }
            }
            
            elevationMinMax.AddVal(elevation);
            return elevation;
        }

        public float GetScaledElevation(float unscaledElevation)
        {
            float elevation = Mathf.Max(0, unscaledElevation);
            elevation = settings.planetRadius * (1 + elevation);
            return elevation;
        }
    }
}
