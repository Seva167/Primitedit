using HelixToolkit.Wpf.SharpDX;
using PrimitierSaveEditor.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Entities.Primitier
{
    public class PrimitierPlayer : MeshGeometryModel3D, Interfaces.ISelectable
    {
        public PrimitierPlayer()
        {
            Geometry = MainViewModel.Cube;
            Material = MainViewModel.PlayerMat;
            UpdatePlayer();
        }

        public void UpdatePlayer()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            Point3D cubeCenter = new Point3D(0.5, 0.5, 0.5);

            Matrix3D transformMatrix = new Matrix3D();
            transformMatrix.ScaleAt(new Vector3D(0.3, 1, 0.3), cubeCenter);
            transformMatrix.RotateAt(new System.Windows.Media.Media3D.Quaternion(new Vector3D(0, 1, 0), SaveController.Save.playerAngle), cubeCenter);
            transformMatrix.Translate(SaveController.Save.playerPos);
            transformMatrix.Translate(new Vector3D(0, 0.5, 0));
            MatrixTransform3D playerTransform = new MatrixTransform3D(transformMatrix);

            Transform = playerTransform;
            mainWindow.viewport.InvalidateRender();
        }

        protected override void OnMouse3DDown(object sender, RoutedEventArgs e) => SelectionController.Selection = this;

        public void Selected()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            mainWindow.propsTable.ItemsSource = new VisualProperty[]
            {
                new VisualProperty("playerPos", SaveController.Save, Utils.ConvVector3, UpdatePlayer),
                new VisualProperty("playerAngle", SaveController.Save, Utils.ConvFloat, UpdatePlayer),
                new VisualProperty("playerMaxLife", SaveController.Save, Utils.ConvFloat, UpdatePlayer),
                new VisualProperty("playerLife", SaveController.Save, Utils.ConvFloat, UpdatePlayer),
            };
        }
    }
}
