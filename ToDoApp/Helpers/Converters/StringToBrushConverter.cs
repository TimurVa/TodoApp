using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ToDoApp.Helpers.Converters
{
    class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.Transparent;

            if (value is string str)
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString(str);
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
