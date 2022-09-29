using PrimitierSaveEditor.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrimitierSaveEditor
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        public bool AutoSave
        {
            get => SettingsController.AppSettings.AutoSaveEnabled;
            set
            {
                SettingsController.AppSettings.AutoSaveEnabled = value;
                SettingsController.Save();
            }
        }

        public int AutoSaveInterval
        {
            get => SettingsController.AppSettings.AutoSaveInterval;
            set
            {
                SettingsController.AppSettings.AutoSaveInterval = value;
                SettingsController.Save();
            }
        }

        public bool UpdateCheck
        {
            get => SettingsController.AppSettings.AutoUpdateCheck;
            set
            {
                SettingsController.AppSettings.AutoUpdateCheck = value;
                SettingsController.Save();
            }
        }

        private void OKClick(object sender, RoutedEventArgs e) => Close();
    }
}
