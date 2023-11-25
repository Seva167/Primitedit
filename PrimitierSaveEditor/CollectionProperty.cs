using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PrimitierSaveEditor
{
    public class CollectionProperty
    {
        public CollectionProperty(int index, IList list, Func<string, Type, object> convFunc)
        {
            Index = index;
            List = list;
            ConversionFunc = convFunc;
        }

        public int Index { get; }

        public IList List { get; }

        private Func<string, Type, object> ConversionFunc { get; }

        public object Value
        {
            get => List[Index];
            set
            {
                try
                {
                    object res = ConversionFunc.Invoke(value as string, null);
                    List[Index] = res;

                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.IsDirty = true;
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
