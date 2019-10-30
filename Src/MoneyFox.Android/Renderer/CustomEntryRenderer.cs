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

            Control.EditText.SetHighlightColor(Color.Accent.ToAndroid());

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                TrySetCursorPointerColorNew();
            }
            else
            {
                TrySetCursorPointerColor();
            }
        }

        private void TrySetCursorPointerColorNew()
        {

            try
            {
                Control.EditText.SetTextCursorDrawable(Resource.Drawable.CustomCursor);

                var textSelectHandleDrawable = Control.EditText.TextSelectHandle;
                textSelectHandleDrawable.SetColorFilter(new BlendModeColorFilter(Color.Accent.ToAndroid(), BlendMode.SrcIn));
                Control.EditText.TextSelectHandle = textSelectHandleDrawable;

                var textSelectHandleLeftDrawable = Control.EditText.TextSelectHandleLeft;
                textSelectHandleLeftDrawable.SetColorFilter(new BlendModeColorFilter(Color.Accent.ToAndroid(), BlendMode.SrcIn));
                Control.EditText.TextSelectHandle = textSelectHandleLeftDrawable;

                var textSelectHandleRightDrawable = Control.EditText.TextSelectHandleRight;
                textSelectHandleRightDrawable.SetColorFilter(new BlendModeColorFilter(Color.Accent.ToAndroid(), BlendMode.SrcIn));
                Control.EditText.TextSelectHandle = textSelectHandleRightDrawable;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
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

                string[] fieldsNames =
                    {"mTextSelectHandleLeftRes", "mTextSelectHandleRightRes", "mTextSelectHandleRes"};
                string[] drawableNames = {"mSelectHandleLeft", "mSelectHandleRight", "mSelectHandleCenter"};

                for (var index = 0; index < fieldsNames.Length && index < drawableNames.Length; index++)
                {
                    string fieldName = fieldsNames[index];
                    string drawableName = drawableNames[index];

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
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }
    }
}