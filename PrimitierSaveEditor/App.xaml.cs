using PrimitierSaveEditor.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PrimitierSaveEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string SettDir { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Primitedit\\";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!Directory.Exists(SettDir))
                Directory.CreateDirectory(SettDir);

            AppDomain.CurrentDomain.UnhandledException += (s, e) => Logger.LogExc(e.ExceptionObject);
            Logger.LogInfo("Primitedit started");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Logger.LogInfo($"Program shut down with exit code {e.ApplicationExitCode}");
        }
    }
}
