using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PrimitierSaveEditor
{
    public static class CameraCommands
    {
        static CameraCommands()
        {
            CameraToSelected = new RoutedCommand("CameraToSelected", typeof(MainWindow));
            CameraToPlayer = new RoutedCommand("CameraToPlayer", typeof(MainWindow));
        }
        public static RoutedCommand CameraToSelected { get; }
        public static RoutedCommand CameraToPlayer { get; }
    }
}
