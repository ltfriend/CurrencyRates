using System;
using System.Globalization;
using Xamarin.Forms;

namespace CurrencyRatesUI.Converters {
    public enum MainPageType {
        Empty,
        ListView,
        Button
    }

    class ItemCountToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int count = System.Convert.ToInt32(value);
            MainPageType pageType = (MainPageType)parameter;
            bool flag;
            switch (pageType) {
                case MainPageType.ListView:
                case MainPageType.Button:
                    flag = false;
                    break;
                case MainPageType.Empty:
                default:
                    flag = true;
                    break;
            }

            if (count > 0) {
                flag = !flag;
            }

            return System.Convert.ToBoolean(flag);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
