using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PrimitierSaveEditor.Controllers
{
    public static class SettingsController
    {
        public static Settings AppSettings { get; private set; }

        static SettingsController()
        {
            if (!File.Exists(App.SettDir + "config.json"))
            {
                AppSettings = new Settings();
                Save();
            }

            Load();
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(AppSettings, Formatting.Indented);
            File.WriteAllText(App.SettDir + "config.json", json);
        }

        static void Load()
        {
            string json = File.ReadAllText(App.SettDir + "config.json");
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
