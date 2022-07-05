using System;
using System.Globalization;
using System.Windows.Data;

namespace TravelAgency.Services
{
    public class DaysConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (int) value;
            number %= 100;
            number = number % 100;

            if (number >= 11 && number <= 19)
            {
                return value + " дней";
            }

            var i = number % 10;
            switch (i)
            {
                case 1:
                    return value + " день";
                case 2:
                case 3:
                case 4:
                    return value + " дня";
                default:
                    return value + " дней";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}