
using MoneyFox.Controls;
using MoneyFox.Droid.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(handler: typeof(ModalContentPage), target: typeof(ModalContentPageRenderer))]

namespace MoneyFox.Droid.Renderer
{

    using Android.Content;
    using Xamarin.Forms.Platform.Android;

    public class ModalContentPageRenderer : PageRenderer
    {
        public ModalContentPageRenderer(Context context) : base(context) { }
    }
}
