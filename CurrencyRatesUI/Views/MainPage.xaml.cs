using System;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;
using CurrencyRatesUI.Models;
using CurrencyRatesUI.ViewModels;

namespace CurrencyRatesUI.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CirclePage {
        public MainPage(MainPageModel viewModel) {
            BindingContext = viewModel;

            InitializeComponent();

            CurrencyRatesModel.Instance.OnRatesUpdated += OnRatesUpdated;
        }

        private void OnAddButtonReleased(object sender, EventArgs e) {
            AddRate();
        }

        private void OnFooterAddButtonClicked(object sender, EventArgs e) {
            AddRate();
        }

        private async void OnRefreshTapped(object sender, EventArgs e) {
            await CurrencyRatesModel.Instance.RefreshRatesAsync();
        }

        private async void AddRate() {
            await Navigation.PushAsync(PageController.GetInstance(Pages.AddRatePage));
        }

        private void OnRatesUpdated(object sender, EventArgs e) {
            ((MainPageModel)BindingContext).Updated = DateTime.Now;
        }
    }
}