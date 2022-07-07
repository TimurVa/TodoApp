using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ToDoApp.Helpers.Converters
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Collapsed;
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
            return (value != null) & (value == (object)Visibility.Visible);
        }
    }

    public class BoleanToVisibilityReversedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Visible;
            try
            {
                if (value != null)
                    visibility = (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
            catch { }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null) & (value == (object)Visibility.Collapsed);
        }
    }
}
