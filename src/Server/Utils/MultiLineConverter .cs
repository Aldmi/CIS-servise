using System;
using System.Globalization;
using System.Windows.Data;

namespace Server.Utils
{
    [ValueConversion(typeof(string), typeof(string))]
    public class MultiLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Replace(':', '\n');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Replace('\n', ':');
        }
    }
}