using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TravelAgency.Services
{
    public class ChangeLabelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = (Visibility)value;
            return prop == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}