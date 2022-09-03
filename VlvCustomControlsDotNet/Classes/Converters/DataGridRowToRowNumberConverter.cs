using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VlvCustomControlsDotNet.Classes.Converters
{
    /// <summary>
    /// Converts row number of datagrid to number
    /// </summary>
    [ValueConversion(typeof(DataGridRow), typeof(int))]
    public class DataGridRowToRowNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // avoid starting from 0 so index + 1
            if (value is DataGridRow dataGridRow)
                return dataGridRow.GetIndex() + 1;
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
