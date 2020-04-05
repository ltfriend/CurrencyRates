using Tizen.Applications;
using ElmSharp;

namespace CurrencyRatesWidget {
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Start widget");
            Elementary.Initialize();
            Elementary.ThemeOverlay();
            WidgetApplication app = new WidgetApplication(typeof(CurrencyRatesWidget));
            app.Run(args);
        }
    }
}
