using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using MoneyFox.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace MoneyFox.Droid.Renderer
{
    public class CustomEntryRenderer : MaterialEntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context) {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e) {
            base.OnElementChanged(e);
            UpdateTextColor(Color.DarkKhaki);

            IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
            IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

            // my_cursor is the xml file name which we defined above
            JNIEnv.SetField(Control.EditText.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);
        }
    }
}