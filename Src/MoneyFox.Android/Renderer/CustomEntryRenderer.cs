using MoneyFox.Droid.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(handler: typeof(Entry), target: typeof(CustomEntryRenderer), supportedVisuals: new[] { typeof(VisualMarker.MaterialVisual) })]

namespace MoneyFox.Droid.Renderer
{

    using System;
    using Android.Content;
    using Android.Graphics;
    using Android.OS;
    using Android.Runtime;
    using Android.Widget;
    using Serilog;
    using Xamarin.Forms;
    using Xamarin.Forms.Material.Android;
    using Xamarin.Forms.Platform.Android;
    using Color = Xamarin.Forms.Color;
    using Resource = Droid.Resource;

    public class CustomEntryRenderer : MaterialEntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control.EditText.SetHighlightColor(Color.Accent.ToAndroid());
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
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
                if (textSelectHandleDrawable != null)
                {
                    textSelectHandleDrawable.SetColorFilter(new BlendModeColorFilter(color: Color.Accent.ToAndroid(), mode: BlendMode.SrcIn!));
                    Control.EditText.TextSelectHandle = textSelectHandleDrawable;
                }

                var textSelectHandleLeftDrawable = Control.EditText.TextSelectHandleLeft;
                if (textSelectHandleLeftDrawable != null)
                {
                    textSelectHandleLeftDrawable.SetColorFilter(new BlendModeColorFilter(color: Color.Accent.ToAndroid(), mode: BlendMode.SrcIn!));
                    Control.EditText.TextSelectHandle = textSelectHandleLeftDrawable;
                }

                var textSelectHandleRightDrawable = Control.EditText.TextSelectHandleRight;
                if (textSelectHandleRightDrawable != null)
                {
                    textSelectHandleRightDrawable.SetColorFilter(new BlendModeColorFilter(color: Color.Accent.ToAndroid(), mode: BlendMode.SrcIn!));
                    Control.EditText.TextSelectHandle = textSelectHandleRightDrawable;
                }
            }
            catch (Exception ex)
            {
                Log.Error(exception: ex, messageTemplate: "Error creating in the CustomEntry Renderer");
            }
        }

        private void TrySetCursorPointerColor()
        {
            try
            {
                var textViewTemplate = new TextView(Control.EditText.Context);
                var field = textViewTemplate.Class.GetDeclaredField("mEditor");
                field.Accessible = true;
                var editor = field.Get(Control.EditText);
                if (editor == null)
                {
                    return;
                }

                string[] fieldsNames = { "mTextSelectHandleLeftRes", "mTextSelectHandleRightRes", "mTextSelectHandleRes" };
                string[] drawableNames = { "mSelectHandleLeft", "mSelectHandleRight", "mSelectHandleCenter" };
                for (var index = 0; index < fieldsNames.Length && index < drawableNames.Length; index++)
                {
                    var fieldName = fieldsNames[index];
                    var drawableName = drawableNames[index];
                    field = textViewTemplate.Class.GetDeclaredField(fieldName);
                    field.Accessible = true;
                    var handle = field.GetInt(Control.EditText);
                    var handleDrawable = Resources?.GetDrawable(id: handle, theme: null);
                    if (handleDrawable == null)
                    {
                        return;
                    }

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        handleDrawable.SetColorFilter(new BlendModeColorFilter(color: Color.Accent.ToAndroid(), mode: BlendMode.SrcIn!));
                    }
                    else
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        handleDrawable.SetColorFilter(color: Color.Accent.ToAndroid(), mode: PorterDuff.Mode.SrcIn!);
#pragma warning restore CS0618 // Type or member is obsolete
                    }

                    field = editor.Class.GetDeclaredField(drawableName);
                    field.Accessible = true;
                    field.Set(obj: editor, value: handleDrawable);
                }

                var intPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                var mCursorDrawableResProperty = JNIEnv.GetFieldID(jclass: intPtrtextViewClass, name: "mCursorDrawableRes", sig: "I");
                JNIEnv.SetField(jobject: Control.EditText.Handle, jfieldID: mCursorDrawableResProperty, val: Resource.Drawable.CustomCursor);
            }
            catch (Exception ex)
            {
                Log.Error(exception: ex, messageTemplate: "Error setting the cursor color for CustomEntry");
            }
        }
    }

}
