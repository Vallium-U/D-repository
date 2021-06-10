using System;
using UnityEngine;

namespace _2nd_Part
{
    public class Planet : MonoBehaviour
    {
        [Range(2,256)]
        public int resolution = 10;

        public bool autoUpdate=true;

        public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };

        public FaceRenderMask faceRenderMask;

        public PlanetShapeSettings planetShapeSettings;
        public PlanetColorSettings planetColorSettings;

        private ShapeGenerator shapeGenerator = new ShapeGenerator();
        private ColorGenerator colorGenerator = new ColorGenerator();
        [HideInInspector]
        public bool shapeSettingsFoldout;
        [HideInInspector]
        public bool colourSettingsFoldout;
        
        [SerializeField,HideInInspector]
        MeshFilter[] meshFilters;
        PlanetTerrainFaces[] terrainFaces;


        private bool procedurallyGenerated = false;
        private void Start()
        {
            if (!procedurallyGenerated)
            {
                GeneratePlanet();   
            }
        }

        public void ConstructRandomPlanet(int res, PlanetShapeSettings sSettings, PlanetColorSettings cSettings)
        {
            this.resolution = res;
            this.planetShapeSettings = sSettings;
            this.planetColorSettings = cSettings;
            procedurallyGenerated = true;
            
            GeneratePlanet();
        }

        void Initialize()
        {
            shapeGenerator.UpdateSettings(planetShapeSettings);
            colorGenerator.UpdateSettings(planetColorSettings);
            if (meshFilters == null || meshFilters.Length==0)
            {
                meshFilters = new MeshFilter[6];
            }
            terrainFaces = new PlanetTerrainFaces[6];

            Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};
        
            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i].mesh = new Mesh();
                }

                meshFilters[i].GetComponent<MeshRenderer>().material = planetColorSettings.planetMaterial;
                
                terrainFaces[i] = new PlanetTerrainFaces(shapeGenerator, meshFilters[i].sharedMesh, resolution,directions[i]);
                bool renderFace = faceRenderMask == FaceRenderMask.All || (int) faceRenderMask - 1 == i;
                meshFilters[i].gameObject.SetActive(renderFace);
            }
        }

        void GenerateMesh()
        {
            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i].gameObject.activeSelf)
                {
                    terrainFaces[i].ConstructMesh(procedurallyGenerated);
                }
            }
            colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
        }
        
        public void GeneratePlanet()
        {
            Initialize();
            GenerateMesh();
            GenerateColour();
        }
        
        public void OnShapeSettingUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateMesh();
            }
        }
        public void OnColourSettingUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateColour();
            }
        }

        void GenerateColour()
        {
            colorGenerator.UpdateColors();
            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i].gameObject.activeSelf)
                {
                    terrainFaces[i].UpdateUVs(colorGenerator);
                }
            }
        }
    
    }
}
