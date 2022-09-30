using PrimitierSaveEditor.Entities.Primitier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using PrimitierSaveEditor.Interfaces;

namespace PrimitierSaveEditor.Controllers
{
    public static class SelectionController
    {
        public static SelectionTool Tool
        {
            get => tool;
            set
            {
                tool = value;
                UpdateToolLabel();
                UpdateSelection();
            }
        }
        public static ISelectable Selection
        {
            get => selection;
            set
            {
                selection = value;
                UpdateSelection();
            }
        }
        private static ISelectable PrevSelection { get; set; }

        private static ISelectable selection;
        private static SelectionTool tool;

        static void UpdateSelection()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            if (Selection == null)
            {
                mainWindow.propsTable.ItemsSource = null;
                if (PrevSelection is HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D)
                {
                    var sel = PrevSelection as HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D;
                    sel.RenderWireframe = false;
                }
                PrevSelection = null;
                return;
            }

            if (Selection != PrevSelection && Selection is HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D)
            {
                HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D cubeSel = Selection as HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D;

                cubeSel.RenderWireframe = true;
                if (PrevSelection != null && PrevSelection is HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D)
                {
                    HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D prevSel = PrevSelection as HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D;
                    prevSel.RenderWireframe = false;
                }

                PrevSelection = selection;
            }

            Selection.Selected();

            mainWindow.viewport.InvalidateRender();
        }

        private static void UpdateToolLabel()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            switch (Tool)
            {
                case SelectionTool.Cube:
                    mainWindow.toolLabel.Content = "Selection: Cubes";
                    break;
                case SelectionTool.Group:
                    mainWindow.toolLabel.Content = "Selection: Groups";
                    break;
                case SelectionTool.Chunk:
                    mainWindow.toolLabel.Content = "Selection: Chunks";
                    break;
                case SelectionTool.Terrain:
                    mainWindow.toolLabel.Content = "Selection: Terrain";
                    break;
            }
        }
    }

    public enum SelectionTool
    {
        Cube,
        Group,
        Chunk,
        Terrain
    }
}
