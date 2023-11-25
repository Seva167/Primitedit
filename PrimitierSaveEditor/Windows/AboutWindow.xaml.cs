using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void OpenGithubClick(object sender, RoutedEventArgs e) => Utils.OpenLink("https://github.com/Seva167");

        public string Version => "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
    }
}
