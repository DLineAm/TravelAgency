using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Controls;
using System.Windows.Data;

namespace TravelAgency.Services
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string) value == "М" ? "Мужской" : "Женский";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem item)
            {
                var label = (Label) item.Content;
                var result = label.Content;
                return (string) result == "Мужской" ? "М" : "Ж";
            }
            else
            {
                return (string) value == "Мужской" ? "М" : "Ж";
            }
        }
    }
}