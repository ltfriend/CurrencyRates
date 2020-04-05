using System;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;
using CurrencyRatesUI.Models;
using CurrencyRatesUI.Utils;

namespace CurrencyRatesUI.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRatePage : CirclePage {
        private Currency firstCurrency;
        private Currency secondCurrency;

        public AddRatePage() {
            InitializeComponent();
        }

        public void Clear() {
            firstCurrency = null;
            secondCurrency = null;

            firstCurrencyLabel.Text = "";
            secondCurrencyLabel.Text = "";
        }

        private async void OnFirstCurrencyTapped(object sender, EventArgs e) {
            var page = new SelectCurrencyPage();
            page.CurrencySelected += OnFirstCurrencySelected;
            await Navigation.PushModalAsync(page);
        }

        private async void OnSecondCurrencyTapped(object sender, EventArgs e) {
            var page = new SelectCurrencyPage();
            page.CurrencySelected += OnSecondCurrencySelected;
            await Navigation.PushModalAsync(page);
        }

        private void OnFirstCurrencySelected(object sender, CurrencySelectedEventArgs e) {
            firstCurrency = e.Currency;
            firstCurrencyLabel.Text = firstCurrency.CodeSymbol;
        }

        private void OnSecondCurrencySelected(object sender, CurrencySelectedEventArgs e) {
            secondCurrency = e.Currency;
            secondCurrencyLabel.Text = secondCurrency.CodeSymbol;
        }

        private async void OnAddRate(object sender, EventArgs e) {
            if ((firstCurrency != null) && (secondCurrency != null)) {
                var rate = new CurrencyRate {
                    Source = firstCurrency,
                    Dest = secondCurrency
                };
                CurrencyRatesModel.Instance.AddCurrencyRate(rate);
            }

            await Navigation.PopAsync();
        }
    }
}