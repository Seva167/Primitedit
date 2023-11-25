using Newtonsoft.Json;
using PrimitierSaveEditor.Entities;
using PrimitierSaveEditor.PMAPI;
using SharpDX;
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
        public static ExtData ExtData { get; private set; }

        public static void OpenSaveFile(string filename)
        {
            bool isUncompressed = filename.EndsWith("json");

            Logger.LogInfo($"Opening {(isUncompressed ? "uncompressed " : "")}{filename}");

            FileStream fs = null;
            GZipStream gz = null;
            MemoryStream dstMs = null;

            try
            {
                string json = null;
                if (!isUncompressed)
                {
                    fs = File.OpenRead(filename);
                    gz = new GZipStream(fs, CompressionMode.Decompress);
                    dstMs = new MemoryStream();
                    gz.CopyTo(dstMs);

                    byte[] decompressedBytes = dstMs.ToArray();
                    json = Encoding.UTF8.GetString(decompressedBytes);
                }
                else
                {
                    json = File.ReadAllText(filename);
                }

                if (json[0] == '#')
                    ExtData = ParsePMAPIExtData(ref json);

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
            bool isUncompressed = filename.EndsWith("json");

            Logger.LogInfo($"Saving {(isUncompressed ? "uncompressed " : "")}{filename}");

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

                if (ExtData != null)
                    SavePMAPIExtData(ref json);

                if (!isUncompressed)
                {
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                    ms = new MemoryStream(jsonBytes);
                    dstFs = File.Create(filename);

                    gz = new GZipStream(dstFs, CompressionMode.Compress);

                    ms.CopyTo(gz);
                }
                else
                {
                    File.WriteAllText(filename, json);
                }

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

        private static ExtData ParsePMAPIExtData(ref string str)
        {
            int stopIndex = int.Parse(str.Substring(13, 8), System.Globalization.NumberStyles.HexNumber);
            string extJson = str[21..stopIndex];

            str = str[stopIndex..];

            return JsonConvert.DeserializeObject<ExtData>(extJson);
        }

        private static void SavePMAPIExtData(ref string str)
        {
            string extJson = JsonConvert.SerializeObject(ExtData);

            StringBuilder sb = new();
            sb.Append("#PMAPI_EXTDAT");
            sb.Append((21 + extJson.Length).ToString("X8"));
            sb.Append(extJson);
            sb.Append(str);
            str = sb.ToString();
        }
    }
}
