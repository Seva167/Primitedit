using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow(string url, string tag, string name, string body)
        {
            InitializeComponent();
            this.url = url;

            StringBuilder sb = new StringBuilder();
            sb.Append("Primitedit ");
            sb.AppendLine(tag);

            sb.AppendLine(name);
            sb.AppendLine();

            sb.Append(body);

            updateText.Text = sb.ToString();
        }

        private string url;

        private void Nyet(object sender, RoutedEventArgs e) => Close();

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            Process link = new Process();
            link.StartInfo.FileName = url;
            link.StartInfo.UseShellExecute = true;
            link.Start();

            Close();
        }
    }
}
