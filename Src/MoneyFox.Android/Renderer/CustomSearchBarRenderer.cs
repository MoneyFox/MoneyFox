using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Droid.Renderer;
using NLog;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

#nullable enable
[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]

namespace MoneyFox.Droid.Renderer
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            try
            {
                if(Control != null)
                {
                    var searchView = Control;
                    searchView.Iconified = false;
                    searchView.SetIconifiedByDefault(false);

                    SetBackgroundColor(searchView);

                    var editText = Control.GetChildrenOfType<EditText>().First();

                    editText.SetHighlightColor(Color.Accent.ToAndroid());

                    if(Build.VERSION.SdkInt >= BuildVersionCodes.Q)
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
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void SetBackgroundColor(SearchView searchView)
        {
            if(Xamarin.Forms.Application.Current.UserAppTheme == OSAppTheme.Dark)
            {
                Xamarin.Forms.Application.Current.Resources.TryGetValue(
                    "BackgroundColorSearchBarDark",
                    out var darkTintColor);
                searchView.SetBackgroundColor(((Color)darkTintColor).ToAndroid());
            }
            else
            {
                Xamarin.Forms.Application.Current.Resources.TryGetValue(
                    "BackgroundColorSearchBarLight",
                    out var lightTintColor);
                searchView.SetBackgroundColor(((Color)lightTintColor).ToAndroid());
            }
        }

        private void TrySetCursorPointerColorNew(TextView textView)
        {
            try
            {
                textView.SetTextCursorDrawable(Resource.Drawable.CustomCursor);

                var textSelectHandleDrawable = textView.TextSelectHandle;

                textSelectHandleDrawable?.SetColorFilter(
                    new BlendModeColorFilter(Color.Accent.ToAndroid(), BlendMode.SrcIn!));
                textView.TextSelectHandle = textSelectHandleDrawable;

                var textSelectHandleLeftDrawable = textView.TextSelectHandleLeft;
                textSelectHandleLeftDrawable?.SetColorFilter(
                    new BlendModeColorFilter(Color.Accent.ToAndroid(), BlendMode.SrcIn!));
                textView.TextSelectHandle = textSelectHandleLeftDrawable;

                var textSelectHandleRightDrawable = textView.TextSelectHandleRight;
                textSelectHandleRightDrawable?.SetColorFilter(
                    new BlendModeColorFilter(Color.Accent.ToAndroid(), BlendMode.SrcIn!));
                textView.TextSelectHandle = textSelectHandleRightDrawable;
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Issue in rendering custom search bar.");
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

                if(editor == null)
                {
                    return;
                }

                string[] fieldsNames =
                {
                    "mTextSelectHandleLeftRes", "mTextSelectHandleRightRes", "mTextSelectHandleRes"
                };
                string[] drawableNames = {"mSelectHandleLeft", "mSelectHandleRight", "mSelectHandleCenter"};

                for(var index = 0; index < fieldsNames.Length && index < drawableNames.Length; index++)
                {
                    var fieldName = fieldsNames[index];
                    var drawableName = drawableNames[index];

                    field = textViewTemplate.Class.GetDeclaredField(fieldName);
                    field.Accessible = true;
                    var handle = field.GetInt(textView);

                    var handleDrawable = Resources?.GetDrawable(handle, null);

                    if(handleDrawable == null)
                    {
                        return;
                    }

                    if(Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        handleDrawable.SetColorFilter(
                            new BlendModeColorFilter(Color.Accent.ToAndroid(), BlendMode.SrcIn!));
                    }
                    else
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        handleDrawable.SetColorFilter(Color.Accent.ToAndroid(), PorterDuff.Mode.SrcIn!);
#pragma warning restore CS0618 // Type or member is obsolete
                    }

                    field = editor.Class.GetDeclaredField(drawableName);
                    field.Accessible = true;
                    field.Set(editor, handleDrawable);
                }

                var intPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                var mCursorDrawableResProperty = JNIEnv.GetFieldID(intPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(textView.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Issue on setting the cursor color for custom search bar.");
            }
        }

        private void UpdateSearchButtonColor()
        {
            var searchViewCloseButtonId = Control.Resources?.GetIdentifier("android:id/search_mag_icon", null, null);
            if(searchViewCloseButtonId.HasValue && searchViewCloseButtonId != 0)
            {
                SetColorGray(FindViewById<ImageView>(searchViewCloseButtonId.Value));
            }
        }

        private void UpdateCancelButtonColor()
        {
            var searchViewCloseButtonId = Control.Resources?.GetIdentifier("android:id/search_close_btn", null, null);
            if(searchViewCloseButtonId.HasValue && searchViewCloseButtonId != 0)
            {
                SetColorGray(FindViewById<ImageView>(searchViewCloseButtonId.Value));
            }
        }

        private static void SetColorGray(ImageView? image)
        {
            if(Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                image?.Drawable?.SetColorFilter(
                    new BlendModeColorFilter(Android.Graphics.Color.Gray, BlendMode.SrcIn!));
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                image?.Drawable?.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcIn!);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
}