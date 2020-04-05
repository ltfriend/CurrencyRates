using System.Net;
using System.Net.Http;

namespace CurrencyRatesUI.Utils {
    public class HttpException : HttpRequestException {
        public HttpStatusCode StatusCode { get; }

        public HttpException(HttpStatusCode statusCode) {
            StatusCode = statusCode;
        }

        public HttpException() { }

        public HttpException(string message) : base(message) { }

        public HttpException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
