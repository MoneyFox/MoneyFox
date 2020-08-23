using Android.Content;
using MoneyFox.Controls;
using MoneyFox.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ModalContentPage), typeof(ModalContentPageRenderer))]
namespace MoneyFox.Droid.Renderer
{
    public class ModalContentPageRenderer : PageRenderer
    {
        public ModalContentPageRenderer(Context context) : base(context)
        {
        }
    }
}