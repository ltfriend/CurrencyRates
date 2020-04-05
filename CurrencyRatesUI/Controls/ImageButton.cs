using System;
using Xamarin.Forms;

namespace CurrencyRatesUI.Controls {
    public class ImageButton : Image {
        public static readonly BindableProperty BlendColorProperty = BindableProperty.Create(
            nameof(BlendColor), typeof(Color), typeof(ImageButton), Color.Default);

        public event EventHandler Clicked;
        public event EventHandler Released;

        public Color BlendColor {
            get { return (Color)GetValue(BlendColorProperty); }
            set { SetValue(BlendColorProperty, value); }
        }

        public ImageButton() {
            BlendColor = Color.FromRgba(255, 255, 255, 100);
        }

        public void SendClicked() {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        public void SendReleased() {
            Released?.Invoke(this, EventArgs.Empty);
        }
    }
}
