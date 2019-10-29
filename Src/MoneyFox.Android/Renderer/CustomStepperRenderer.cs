using Android.Content;
using Android.Graphics;
using Android.OS;
using MoneyFox.Droid.Renderer;
using MoneyFox.Presentation;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(Stepper), typeof(CustomStepperRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace MoneyFox.Droid.Renderer
{
    public class CustomStepperRenderer : MaterialStepperRenderer
    {
        /// <inheritdoc />
        public CustomStepperRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var color = (Color) App.Current.Resources["ButtonBackgroundColor"];

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    Control.GetChildAt(0).Background.SetColorFilter(new BlendModeColorFilter(color.ToAndroid(), BlendMode.Multiply));
                    Control.GetChildAt(1).Background.SetColorFilter(new BlendModeColorFilter(color.ToAndroid(), BlendMode.Multiply));
                }
                else
                {
                    Control.GetChildAt(0).Background.SetColorFilter(color.ToAndroid(), PorterDuff.Mode.Multiply);
                    Control.GetChildAt(1).Background.SetColorFilter(color.ToAndroid(), PorterDuff.Mode.Multiply);
                }
            }
        }
    }
}