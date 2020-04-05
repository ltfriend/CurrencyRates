using System;
using CurrencyRatesUI.Models;

//TODO: Проверить нужен ли этот класс?

namespace CurrencyRatesUI.Utils {
    public class CurrencySelectedEventArgs : EventArgs {
        public Currency Currency { get; private set; }

        public CurrencySelectedEventArgs(Currency currency) {
            Currency = currency;
        }
    }
}
