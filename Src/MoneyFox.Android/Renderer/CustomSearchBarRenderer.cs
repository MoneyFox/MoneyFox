using System;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using MoneyFox.Droid.Renderer;
using NLog;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace MoneyFox.Droid.Renderer
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);
            if (Control != null)
            {
                var searchView = Control;
                searchView.Iconified = false;
                searchView.SetIconifiedByDefault(false);

                var editText = Control.GetChildrenOfType<EditText>().First();

                SetCursorColor(editText);
                TrySetCursorPointerColor(editText);

                var fontColor = (Color)Application.Current.Resources["PrimaryFontColor"];
                UpdateSearchButtonColor(fontColor);
                UpdateCancelButtonColor(fontColor);
            }
        }
        private void SetCursorColor(EditText editText)
        {
            try
            {
                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(editText.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);

            }
            catch (Exception ex) {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }

        private void TrySetCursorPointerColor(EditText editText)
        {
            try
            {
                TextView textViewTemplate = new TextView(editText.Context);

                var field = textViewTemplate.Class.GetDeclaredField("mEditor");
                field.Accessible = true;
                var editor = field.Get(editText);

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
                    Int32 handle = field.GetInt(editText);

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


        void UpdateSearchButtonColor(Color fontColor)
        {
            int searchViewCloseButtonId = Control.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            if (searchViewCloseButtonId != 0)
            {
                var image = FindViewById<ImageView>(searchViewCloseButtonId);
                if (image != null && image.Drawable != null) {
                    image.Drawable.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcIn);
                }
            }
        }
        void UpdateCancelButtonColor(Color fontColor)
        {
            int searchViewCloseButtonId = Control.Resources.GetIdentifier("android:id/search_close_btn", null, null);
            if (searchViewCloseButtonId != 0)
            {
                var image = FindViewById<ImageView>(searchViewCloseButtonId);
                if (image != null && image.Drawable != null) {
                    image.Drawable.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcIn);
                }
            }
        }
    }
}