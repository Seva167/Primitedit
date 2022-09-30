using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Windows;

namespace PrimitierSaveEditor
{
    public class VisualProperty
    {
        public VisualProperty(string prop, object data, Func<string, Type, object> convFunc, Action callback, bool button = false)
        {
            Data = data;
            Property = prop;
            ConversionFunc = convFunc;
            Callback = callback;
            EnableButton = button ? Visibility.Visible : Visibility.Hidden;
        }

        private Func<string, Type, object> ConversionFunc { get; }

        private Action Callback { get; }

        public string Property { get; }

        public Visibility EnableButton { get; } = Visibility.Hidden;

        private object Data { get; }

        public object Value
        {
            get => Data.GetType().GetField(Property).GetValue(Data);
            set
            {
                FieldInfo field = Data.GetType().GetField(Property);

                try
                {
                    object res = ConversionFunc.Invoke(value as string, field.FieldType);
                    field.SetValue(Data, res);

                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.IsDirty = true;

                    Callback.Invoke();
                }
                catch (Exception)
                {

                }

            }
        }
    }
}
