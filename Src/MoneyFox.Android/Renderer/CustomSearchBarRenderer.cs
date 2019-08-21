using System;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using Java.Lang.Reflect;
using MoneyFox.Droid.Renderer;
using NLog;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Object = Java.Lang.Object;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]

namespace MoneyFox.Droid.Renderer
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                SearchView searchView = Control;
                searchView.Iconified = false;
                searchView.SetIconifiedByDefault(false);

                EditText editText = Control.GetChildrenOfType<EditText>().First();

                SetCursorColor(editText);
                TrySetCursorPointerColor(editText);

                UpdateSearchButtonColor();
                UpdateCancelButtonColor();
            }
        }

        private static void SetCursorColor(EditText editText)
        {
            try
            {
                IntPtr intPtrTextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(intPtrTextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(editText.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }

        private void TrySetCursorPointerColor(EditText editText)
        {
            try
            {
                var textViewTemplate = new TextView(editText.Context);

                Field field = textViewTemplate.Class.GetDeclaredField("mEditor");
                field.Accessible = true;
                Object editor = field.Get(editText);

                string[] fieldsNames = {"mTextSelectHandleLeftRes", "mTextSelectHandleRightRes", "mTextSelectHandleRes"};
                string[] drawablesNames = {"mSelectHandleLeft", "mSelectHandleRight", "mSelectHandleCenter"};

                for (var index = 0; index < fieldsNames.Length && index < drawablesNames.Length; index++)
                {
                    string fieldName = fieldsNames[index];
                    string drawableName = drawablesNames[index];

                    field = textViewTemplate.Class.GetDeclaredField(fieldName);
                    field.Accessible = true;
                    int handle = field.GetInt(editText);

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


        private void UpdateSearchButtonColor()
        {
            int searchViewCloseButtonId = Control.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            if (searchViewCloseButtonId != 0)
            {
                var image = FindViewById<ImageView>(searchViewCloseButtonId);
                if (image != null && image.Drawable != null) image.Drawable.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcIn);
            }
        }

        private void UpdateCancelButtonColor()
        {
            int searchViewCloseButtonId = Control.Resources.GetIdentifier("android:id/search_close_btn", null, null);
            if (searchViewCloseButtonId != 0)
            {
                var image = FindViewById<ImageView>(searchViewCloseButtonId);
                if (image != null && image.Drawable != null) image.Drawable.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcIn);
            }
        }
    }
}
