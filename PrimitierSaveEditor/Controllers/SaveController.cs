using Newtonsoft.Json;
using PrimitierSaveEditor.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Controllers
{
    public static class SaveController
    {
        public static SaveData Save { get; private set; }

        public static void OpenSaveFile(string filename)
        {
            FileStream fs = null;
            GZipStream gz = null;
            MemoryStream dstMs = null;

            try
            {
                fs = File.OpenRead(filename);
                gz = new GZipStream(fs, CompressionMode.Decompress);
                dstMs = new MemoryStream();
                gz.CopyTo(dstMs);

                byte[] decompressedBytes = dstMs.ToArray();
                string json = Encoding.UTF8.GetString(decompressedBytes);

                Save = JsonConvert.DeserializeObject<SaveData>(json);

                if (Save.saveEditorMetadata == null)
                {
                    Save.saveEditorMetadata = new SaveEditorMetadata
                    {
                        version = Assembly.GetExecutingAssembly().GetName().Version,
                        cameraPos = new Point3D(130, 8, 130),
                        cameraDir = new Vector3D(0.3, -0.3, 5)
                    };
                }

                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.IsDirty = false;
            }
            finally
            {
                gz?.Dispose();
                dstMs?.Dispose();
                fs?.Dispose();

                GC.Collect(0, GCCollectionMode.Forced, true);
            }

        }

        public static void SaveSaveFile(string filename)
        {
            MemoryStream ms = null;
            GZipStream gz = null;
            FileStream dstFs = null;

            try
            {
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                Save.saveEditorMetadata.version = Assembly.GetExecutingAssembly().GetName().Version;
                Save.saveEditorMetadata.cameraPos = mainWindow.viewport.Camera.Position;
                Save.saveEditorMetadata.cameraDir = mainWindow.viewport.Camera.LookDirection;

                string json = JsonConvert.SerializeObject(Save, Formatting.None);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                ms = new MemoryStream(jsonBytes);
                dstFs = File.Create(filename);

                gz = new GZipStream(dstFs, CompressionMode.Compress);

                ms.CopyTo(gz);

                mainWindow.IsDirty = false;
            }
            finally
            {
                gz?.Dispose();
                ms?.Dispose();
                dstFs?.Dispose();

                GC.Collect(0, GCCollectionMode.Forced, true);
            }
        }
    }
}
