using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using PrimitierSaveEditor.Entities;
using PrimitierSaveEditor.Controllers;

namespace PrimitierSaveEditor.Entities.Primitier
{
    public class PrimitierCube : MeshGeometryModel3D, Interfaces.ISelectable
    {
        public PrimitierGroup Group { get; }
        public CubeData Data { get; }

        public PrimitierCube(CubeData data, PrimitierGroup group)
        {
            Data = data;
            Group = group;
            Geometry = MainViewModel.Cube;
            UpdateCube();
        }

        public void UpdateCube()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            Point3D cubeCenter = new Point3D(0.5, 0.5, 0.5);

            Matrix3D transformMatrix = new Matrix3D();
            transformMatrix.ScaleAt(Data.scale, cubeCenter);
            transformMatrix.RotateAt(Data.rot, cubeCenter);
            transformMatrix.Translate(Data.pos);

            MatrixTransform3D cubeTransform = new MatrixTransform3D(transformMatrix);

            Transform = cubeTransform;
            Material = Utils.GetMaterialForSubstance(Data.substance);

            mainWindow.viewport.InvalidateRender();
        }

        protected override void OnMouse3DDown(object sender, RoutedEventArgs e)
        {
            if (SelectionController.Tool == SelectionTool.Terrain)
                return;

            SelectionController.Selection = this;
        }

        public void Selected()
        {
            switch (SelectionController.Tool)
            {
                case SelectionTool.Group:
                    Group.Selected();
                    return;
                case SelectionTool.Chunk:
                    Group.Chunk.Selected();
                    return;
            }

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            mainWindow.propsTable.ItemsSource = new VisualProperty[]
            {
                new VisualProperty("pos", Data, Utils.ConvVector3, UpdateCube),
                new VisualProperty("rot", Data, Utils.ConvQuaternion, UpdateCube),
                new VisualProperty("scale", Data, Utils.ConvVector3, UpdateCube),
                new VisualProperty("lifeRatio", Data, Utils.ConvFloat, UpdateCube),
                new VisualProperty("anchor", Data, Utils.ConvEnum, UpdateCube),
                new VisualProperty("substance", Data, Utils.ConvEnum, UpdateCube),
                new VisualProperty("connections", Data, Utils.ConvEnum, UpdateCube, true),
                new VisualProperty("temperature", Data, Utils.ConvFloat, UpdateCube),
                new VisualProperty("isBurning", Data, Utils.ConvBool, UpdateCube),
                new VisualProperty("burnedRatio", Data, Utils.ConvFloat, UpdateCube),
                new VisualProperty("sectionState", Data, Utils.ConvEnum, UpdateCube),
                new VisualProperty("uvOffset", Data, Utils.ConvUvOffset, UpdateCube),
                new VisualProperty("behaviors", Data, Utils.ConvEnum, UpdateCube, true),
                new VisualProperty("states", Data, Utils.ConvEnum, UpdateCube, true)
            };
        }
    }
}
