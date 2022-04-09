using MoneyFox.Droid.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(handler: typeof(SearchBar), target: typeof(CustomSearchBarRenderer))]

namespace MoneyFox.Droid.Renderer
{

    using System;
    using System.Linq;
    using Android.Content;
    using Android.Graphics;
    using Android.OS;
    using Android.Runtime;
    using Android.Widget;
    using Microsoft.AppCenter.Crashes;
    using Serilog;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Color = Xamarin.Forms.Color;
    using Resource = Droid.Resource;

    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    var searchView = Control;
                    searchView.Iconified = false;
                    searchView.SetIconifiedByDefault(false);
                    SetBackgroundColor(searchView);
                    var editText = Control.GetChildrenOfType<EditText>().First();
                    editText.SetHighlightColor(Color.Accent.ToAndroid());
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        TrySetCursorPointerColorNew(editText);
                    }
                    else
                    {
                        TrySetCursorPointerColor(editText);
                    }

                    UpdateSearchButtonColor();
                    UpdateCancelButtonColor();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void SetBackgroundColor(SearchView searchView)
        {
            if (Application.Current.UserAppTheme == OSAppTheme.Dark)
            {
                Application.Current.Resources.TryGetValue(key: "BackgroundColorSearchBarDark", value: out var darkTintColor);
                searchView.SetBackgroundColor(((Color)darkTintColor).ToAndroid());
            }
            else
            {
                Application.Current.Resources.TryGetValue(key: "BackgroundColorSearchBarLight", value: out var lightTintColor);
                searchView.SetBackgroundColor(((Color)lightTintColor).ToAndroid());
            }
        }

        private void TrySetCursorPointerColorNew(TextView textView)
        {
            try
            {
                textView.SetTextCursorDrawable(Resource.Drawable.CustomCursor);
                var textSelectHandleDrawable = textView.TextSelectHandle;
                textSelectHandleDrawable?.SetColorFilter(new BlendModeColorFilter(color: Color.Accent.ToAndroid(), mode: BlendMode.SrcIn!));
                textView.TextSelectHandle = textSelectHandleDrawable;
                var textSelectHandleLeftDrawable = textView.TextSelectHandleLeft;
                textSelectHandleLeftDrawable?.SetColorFilter(new BlendModeColorFilter(color: Color.Accent.ToAndroid(), mode: BlendMode.SrcIn!));
                textView.TextSelectHandle = textSelectHandleLeftDrawable;
                var textSelectHandleRightDrawable = textView.TextSelectHandleRight;
                textSelectHandleRightDrawable?.SetColorFilter(new BlendModeColorFilter(color: Color.Accent.ToAndroid(), mode: BlendMode.SrcIn!));
                textView.TextSelectHandle = textSelectHandleRightDrawable;
            }
            catch (Exception ex)
            {
                Log.Error(exception: ex, messageTemplate: "Issue in rendering custom search bar");
            }
        }

        private void TrySetCursorPointerColor(TextView textView)
        {
            try
            {
                var textViewTemplate = new TextView(textView.Context);
                var field = textViewTemplate.Class.GetDeclaredField("mEditor");
                field.Accessible = true;
                var editor = field.Get(textView);
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
                    var handle = field.GetInt(textView);
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
                JNIEnv.SetField(jobject: textView.Handle, jfieldID: mCursorDrawableResProperty, val: Resource.Drawable.CustomCursor);
            }
            catch (Exception ex)
            {
                Log.Error(exception: ex, messageTemplate: "Issue on setting the cursor color for custom search bar");
            }
        }

        private void UpdateSearchButtonColor()
        {
            var searchViewCloseButtonId = Control.Resources?.GetIdentifier(name: "android:id/search_mag_icon", defType: null, defPackage: null);
            if (searchViewCloseButtonId.HasValue && searchViewCloseButtonId != 0)
            {
                SetColorGray(FindViewById<ImageView>(searchViewCloseButtonId.Value));
            }
        }

        private void UpdateCancelButtonColor()
        {
            var searchViewCloseButtonId = Control.Resources?.GetIdentifier(name: "android:id/search_close_btn", defType: null, defPackage: null);
            if (searchViewCloseButtonId.HasValue && searchViewCloseButtonId != 0)
            {
                SetColorGray(FindViewById<ImageView>(searchViewCloseButtonId.Value));
            }
        }

        private static void SetColorGray(ImageView? image)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                image?.Drawable?.SetColorFilter(new BlendModeColorFilter(color: Android.Graphics.Color.Gray, mode: BlendMode.SrcIn!));
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                image?.Drawable?.SetColorFilter(color: Android.Graphics.Color.Gray, mode: PorterDuff.Mode.SrcIn!);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }

}
