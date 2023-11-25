using HelixToolkit.Wpf.SharpDX;
using PrimitierSaveEditor.Controllers;
using PrimitierSaveEditor.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Entities.Primitier
{
    public class PrimitierChunk : GroupModel3D, Interfaces.ISelectable
    {
        public SaveData.ChunkData Data { get; }

        public PrimitierChunk(SaveData.ChunkData data)
        {
            Data = data;
            UpdateChunk();
        }

        public void UpdateChunk()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            // Chunk system was reworked in 1.4.0
            if (SaveController.Save.version[1] > 3)
                Transform = new TranslateTransform3D(Data.x * 16, 0, Data.z * 16);

            mainWindow.viewport.InvalidateRender();
        }

        public void Selected()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            mainWindow.propsTable.ItemsSource = new VisualProperty[]
            {
                new VisualProperty("x", Data, Utils.ConvInt, UpdateChunk),
                new VisualProperty("z", Data, Utils.ConvInt, UpdateChunk),
                new VisualProperty("groups", Data, null, UpdateChunk, true)
            };
        }
    }
}
