using Newtonsoft.Json;

namespace CurrencyRatesUI.Models {
    public class Currency {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "CodeSymbol")]
        public string CodeSymbol { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
