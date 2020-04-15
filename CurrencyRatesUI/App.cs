using System;
using Xamarin.Forms;
using CurrencyRatesUI.Models;
using CurrencyRatesUI.Views;
using CurrencyRatesUI.ViewModels;

namespace CurrencyRatesUI {
    //TODO: Перевести на русский язык.
    public partial class App : Application {
        readonly MainPage firstPage;

        public App() {
            CurrencyRatesModel.Initialize();
            firstPage = (MainPage)PageController.GetInstance(Pages.MainPage, new MainPageModel());
            MainPage = new NavigationPage(firstPage);
        }

        protected override void OnStart() {
            base.OnStart();
            //TODO: Добавить обновление по расписанию?

            if (CurrencyRatesModel.Instance.CurrencyRateList.Count > 0) {
                RefreshRates();
            }
        }

        private async void RefreshRates() {
            DateTime now = DateTime.UtcNow;
            TimeSpan updateRatesTime = CurrencyRatesModel.Instance.UpdateRatesTime;

            DateTime nextUpdateDate = new DateTime(now.Year, now.Month, now.Day,
                updateRatesTime.Hours, updateRatesTime.Minutes, updateRatesTime.Seconds);
            DateTime prevUpdateDate = nextUpdateDate.AddDays(-1);
            DateTime lastUpdateDate = CurrencyRatesModel.Instance.Updated.ToUniversalTime();

            if ((now > nextUpdateDate && nextUpdateDate > lastUpdateDate) || lastUpdateDate < prevUpdateDate) {
                await CurrencyRatesModel.Instance.RefreshRatesAsync();
            }
        }
    }
}
