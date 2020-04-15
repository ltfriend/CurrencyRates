using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using Tizen.Network.Connection;
using CurrencyRatesUI.Utils;

using TizenPreference = Tizen.Applications.Preference;

namespace CurrencyRatesUI.Models {
    public class CurrencyRatesModel {
        private const string MainCurrencyCode = "643";
        private const string RequestUriTemplate = "http://cbrates.rbc.ru/tsv/{0}/{1}.tsv";
        public readonly TimeSpan UpdateRatesTime = new TimeSpan(10, 0, 0);

        public IList<CurrencyRate> CurrencyRateList { get; set; }
        public ObservableCollection<CurrencyRate> ObservableCurrencyRateList { get; set; }

        public static CurrencyRatesModel Instance { get; private set; }

        public event EventHandler OnRatesUpdated;

        private DateTime _updated;
        public DateTime Updated {
            get => _updated;
            private set {
                if (_updated != value) {
                    _updated = value;
                    TizenPreference.Set("Updated", string.Format("{0:yyyyMMddTHH:mm:ss}", value));
                }
            }
        }

        public bool IsRefreshing { get; private set; }

        public static void Initialize() {
            Instance = new CurrencyRatesModel {
                ObservableCurrencyRateList = new ObservableCollection<CurrencyRate>(),
                CurrencyRateList = CurrencyRatesPersistentHandler.Deserialize(),
                IsRefreshing = false
            };

            if (TizenPreference.Contains("Updated")) {
                string updatedStr = TizenPreference.Get<string>("Updated");
                Instance._updated = DateTime.ParseExact(updatedStr, "yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture);
            }

            if (Instance.CurrencyRateList == null) {
                Instance.CurrencyRateList = new List<CurrencyRate>();
            } else {
                foreach (var rate in Instance.CurrencyRateList) {
                    Instance.ObservableCurrencyRateList.Add(rate);
                }
            }
        }

        public void AddCurrencyRate(CurrencyRate rate) {
            ObservableCurrencyRateList.Add(rate);
            CurrencyRateList.Add(rate);

            SaveRateList();

            Task.Run(async () => {
                bool success = false;
                //TODO: обработка ошибок.
                try {
                    await LoadCurrencyRateAsync(rate);
                    SaveRateList();
                    success = true;
                } catch (HttpException ex) {
                    Console.WriteLine($"[Load currency rate {rate} failed] HTTP status code: {ex.StatusCode}");
                } catch (InvalidRateResponseException ex) {
                    Console.WriteLine($"[Load currency rate {rate} failed] {ex.Message}");
                } catch (HttpRequestException ex) {
                    Console.WriteLine($"[Load currency rate {rate} failed] {ex.Message}");
                }

                RatesUpdated(success);
            });
        }

        public void DeleteCurrencyRate(CurrencyRate rate) {
            CurrencyRateList.Remove(rate);
            ObservableCurrencyRateList.Remove(rate);
            SaveRateList();
        }

        public async Task RefreshRatesAsync() {
            bool wereErrors = false;
            IsRefreshing = true;

            foreach (var rate in ObservableCurrencyRateList) {
                //TODO: обработка ошибок.
                try {
                    await LoadCurrencyRateAsync(rate);
                } catch (HttpException ex) {
                    Console.WriteLine($"[Load currency rate {rate} failed] HTTP status code: {ex.StatusCode}");
                    wereErrors = true;
                } catch (InvalidRateResponseException ex) {
                    Console.WriteLine($"[Load currency rate {rate} failed] {ex.Message}");
                    wereErrors = true;
                } catch (HttpRequestException ex) {
                    Console.WriteLine($"[Load currency rate {rate} failed] {ex.Message}");
                    wereErrors = true;
                }
            }

            if (!wereErrors) {
                SaveRateList();
            }

            RatesUpdated(!wereErrors);
        }

        private async Task LoadCurrencyRateAsync(CurrencyRate currencyRate) {
            DateTime nowDate = DateTime.UtcNow;
            bool tomorrow = false;

            if (nowDate.TimeOfDay > UpdateRatesTime) {
                // Должный быть доступны курсы на завтрашний день.
                tomorrow = true;
                nowDate += TimeSpan.FromDays(1);
            }

            bool rateExists = false;

            while (!rateExists) {
                try {
                    double sourceRate = currencyRate.Source.Code == MainCurrencyCode ?
                        1 : await GetCurrencyRateAsync(currencyRate.Source.Code, nowDate);
                    double destRate = currencyRate.Dest.Code == MainCurrencyCode ?
                        1 : await GetCurrencyRateAsync(currencyRate.Dest.Code, nowDate);

                    currencyRate.Rate = destRate == 0 ? 0 : sourceRate / destRate;
                    rateExists = true;
                } catch (HttpException ex) {
                    if (ex.StatusCode == HttpStatusCode.NotFound && tomorrow) {
                        // Курсы на завтрашний день отсутствуют. Необходимо загрузить сегодняшние курсы.
                        nowDate -= TimeSpan.FromDays(1);
#pragma warning disable IDE0059 // Ненужное присваивание значения - На самом деле оно нужное! IDE "тупит".
                        tomorrow = false;
#pragma warning restore IDE0059 // Ненужное присваивание значения
                    } else {
                        throw ex;
                    }
                }
            }

            DateTime prevDate = nowDate - TimeSpan.FromDays(1);

            double prevSourceRate = currencyRate.Source.Code == MainCurrencyCode ?
                1 : await GetCurrencyRateAsync(currencyRate.Source.Code, prevDate);
            double prevDestRate = currencyRate.Dest.Code == MainCurrencyCode ?
                1 : await GetCurrencyRateAsync(currencyRate.Dest.Code, prevDate);
            currencyRate.PrevRate = prevDestRate == 0 ? 0 : prevSourceRate / prevDestRate;
        }

        private void SaveRateList() {
            CurrencyRatesPersistentHandler.SerializeAsync(CurrencyRateList).Wait();
        }

        private async Task<double> GetCurrencyRateAsync(string code, DateTime date) {
            HttpClient httpClient;
            string proxyAddr = ConnectionManager.GetProxy(AddressFamily.IPv4);

            if (string.IsNullOrEmpty(proxyAddr)) {
                httpClient = new HttpClient();
            } else {
                var httpClientHandler = new HttpClientHandler() { Proxy = new WebProxy(proxyAddr) };
                httpClient = new HttpClient(httpClientHandler, true);
            }

            var requestUri = string.Format(RequestUriTemplate, code,
                date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
            var response = await httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode) {
                throw new HttpException(response.StatusCode);
            }

            string content = await response.Content.ReadAsStringAsync();
            int pos = content.IndexOf('\t');
            if (pos == -1) {
                throw new InvalidRateResponseException("Invalid response format");
            }

            try {
                return double.Parse(content.Substring(pos + 1), CultureInfo.InvariantCulture);
            } catch (FormatException) {
                throw new InvalidRateResponseException("Rate is not number");
            }
        }
 
        private void RatesUpdated(bool success) {
            if (success) {
                Updated = DateTime.Now;
            }

            IsRefreshing = false;
            OnRatesUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}