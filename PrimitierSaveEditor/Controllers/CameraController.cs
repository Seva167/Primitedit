using HelixToolkit.Wpf.SharpDX;
using PrimitierSaveEditor.Entities.Primitier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Controllers
{
    public static class CameraController
    {
        public static void SetCameraToCube(PrimitierCube cube)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            Vector3D cubePos = cube.Transform.ToVector3D();
            Vector3D groupPos = cube.Group.Transform.ToVector3D();
            Vector3D chunkPos = cube.Group.Chunk.Transform.ToVector3D();
            Vector3D dstPos = Utils.CubePosToWorldPos(cubePos, groupPos, chunkPos) - mainWindow.viewport.Camera.LookDirection;
            mainWindow.viewport.Camera.AnimateTo(new Point3D(dstPos.X, dstPos.Y, dstPos.Z), mainWindow.viewport.Camera.LookDirection, mainWindow.viewport.Camera.UpDirection, 300);
        }

        public static void SetCameraToPlayer(PrimitierPlayer player)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            Vector3D pos = player.Transform.ToVector3D() - mainWindow.viewport.Camera.LookDirection;
            mainWindow.viewport.Camera.AnimateTo(new Point3D(pos.X, pos.Y, pos.Z), mainWindow.viewport.Camera.LookDirection, mainWindow.viewport.Camera.UpDirection, 300);
        }
    }
}
