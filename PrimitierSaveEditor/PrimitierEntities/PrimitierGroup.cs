using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Media3D;
using PrimitierSaveEditor.Entities;
using System.Windows;

namespace PrimitierSaveEditor.Entities.Primitier
{
    public class PrimitierGroup : GroupModel3D, Interfaces.ISelectable
    {
        public PrimitierChunk Chunk { get; }
        public SaveData.ChunkData.GroupData Data { get; }

        public PrimitierGroup(SaveData.ChunkData.GroupData data, PrimitierChunk chunk)
        {
            Data = data;
            Chunk = chunk;
            UpdateGroup();
        }

        public void UpdateGroup()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            Point3D groupCenter = new Point3D(0.5, 0.5, 0.5);

            Matrix3D transformMatrix = new Matrix3D();
            transformMatrix.RotateAt(Data.rot, groupCenter);
            transformMatrix.Translate(Data.pos);

            MatrixTransform3D groupTransform = new MatrixTransform3D(transformMatrix);

            Transform = groupTransform;
            mainWindow.viewport.InvalidateRender();
        }

        public void Selected()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            mainWindow.propsTable.ItemsSource = new VisualProperty[]
            {
                new VisualProperty("pos", Data, Utils.ConvVector3, UpdateGroup),
                new VisualProperty("rot", Data, Utils.ConvQuaternion, UpdateGroup),
                new VisualProperty("cubes", Data, null, UpdateGroup, true)
            };
        }
    }
}