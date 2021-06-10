using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _2nd_Part
{
    public class PlanetGenerator : MonoBehaviour
    {
        [Header("Random Planet Attributes")]
        public RandomPlanetSettings[] settings;
        protected int planetSettingsIndex;

        [HideInInspector] 
        public bool settingsFoldout = false;

        [Header("Other Attributes")] 
        public Material copyMaterial;
        public Transform[] spawnTransforms;
        public bool parentToSpawn;

        private GameObject[] planetGameObjects;
        private Planet[] planets;
        
        // Start is called before the first frame update
        void Awake()
        {
            planetGameObjects = new GameObject[spawnTransforms.Length];
            planets = new Planet[spawnTransforms.Length];

            for (int i = 0; i < spawnTransforms.Length; i++)
            {
                planetGameObjects[i] = new GameObject("Generated Planet #");
                planets[i] = planetGameObjects[i].AddComponent<Planet>();
                GenerateRandomPlanet(planets[i], i);

                planetGameObjects[i].transform.position = spawnTransforms[i].position;
                planetGameObjects[i].transform.rotation = spawnTransforms[i].rotation;
                planetGameObjects[i].transform.localScale = spawnTransforms[i].localScale;

                if (parentToSpawn)
                {
                    planetGameObjects[i].transform.SetParent(spawnTransforms[i]);
                    planetGameObjects[i].transform.localPosition = Vector3.zero;
                    planetGameObjects[i].transform.localRotation = Quaternion.identity;
                }
                
                Rotator rotator = planetGameObjects[i].AddComponent<Rotator>();
                rotator.axis = RandomXT.RandomUnitVector3();
                rotator.rotationSpeed = Random.Range(10f, 30f);
            }
            
            

            
        }

        void GenerateRandomPlanet(Planet planet, int planetSettingsId)
        {
             //terrain settings randomize
            PlanetShapeSettings shapeSettings = new PlanetShapeSettings();
            shapeSettings.planetRadius = settings[planetSettingsId].planetRadius.PickRandomValue();
            shapeSettings.noiseLayers = new PlanetShapeSettings.NoiseLayer[settings[planetSettingsId].noiseLayers.PickRandomValue()];
            for (int i = 0; i < shapeSettings.noiseLayers.Length; ++i)
            {
                PlanetShapeSettings.NoiseLayer randomLayer = new PlanetShapeSettings.NoiseLayer();
                randomLayer.enabled = true;
                randomLayer.useFirstLayerAsMask = i == 0 ? false : RandomXT.RandomBool();

                NoiseSettings randomNoiseSettings = new NoiseSettings();
                randomNoiseSettings.filterType = NoiseSettings.FilterType.Simple;
                randomNoiseSettings.simpleNoiseSettings = settings[planetSettingsId].terrainSimpleNoiseSettings.PickRandomValue();

                randomLayer.noiseSettings = randomNoiseSettings;
                shapeSettings.noiseLayers[i] = randomLayer;
            }
            
            //color settings randomize
            PlanetColorSettings colorSettings = new PlanetColorSettings();
            colorSettings.planetMaterial = new Material(copyMaterial);
            colorSettings.planetMaterial.SetFloat("_smoothness", settings[planetSettingsId].waterSmoothness.PickRandomValue());
            colorSettings.planetMaterial.SetFloat("_waterNoiseStrength", settings[planetSettingsId].waterNoiseStrength.PickRandomValue());
            colorSettings.planetMaterial.SetVector("_waterScroll", settings[planetSettingsId].waterScroll.PickRandomValue());
            colorSettings.planetMaterial.SetVector("_waterTextureScale", settings[planetSettingsId].waterTextureScale.PickRandomValue());
            colorSettings.planetMaterial.SetFloat("_terrainNoiseStrength", settings[planetSettingsId].terrainNoiseStrength.PickRandomValue());
            colorSettings.planetMaterial.SetVector("_terrainTextureScale", settings[planetSettingsId].terrainTextureScale.PickRandomValue());
            colorSettings.biomeColourSettings = new PlanetColorSettings.BiomeColourSettings();
            colorSettings.biomeColourSettings.blendAmount = settings[planetSettingsId].biomeBlendAmount.PickRandomValue();
            colorSettings.biomeColourSettings.noiseOffset = settings[planetSettingsId].biomeNoiseOffset.PickRandomValue();
            colorSettings.biomeColourSettings.noiseStrength = settings[planetSettingsId].biomeNoiseStrength.PickRandomValue();

            NoiseSettings biomeNoiseSettings = new NoiseSettings();
            biomeNoiseSettings.filterType = NoiseSettings.FilterType.Simple;
            biomeNoiseSettings.simpleNoiseSettings = settings[planetSettingsId].biomeSimpleNoiseSettings.PickRandomValue();
            colorSettings.biomeColourSettings.noise = biomeNoiseSettings;
            
            colorSettings.oceanColor = RandomXT.RandomGradient(new Color[]{settings[planetSettingsId].oceanDepth.PickRandomValue(), 
                settings[planetSettingsId].oceanSurface.PickRandomValue()});

                colorSettings.biomeColourSettings.biomes =
                new PlanetColorSettings.BiomeColourSettings.Biome[settings[planetSettingsId].biomeCount.PickRandomValue()];
            float startHeight = 0f;
            float increment = 1f / (float) settings[planetSettingsId].biomeCount.lastValue;
            for (int i = 0; i < colorSettings.biomeColourSettings.biomes.Length; ++i)
            {
                colorSettings.biomeColourSettings.biomes[i] = new PlanetColorSettings.BiomeColourSettings.Biome();
                colorSettings.biomeColourSettings.biomes[i].tintPercent = settings[planetSettingsId].biomeTintPercent.PickRandomValue();
                colorSettings.biomeColourSettings.biomes[i].startHeight = startHeight;
                colorSettings.biomeColourSettings.biomes[i].gradient = RandomXT.RandomGradient(new Color[]{settings[planetSettingsId].sand.PickRandomValue(), 
                    settings[planetSettingsId].ground.PickRandomValue(), 
                    settings[planetSettingsId].mountain.PickRandomValue(),
                    settings[planetSettingsId].mountainPeak.PickRandomValue()});
                colorSettings.biomeColourSettings.biomes[i].tint = colorSettings.biomeColourSettings.biomes[i].gradient
                    .Evaluate(Random.Range(0.2f, 0.8f));

                startHeight += increment;
            }

            planet.ConstructRandomPlanet(settings[planetSettingsId].resolution.PickRandomValue(), shapeSettings, colorSettings);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}