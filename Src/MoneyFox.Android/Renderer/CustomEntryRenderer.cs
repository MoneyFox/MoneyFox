using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using MoneyFox.Droid.Renderer;
using NLog;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace MoneyFox.Droid.Renderer
{
    public class CustomEntryRenderer : MaterialEntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context) {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e) {
            base.OnElementChanged(e);

            // set cursor color
            IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
            IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

            JNIEnv.SetField(Control.EditText.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);

            // try set cursor pointer color
            try
            {
                TextView textViewTemplate = new TextView(Control.EditText.Context);

                var field = textViewTemplate.Class.GetDeclaredField("mEditor");
                field.Accessible = true;
                var editor = field.Get(Control.EditText);
                
                String[]
                    fieldsNames = { "mTextSelectHandleLeftRes", "mTextSelectHandleRightRes", "mTextSelectHandleRes" },
                    drawablesNames = { "mSelectHandleLeft", "mSelectHandleRight", "mSelectHandleCenter" };

                for (Int32 index = 0; index < fieldsNames.Length && index < drawablesNames.Length; index++)
                {
                    String
                        fieldName = fieldsNames[index],
                        drawableName = drawablesNames[index];

                    field = textViewTemplate.Class.GetDeclaredField(fieldName);
                    field.Accessible = true;
                    Int32 handle = field.GetInt(Control.EditText);

                    Drawable handleDrawable = Resources.GetDrawable(handle, null);

                    handleDrawable.SetColorFilter(Color.Accent.ToAndroid(), PorterDuff.Mode.SrcIn);

                    field = editor.Class.GetDeclaredField(drawableName);
                    field.Accessible = true;
                    field.Set(editor, handleDrawable);
                }
            } 
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }
    }
}