﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace VlvCustomControlsDotNet.Classes.Converters
{

    public class AdditionalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value != null) && (parameter != null))
            {
                var firstValue = (double)value;
                var secondValue = double.Parse(parameter as string);

                return firstValue + secondValue;
            }

            return 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
