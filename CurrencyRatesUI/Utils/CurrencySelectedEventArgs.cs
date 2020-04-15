using System;
using CurrencyRatesUI.Models;

namespace CurrencyRatesUI.Utils {
    public class CurrencySelectedEventArgs : EventArgs {
        public Currency Currency { get; private set; }

        public CurrencySelectedEventArgs(Currency currency) {
            Currency = currency;
        }
    }
}
