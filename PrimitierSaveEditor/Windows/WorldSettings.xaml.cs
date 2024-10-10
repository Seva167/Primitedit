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
using System.Linq;
using static PrimitierSaveEditor.Controllers.SaveController;

namespace PrimitierSaveEditor
{
    public partial class WorldSettings : Window
    {
        public WorldSettings()
        {
            InitializeComponent();
            primVer.Text = $"{Save.version[0]}.{Save.version[1]}.{Save.version[2]}";
            seed.Text = Save.seed.ToString();
            time.Text = Save.time.ToString();
            creativeMode.IsChecked = Save.isCreativeMode;
            horScale.Text = Save.terrainHorizontalScale.ToString();
            verScale.Text = Save.terrainVerticalScale.ToString();
        }

        private void PrimVer_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] versionTexts = primVer.Text.Split('.');
            for (int i = 0; i < versionTexts.Length; i++)
            {
                if (int.TryParse(versionTexts[i], out int res))
                {
                    Save.version[i] = res;
                }
            }

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;
        }

        private void Seed_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save.seed = int.TryParse(seed.Text, out int res) ? res : Save.seed;

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;
        }

        private void Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save.time = float.TryParse(time.Text, out float res) ? res : Save.time;

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;
        }

        private void CreativeMode_Click(object sender, RoutedEventArgs e)
        {
            Save.isCreativeMode = creativeMode.IsChecked.Value;

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;
        }

        private void HorScale_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save.terrainHorizontalScale = float.TryParse(horScale.Text, out float res) ? res : Save.terrainHorizontalScale;

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;
        }

        private void VerScale_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save.terrainVerticalScale = float.TryParse(verScale.Text, out float res) ? res : Save.terrainVerticalScale;

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;
        }
    }
}
