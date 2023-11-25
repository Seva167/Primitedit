using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PrimitierSaveEditor
{
    public static class MenuCommands
    {
        static MenuCommands()
        {
            Open = new RoutedCommand("Open", typeof(MainWindow));
            Save = new RoutedCommand("Save", typeof(MainWindow));
            SaveAs = new RoutedCommand("SaveAs", typeof(MainWindow));
        }
        public static RoutedCommand Open { get; }
        public static RoutedCommand Save { get; }
        public static RoutedCommand SaveAs { get; }
    }
}
