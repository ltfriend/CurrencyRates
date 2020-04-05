using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace CurrencyRatesUI.Utils {
    public class JsonDataFileReader<T> {
        private readonly string fileNameSpace;
        private readonly string fileName;

        public T Result { get; set; }

        public JsonDataFileReader(string fileNameSpace, string fileName) {
            this.fileNameSpace = fileNameSpace;
            this.fileName = fileName;
        }

        public virtual void Read() {
            var assembly = typeof(JsonDataFileReader<T>).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream(fileNameSpace + fileName))
            using (var streamReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(streamReader)) {
                var serializer = JsonSerializer.Create();
                Result = serializer.Deserialize<T>(reader);
            }
        }
    }
}
