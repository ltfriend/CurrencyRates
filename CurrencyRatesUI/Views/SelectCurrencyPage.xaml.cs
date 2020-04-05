using System;
using System.Linq;
using System.Collections.Generic;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CurrencyRatesUI.Models;
using CurrencyRatesUI.Utils;

namespace CurrencyRatesUI.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCurrencyPage : CirclePage {
        public event EventHandler<CurrencySelectedEventArgs> CurrencySelected;

        public SelectCurrencyPage() {
            InitializeComponent();
            LoadCurrencyList();
        }

        private void LoadCurrencyList() {
            var jsonReader = new JsonDataFileReader<IList<Currency>>("CurrencyRatesUI.Data.", "currencies.json");
            jsonReader.Read();
            list.ItemsSource = jsonReader.Result.AsQueryable();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            CurrencySelected?.Invoke(this, new CurrencySelectedEventArgs(e.SelectedItem as Currency));
            await Navigation.PopModalAsync();
        }
    }
}