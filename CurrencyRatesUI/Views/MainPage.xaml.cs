using System;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CurrencyRatesUI.Models;
using CurrencyRatesUI.ViewModels;

namespace CurrencyRatesUI.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CirclePage {
        public MainPage(MainPageModel viewModel) {
            BindingContext = viewModel;

            InitializeComponent();

            //TODO: Хорошо бы убирать кнопку меню, когда список пустой
            // (может, заполнять динамически, потому что методов скрыть не было найдено).

            CurrencyRatesModel.Instance.OnRatesUpdated += OnRatesUpdated;
        }

        private void OnAddButtonReleased(object sender, EventArgs e) {
            AddRate();
        }

        private void OnFooterAddButtonClicked(object sender, EventArgs e) {
            AddRate();
        }

        private void OnDeleteButtonClicked(object sender, EventArgs e) {
            if (RateListView.SelectedItem == null) {
                return;
            }

            var selectedRate = RateListView.SelectedItem as CurrencyRate;

            var popup = new TwoButtonPopup() {
                FirstButton = new MenuItem() {
                    IconImageSource = "ic_popup_btn_check.png",
                    IsDestructive = true
                },
                SecondButton = new MenuItem() {
                    IconImageSource = "ic_popup_btn_cancel.png"
                },
                Content = new Label {
                    Text = $"Delete rate\n{selectedRate}?", // TODO: Translate
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                }
            };
            popup.BackButtonPressed += (s, args) => { popup.Dismiss(); };
            popup.SecondButton.Clicked += (s, args) => { popup.Dismiss(); };
            popup.FirstButton.Clicked += (s, args) => {
                CurrencyRatesModel.Instance.DeleteCurrencyRate(selectedRate);
                popup.Dismiss();
            };
            popup.Show();
        }

        private async void OnRefreshTapped(object sender, EventArgs e) {
            (BindingContext as MainPageModel).IsNotRefreshing = false;
            await CurrencyRatesModel.Instance.RefreshRatesAsync();
        }

        private async void AddRate() {
            await Navigation.PushAsync(PageController.GetInstance(Pages.AddRatePage));
        }

        private void OnRatesUpdated(object sender, EventArgs e) {
            MainPageModel model = BindingContext as MainPageModel;
            model.Updated = CurrencyRatesModel.Instance.Updated;
            model.IsNotRefreshing = true;
        }
    }
}