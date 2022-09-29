using HelixToolkit.Wpf.SharpDX;
using PrimitierSaveEditor.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Entities.Primitier
{
    public class PrimitierTerrain : MeshGeometryModel3D, Interfaces.ISelectable
    {
        public SaveData.TerrainData Data { get; }

        public PrimitierTerrain(SaveData.TerrainData data)
        {
            Data = data;
            UpdateTerrain();
        }

        public void UpdateTerrain()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            Transform = new TranslateTransform3D(Data.x * 252f, 0, Data.z * 252f);

            mainWindow.viewport.InvalidateRender();
        }

        protected override void OnMouse3DDown(object sender, RoutedEventArgs e)
        {
            if (SelectionController.Tool != SelectionTool.Terrain)
                return;

            SelectionController.Selection = this;
        }

        public void Selected()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            mainWindow.propsTable.ItemsSource = new VisualProperty[]
            {
                new VisualProperty("x", Data, Utils.ConvInt, UpdateTerrain),
                new VisualProperty("z", Data, Utils.ConvInt, UpdateTerrain),
                new VisualProperty("generated", Data, Utils.ConvBool, UpdateTerrain),
                new VisualProperty("materialGenerated", Data, Utils.ConvBool, UpdateTerrain)
            };
        }
    }
}
