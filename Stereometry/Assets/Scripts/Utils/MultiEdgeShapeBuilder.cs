using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    public static class MultiEdgeShapeBuilder
    {
        private const string ShapeName = "MultiEdgeShape";

        public static GameObject Create(Vector3[] points, bool doubleSided = false)
        {
            var shapeGameObject = new GameObject(ShapeName);
            var mesh = new Mesh();

            var centroid = points.Aggregate(Vector3.zero, (current, total) => current + total);
            centroid /= points.Length;

            for (var i = 0; i < points.Length; i++)
            {
                points[i] -= centroid;
            }

            shapeGameObject.transform.position = centroid;

            int sidesNumber = doubleSided ? 2 : 1;

            using (var vertexHelper = new VertexHelper())
            {
                var vertexCounter = 0;

                for (var side = 0; side < sidesNumber; side++)
                {
                    for (var i = 0; i < points.Length; i++)
                    {
                        int iMapped = side == 0 ? i : points.Length - 1 - i;

                        var vertex = points[iMapped];

                        var vtx = new UIVertex
                        {
                            position = new Vector3(vertex.x, vertex.y, vertex.z),
                            uv0 = new Vector3(vertex.x, vertex.y, vertex.z)
                        };

                        vertexHelper.AddVert(vtx);

                        if (i > 1 && i < points.Length || i > points.Length + 1)
                        {
                            if (side == 0)
                            {
                                vertexHelper.AddTriangle(0, vertexCounter - 1, vertexCounter);
                            }

                            if (side == 1)
                            {
                                vertexHelper.AddTriangle(points.Length, vertexCounter - 1, vertexCounter);
                            }
                        }

                        vertexCounter++;
                    }
                }

                vertexHelper.FillMesh(mesh);
            }

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            var meshFilter = shapeGameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            shapeGameObject.AddComponent<MeshRenderer>();

            return shapeGameObject;
        }
    }
}
