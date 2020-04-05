using Xamarin.Forms;
using CurrencyRatesUI.Views;
using CurrencyRatesUI.ViewModels;

namespace CurrencyRatesUI {
    public enum Pages {
        MainPage,
        AddRatePage
    }

    public static class PageController {
        static MainPage MainPage;
        static AddRatePage AddRatePage;

        public static Page GetInstance(Pages page, object o = null) {
            switch (page) {
                case Pages.MainPage:
                    return MainPage ?? (MainPage = new MainPage(o as MainPageModel));
                case Pages.AddRatePage:
                    AddRatePage?.Clear();
                    return AddRatePage ?? (AddRatePage = new AddRatePage());
                default:
                    return null;
            }
        }
    }
}
