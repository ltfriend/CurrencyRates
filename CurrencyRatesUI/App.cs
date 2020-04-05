using Xamarin.Forms;
using CurrencyRatesUI.Models;
using CurrencyRatesUI.Views;
using CurrencyRatesUI.ViewModels;

namespace CurrencyRatesUI {
    public partial class App : Application {
        readonly MainPage firstPage;

        public App() {
            CurrencyRatesModel.Initialize();
            firstPage = (MainPage)PageController.GetInstance(Pages.MainPage, new MainPageModel());
            MainPage = new NavigationPage(firstPage);
        }

        protected override void OnStart() {
            base.OnStart();
            //TODO: Обновлять по расписанию, а не при запуске. Или при запуске, если давно не обновлялось.
            // Т.к. курс меняется раз в день, то нет необходимости "лезть в интернет" при каждом запуске.
            RefreshRates();
        }

        private async void RefreshRates() {
            await CurrencyRatesModel.Instance.RefreshRatesAsync();
        }
    }
}
