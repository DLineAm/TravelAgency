using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace TravelAgency.Services
{
    class SearchCountToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var result = (int)value;
            result %= 100;

            if (result >= 11 && result <= 19)
            {
                return "Найдено " + value + " результатов";
            }

            var i = result % 10;
            switch (i)
            {
                case 1:
                    return "Найден " + value + " результат";
                case 2:
                case 3:
                case 4:
                    return "Найдено " + value + " результата";
                default:
                    return "Найдено " + value + " результатов";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
