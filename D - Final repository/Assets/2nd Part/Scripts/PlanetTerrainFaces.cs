using UnityEngine;

namespace _2nd_Part
{
    public class PlanetTerrainFaces
    {
        private ShapeGenerator shapeGenerator;
        Mesh mesh;
        private int resolution;
        private Vector3 localUp;
        private Vector3 axisA;
        private Vector3 axisB;

        public PlanetTerrainFaces(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
        {
            this.shapeGenerator = shapeGenerator;
            this.mesh = mesh;
            this.resolution = resolution;
            this.localUp = localUp;

            axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            axisB = Vector3.Cross(localUp, axisA);
        }

        public void ConstructMesh(bool procedurallyGenerated)
        {
            Vector3[] vertices = new Vector3[resolution * resolution];
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
            int trianglesIdx = 0;
            //Vector2[] uv;
            Vector2[] uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2[vertices.Length];

            if (procedurallyGenerated || mesh.uv.Length != vertices.Length)
            {
                uv = new Vector2[vertices.Length];
            }
            else
            {
                uv = mesh.uv;
            }
            
            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int i = x + y * resolution;
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 pointOnCubeUnit = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                    Vector3 pointOnSphereUnit = pointOnCubeUnit.normalized;
                    float unscaledElevation = shapeGenerator.CalculateUnscaleElevation(pointOnSphereUnit);
                    vertices[i] = pointOnSphereUnit * shapeGenerator.GetScaledElevation(unscaledElevation);
                    uv[i].y = unscaledElevation;

                    if (x != resolution - 1 && y != resolution-1)
                    {
                        triangles[trianglesIdx] = i;
                        triangles[trianglesIdx+1] = i+resolution+1;
                        triangles[trianglesIdx+2] = i+resolution;
                    
                        triangles[trianglesIdx+3] = i;
                        triangles[trianglesIdx+4] = i+1;
                        triangles[trianglesIdx+5] = i+resolution+1;
                        trianglesIdx += 6;
                    }
                }
            }
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.uv = uv;
        }

        public void UpdateUVs(ColorGenerator colorGenerator)
        {
            Vector2[] uv = mesh.uv;
            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int i = x + y * resolution;
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 pointOnCubeUnit = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                    Vector3 pointOnSphereUnit = pointOnCubeUnit.normalized;

                    uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnSphereUnit);
                }
            }

            mesh.uv = uv;
        }
    }
}
