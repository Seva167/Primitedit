using HelixToolkit.Wpf.SharpDX;
using PrimitierSaveEditor.Entities.Primitier;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Controllers
{
    public static class TerrainController
    {
        public static MeshGeometryModel3D Water { get; private set; }

        public static MeshGeometryModel3D CreateWater()
        {
            Matrix3D waterTrMatr = new Matrix3D();
            waterTrMatr.ScaleAt(new Vector3D(10000, 0.1, 10000), new Point3D(0.5, 0.5, 0.5));
            waterTrMatr.Transform(new Point3D(0, -0.5, 0));
            MeshBuilder wmb = new MeshBuilder();
            wmb.AddFaceNY();

            MatrixTransform3D waterTr = new MatrixTransform3D(waterTrMatr);
            Water = new MeshGeometryModel3D
            {
                Geometry = wmb.ToMesh(),
                Material = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                {
                    DiffuseColor = new Color4(0.4f, 0.6f, 1f, 1f),
                    EnableFlatShading = true
                },
                Transform = waterTr,
                AlwaysHittable = false,
                IsHitTestVisible = false
            };
            return Water;
        }

        private static PrimitierTerrain[] terrainMeshes;

        public static MeshGeometryModel3D[] CreateTerrain()
        {
            HelixToolkit.Wpf.SharpDX.DiffuseMaterial vertexColMat = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
            {
                VertexColorBlendingFactor = 1,
                EnableFlatShading = true
            };

            terrainMeshes = new PrimitierTerrain[SaveController.Save.terrains.Count];
            for (int k = 0; k < SaveController.Save.terrains.Count; k++)
            {
                MeshBuilder mb = new MeshBuilder();
                Vector3[,] points = new Vector3[64, 64];
                Color4[] colors = new Color4[4096];

                int index = 0;
                for (int i = 0; i < SaveController.Save.terrains[k].heightMap.Length / 64; i++)
                {
                    for (int j = 0; j < SaveController.Save.terrains[k].heightMap.Length / 64; j++)
                    {
                        points[i, j] = new Vector3(j, SaveController.Save.terrains[k].heightMap[index], i);
                        colors[index] = Utils.GetColorForBiome(SaveController.Save.terrains[k].materialMap[index]);
                        index++;
                    }
                }

                mb.AddRectangularMesh(points);
                HelixToolkit.Wpf.SharpDX.MeshGeometry3D mesh = mb.ToMesh();
                mesh.Colors = new Color4Collection(colors);
                PrimitierTerrain terrainMesh = new PrimitierTerrain(SaveController.Save.terrains[k])
                {
                    Geometry = mesh,
                    Material = vertexColMat,
                };

                terrainMeshes[k] = terrainMesh;
            }

            return terrainMeshes;
        }

        public static void TerrainDisplayChange(TerrainView view)
        {
            if (terrainMeshes == null)
                return;

            for (int i = 0; i < terrainMeshes.Length; i++)
            {
                for (int j = 0; j < terrainMeshes[i].Geometry.Colors.Count; j++)
                {
                    switch (view)
                    {
                        case TerrainView.Normal:
                            terrainMeshes[i].Geometry.Colors[j] = Utils.GetColorForBiome(SaveController.Save.terrains[i].materialMap[j]);
                            break;
                        case TerrainView.Temperature:
                            float col = SaveController.Save.terrains[i].temperatureMap[j] / 20f;
                            terrainMeshes[i].Geometry.Colors[j] = new Color4(col, 0, 0, 1);
                            break;
                        case TerrainView.Rainfall:
                            float rain = SaveController.Save.terrains[i].rainfallMap[j] / 60f;
                            terrainMeshes[i].Geometry.Colors[j] = new Color4(0, rain, rain, 1);
                            break;
                    }
                }
                terrainMeshes[i].Geometry.UpdateColors();
            }
        }
    }

    public enum TerrainView
    {
        Normal,
        Temperature,
        Rainfall
    }
}
