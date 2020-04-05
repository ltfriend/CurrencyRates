using System;
using ElmSharp;
using Xamarin.Forms.Platform.Tizen;
using CurrencyRatesUI.Controls;

[assembly: Xamarin.Forms.ExportRenderer(typeof(ImageButton), typeof(CurrencyRatesUI.Renderers.ImageButtonRenderer))]
namespace CurrencyRatesUI.Renderers {
    class ImageButtonRenderer : ImageRenderer {
        private GestureLayer GestureRecognizer;
        private volatile bool isTouched;

        public ImageButtonRenderer() {
            RegisterPropertyHandler(ImageButton.BlendColorProperty, UpdateBlendColor);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> args) {
            base.OnElementChanged(args);

            if (Control == null) {
                return;
            }

            if (GestureRecognizer == null) {
                GestureRecognizer = new GestureLayer(Control);
                GestureRecognizer.Attach(Control);
            }

            if (args.NewElement == null) {
                GestureRecognizer.ClearCallbacks();
                return;
            } else {
                Control.Clicked += SendClicked;
            }

            if (!(args.NewElement is ImageButton button)) {
                return;
            }

            GestureRecognizer.SetTapCallback(GestureLayer.GestureType.Tap, GestureLayer.GestureState.Start, x => {
                KeyDown();
            });
            GestureRecognizer.SetTapCallback(GestureLayer.GestureType.Tap, GestureLayer.GestureState.End, x => {
                KeyUp();
            });
            GestureRecognizer.SetTapCallback(GestureLayer.GestureType.LongTap, GestureLayer.GestureState.End, x => {
                KeyUp();
            });
            GestureRecognizer.SetTapCallback(GestureLayer.GestureType.LongTap, GestureLayer.GestureState.Abort, x => {
                KeyUp();
            });
        }

        private void KeyDown() {
            if (Control == null || !(Element is ImageButton button)) {
                return;
            }

            Control.Color = button.BlendColor.ToNative();
            isTouched = true;
        }

        private void KeyUp() {
            if (Control == null) {
                return;
            }

            if (isTouched) {
                ((ImageButton)Element).SendReleased();
            }

            Control.Color = Color.Default;
            isTouched = false;
        }

        private void UpdateBlendColor(bool obj) {
            Control.Color = (Element as ImageButton).BlendColor.ToNative();
        }

        private void SendClicked(object sender, EventArgs e) {
            ((ImageButton)Element).SendClicked();
        }
    }
}
