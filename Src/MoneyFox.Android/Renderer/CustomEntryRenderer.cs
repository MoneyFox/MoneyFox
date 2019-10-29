using Android.Content;
using MoneyFox.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer), new[] {typeof(VisualMarker.MaterialVisual)})]
namespace MoneyFox.Droid.Renderer
{
    public class CustomEntryRenderer : MaterialEntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //Control.EditText.SetTextCursorDrawable(Resource.Drawable.CustomCursor);
        }
    }
}
