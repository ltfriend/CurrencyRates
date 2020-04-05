using Tizen.Applications;
using ElmSharp;

namespace CurrencyRatesWidget {
    public class CurrencyRatesWidget : WidgetBase {
        public override void OnCreate(Bundle content, int w, int h) {
            base.OnCreate(content, w, h);
            MakeUI();
        }

        public override void OnPause() {
            base.OnPause();
        }

        public override void OnResume() {
            base.OnResume();
        }

        public override void OnResize(int w, int h) {
            base.OnResize(w, h);
        }

        public override void OnUpdate(Bundle content, bool isForce) {
            base.OnUpdate(content, isForce);
        }

        public override void OnDestroy(WidgetBase.WidgetDestroyType reason, Bundle content) {
            base.OnDestroy(reason, content);
        }

        void MakeUI() {
            Conformant conformant = new Conformant(Window);
            conformant.Show();

            Scroller scroller = new Scroller(Window) {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
                ScrollBlock = ScrollBlock.None
            };
            scroller.Show();

            Box box = new Box(Window) {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1
            };
            box.Show();
            scroller.SetContent(box);
            conformant.SetContent(scroller);

            Label label = new Label(Window) {
                Text = "Welcome to Xamarin Forms!"
            };
            box.PackEnd(label);
            label.Show();
        }
    }
}