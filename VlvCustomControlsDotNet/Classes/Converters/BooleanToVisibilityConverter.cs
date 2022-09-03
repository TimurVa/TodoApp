using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VlvCustomControlsDotNet.Classes.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Visible;

            try
            {
                if (value != null)
                    visibility = (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
