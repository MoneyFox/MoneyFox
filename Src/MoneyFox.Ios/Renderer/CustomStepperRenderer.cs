using MoneyFox.iOS.Renderer;
using MoneyFox.Presentation;
using Xamarin.Forms;
using Xamarin.Forms.Material.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Stepper), typeof(CustomStepperRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace MoneyFox.iOS.Renderer
{
    public class CustomStepperRenderer : MaterialStepperRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var color = (Color)App.Current.Resources["ButtonBackgroundColor"];

                Control.TintColor = color.ToUIColor();
            }
        }
    }
}