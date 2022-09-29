using HelixToolkit.Wpf.SharpDX;
using PrimitierSaveEditor.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Entities.Primitier
{
    public class PrimitierCamera : MeshGeometryModel3D, Interfaces.ISelectable
    {
        public PrimitierCamera()
        {
            Geometry = MainViewModel.CameraGizmo;
            Material = MainViewModel.CameraGizmoMat;
            UpdateCamera();
        }

        public void UpdateCamera()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            Point3D cubeCenter = new Point3D(0.5, 0.5, 0.5);

            Matrix3D transformMatrix = new Matrix3D();
            transformMatrix.ScaleAt(new Vector3D(0.3, 0.3, 0.3), cubeCenter);
            transformMatrix.RotateAt(SaveController.Save.cameraRot, cubeCenter);
            transformMatrix.Translate(SaveController.Save.cameraPos);
            MatrixTransform3D camTransform = new MatrixTransform3D(transformMatrix);

            Transform = camTransform;
            mainWindow.viewport.InvalidateRender();
        }

        protected override void OnMouse3DDown(object sender, RoutedEventArgs e) => SelectionController.Selection = this;

        public void Selected()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            mainWindow.propsTable.ItemsSource = new VisualProperty[]
            {
                new VisualProperty("cameraPos", SaveController.Save, Utils.ConvVector3, UpdateCamera),
                new VisualProperty("cameraRot", SaveController.Save, Utils.ConvQuaternion, UpdateCamera),
            };
        }
    }
}
