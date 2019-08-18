using Android.Content;
using MoneyFox.Droid.Renderer;
using Xamarin.Forms;
using MoneyFox.Presentation.Controls;
using Xamarin.Forms.Platform.Android;
using FrameRenderer = Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer;

[assembly: ExportRenderer(typeof(CardView), typeof(CardViewRenderer))]
namespace MoneyFox.Droid.Renderer
{
    public class CardViewRenderer : FrameRenderer
    {
        public CardViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            this.Elevation = ((CardView)e.NewElement).ShadowRadius;
        }
    }
}
