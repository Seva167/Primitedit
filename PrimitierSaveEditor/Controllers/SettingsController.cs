using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PrimitierSaveEditor.Controllers
{
    public static class SettingsController
    {
        static string SaveDir { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Primitedit\\";

        public static Settings AppSettings { get; private set; }

        static SettingsController()
        {
            if (!Directory.Exists(SaveDir))
                Directory.CreateDirectory(SaveDir);

            if (!File.Exists(SaveDir + "config.json"))
            {
                AppSettings = new Settings();
                Save();
            }

            Load();
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(AppSettings, Formatting.Indented);
            File.WriteAllText(SaveDir + "config.json", json);
        }

        static void Load()
        {
            string json = File.ReadAllText(SaveDir + "config.json");
            AppSettings = JsonConvert.DeserializeObject<Settings>(json);

            if (AppSettings == null)
            {
                AppSettings = new Settings();
                Save();
            }
        }


        public class Settings
        {
            public bool AutoSaveEnabled { get; set; } = true;

            public int AutoSaveInterval { get; set; } = 15;

            public bool AutoUpdateCheck { get; set; } = true;
        }
    }
}
