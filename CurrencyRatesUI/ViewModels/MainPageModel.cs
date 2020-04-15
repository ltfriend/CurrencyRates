using System;
using System.Collections.ObjectModel;
using CurrencyRatesUI.Models;

namespace CurrencyRatesUI.ViewModels {
    public class MainPageModel : ViewModelBase {
        public ObservableCollection<CurrencyRate> RateList =>
            CurrencyRatesModel.Instance.ObservableCurrencyRateList;

        private DateTime _updated;
        public DateTime Updated {
            get => _updated;
            set => SetProperty(ref _updated, value);
        }

        private bool _isNotRefreshing;
        public bool IsNotRefreshing {
            get => _isNotRefreshing;
            set => SetProperty(ref _isNotRefreshing, value);
        }

        public MainPageModel() {
            Updated = CurrencyRatesModel.Instance.Updated;
            IsNotRefreshing = !CurrencyRatesModel.Instance.IsRefreshing;
        }
    }
}
