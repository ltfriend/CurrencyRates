using System;
using System.Globalization;
using Xamarin.Forms;

namespace CurrencyRatesUI.Converters {
    class ChangeRateToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            double change = System.Convert.ToDouble(value);

            if (change == 0) {
                return $"{change:F4}";
            } else {
                char changeIcon = change < 0 ? '↓' : '↑';
                return $"{changeIcon} {change:F4}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
