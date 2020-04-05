using System.ComponentModel;
using Newtonsoft.Json;

namespace CurrencyRatesUI.Models {
    public class CurrencyRate : INotifyPropertyChanged {
        private Currency _source;
        [JsonProperty(PropertyName = "Source")]
        public Currency Source {
            get { return _source; }
            set {
                if (_source != value) {
                    _source = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
                }
            }
        }

        private Currency _dest;
        [JsonProperty(PropertyName = "Dest")]
        public Currency Dest {
            get { return _dest; }
            set {
                if (_dest != value) {
                    _dest = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dest)));
                }
            }
        }

        private double _rate;
        [JsonProperty(PropertyName = "Rate")]
        public double Rate {
            get { return _rate; }
            set {
                if (_rate != value) {
                    _rate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rate)));
                }
            }
        }

        private bool _isFavorite;
        [JsonProperty(PropertyName = "IsFavorite")]
        public bool IsFavorive {
            get { return _isFavorite; }
            set {
                if (_isFavorite != value) {
                    _isFavorite = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFavorive)));
                }
            }
        }

        [JsonIgnore]
        public CurrencyRate Self => this;

        public override string ToString() {
            return $"{Source.CodeSymbol} / {Dest.CodeSymbol}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}