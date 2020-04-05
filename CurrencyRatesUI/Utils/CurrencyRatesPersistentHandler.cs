using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CurrencyRatesUI.Models;

namespace CurrencyRatesUI.Utils {
    public static class CurrencyRatesPersistentHandler {
        const string StorageFile = "currency_rates.json";

        public static IList<CurrencyRate> Deserialize() {
            var fileOpener = new FileOpener();
            using (var stream = fileOpener.OpenFile(StorageFile, FileMode.OpenOrCreate)) {
                if (stream.Length == 0) {
                    return null;
                }

                using (var streamReader = new StreamReader(stream))
                using (var reader = new JsonTextReader(streamReader)) {
                    var serializer = JsonSerializer.Create();
                    return serializer.Deserialize<IList<CurrencyRate>>(reader);
                }
            }
        }

        public static Task SerializeAsync(IList<CurrencyRate> rates) {
            return Task.Run(() => {
                var fileOpener = new FileOpener();
                using (var stream = fileOpener.OpenFile(StorageFile, FileMode.Create))
                using (var streamWriter = new StreamWriter(stream))
                using (var writer = new JsonTextWriter(streamWriter)) {
                    var serializer = JsonSerializer.Create();
                    serializer.Serialize(writer, rates);
                }
            });
        }
    }
}
