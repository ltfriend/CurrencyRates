using System;
using System.Globalization;
using Xamarin.Forms;

namespace CurrencyRatesUI.Converters {
    class ChangeRateToColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            double change = System.Convert.ToDouble(value);
            return change < 0 ? Color.Red : Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
