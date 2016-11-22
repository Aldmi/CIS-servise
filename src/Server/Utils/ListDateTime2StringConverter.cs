using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Windows;
using System.Windows.Data;


namespace Server.Utils
{
    public class ListDateTime2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //распарсить строку в список дат
            var dateString = value as string;

            var newLL= new ObservableCollection<DateTime>    {
                new DateTime(2016,11,20),
                new DateTime(2016,11,21),
                new DateTime(2016,11,22),
                new DateTime(2016,11,28),
            };

            return newLL;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var listDate = value as IEnumerable<DateTime>;
            var dateString = "iiiiiii";

            return dateString;
        }
    }
}