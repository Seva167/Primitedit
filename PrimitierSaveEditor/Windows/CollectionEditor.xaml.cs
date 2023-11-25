using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class CollectionEditor : Window
    {
        public IList List { get; }
        Type listType;

        public CollectionEditor(IList list)
        {
            List = list;
            listType = List.GetType().GenericTypeArguments[0];
            InitializeComponent();

            RegenerateItemSource();
        }

        void RegenerateItemSource()
        {
            ObservableCollection<CollectionProperty> properties = new ObservableCollection<CollectionProperty>();
            for (int i = 0; i < List.Count; i++)
            {
                properties.Add(new CollectionProperty(i, List, GetConvFunction()));
            }

            collTable.ItemsSource = properties;
        }

        private void AddElementClick(object sender, RoutedEventArgs e)
        {
            ObservableCollection<CollectionProperty> properties = collTable.ItemsSource as ObservableCollection<CollectionProperty>;

            properties.Add(new CollectionProperty(List.Add(GetDefault(listType)), List, GetConvFunction()));

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;
        }

        private void RemoveElementClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            CollectionProperty property = btn.DataContext as CollectionProperty;

            List.RemoveAt(property.Index);

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.IsDirty = true;

            RegenerateItemSource();
        }

        public static object GetDefault(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

        private Func<string, Type, object> GetConvFunction()
        {
            return listType.Name switch
            {
                "String" => Utils.ConvString,
                "Int32" => Utils.ConvInt,
                _ => null
            };
        }

    }
}
