using System;
using System.Net.Http;

namespace CurrencyRatesUI.Utils {
    public class InvalidRateResponseException : HttpRequestException {
        public InvalidRateResponseException() { }

        public InvalidRateResponseException(string message) : base(message) { }

        public InvalidRateResponseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
