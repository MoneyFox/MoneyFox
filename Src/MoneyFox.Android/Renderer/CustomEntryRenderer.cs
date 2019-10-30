using System;
using Android.Content;
using MoneyFox.Droid.Renderer;
using NLog;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using Java.Lang.Reflect;
using Color = Xamarin.Forms.Color;
using Object = Java.Lang.Object;

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

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Control.EditText.SetTextCursorDrawable(Resource.Drawable.CustomCursor);
            }
            else {
                TrySetCursorPointerColor();
            }
        }


        private void TrySetCursorPointerColor()
        {
            try
            {
                var textViewTemplate = new TextView(Control.EditText.Context);

                Field field = textViewTemplate.Class.GetDeclaredField("mEditor");
                field.Accessible = true;
                Object editor = field.Get(Control.EditText);

                string[] fieldsNames = { "mTextSelectHandleLeftRes", "mTextSelectHandleRightRes", "mTextSelectHandleRes" };
                string[] drawableNames = { "mSelectHandleLeft", "mSelectHandleRight", "mSelectHandleCenter" };

                for (var index = 0; index < fieldsNames.Length && index < drawableNames.Length; index++)
                {
                    string
                        fieldName = fieldsNames[index],
                        drawableName = drawableNames[index];

                    field = textViewTemplate.Class.GetDeclaredField(fieldName);
                    field.Accessible = true;
                    int handle = field.GetInt(Control.EditText);

                    Drawable handleDrawable = Resources.GetDrawable(handle, null);

                    handleDrawable.SetColorFilter(Color.Accent.ToAndroid(), PorterDuff.Mode.SrcIn);

                    field = editor.Class.GetDeclaredField(drawableName);
                    field.Accessible = true;
                    field.Set(editor, handleDrawable);
                }

                IntPtr intPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(intPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(Control.EditText.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);

            } catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }
    }
}
