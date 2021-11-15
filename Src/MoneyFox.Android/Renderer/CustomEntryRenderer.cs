﻿using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Java.Lang.Reflect;
using MoneyFox.Droid.Renderer;
using NLog;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Object = Java.Lang.Object;

#nullable enable
[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer), new[] {typeof(VisualMarker.MaterialVisual)})]

namespace MoneyFox.Droid.Renderer
{
    public class CustomEntryRenderer : MaterialEntryRenderer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            Control.EditText.SetHighlightColor(Color.Accent.ToAndroid());

            if(Build.VERSION.SdkInt >= BuildVersionCodes.Q)
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

                Drawable? textSelectHandleDrawable = Control.EditText.TextSelectHandle;
                if(textSelectHandleDrawable != null)
                {
                    textSelectHandleDrawable.SetColorFilter(
                        new BlendModeColorFilter(
                            Color.Accent.ToAndroid(),
                            BlendMode.SrcIn!));
                    Control.EditText.TextSelectHandle = textSelectHandleDrawable;
                }

                Drawable? textSelectHandleLeftDrawable = Control.EditText.TextSelectHandleLeft;
                if(textSelectHandleLeftDrawable != null)
                {
                    textSelectHandleLeftDrawable.SetColorFilter(
                        new BlendModeColorFilter(
                            Color.Accent.ToAndroid(),
                            BlendMode.SrcIn!));
                    Control.EditText.TextSelectHandle = textSelectHandleLeftDrawable;
                }

                Drawable? textSelectHandleRightDrawable = Control.EditText.TextSelectHandleRight;
                if(textSelectHandleRightDrawable != null)
                {
                    textSelectHandleRightDrawable.SetColorFilter(
                        new BlendModeColorFilter(
                            Color.Accent.ToAndroid(),
                            BlendMode.SrcIn!));
                    Control.EditText.TextSelectHandle = textSelectHandleRightDrawable;
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Error creating in the CustomEntry Renderer");
            }
        }

        private void TrySetCursorPointerColor()
        {
            try
            {
                var textViewTemplate = new TextView(Control.EditText.Context);

                Field field = textViewTemplate.Class.GetDeclaredField("mEditor");
                field.Accessible = true;

                Object? editor = field.Get(Control.EditText);
                if(editor == null)
                {
                    return;
                }

                string[] fieldsNames =
                {
                    "mTextSelectHandleLeftRes", "mTextSelectHandleRightRes", "mTextSelectHandleRes"
                };
                string[] drawableNames = {"mSelectHandleLeft", "mSelectHandleRight", "mSelectHandleCenter"};

                for(int index = 0; index < fieldsNames.Length && index < drawableNames.Length; index++)
                {
                    string fieldName = fieldsNames[index];
                    string drawableName = drawableNames[index];

                    field = textViewTemplate.Class.GetDeclaredField(fieldName);
                    field.Accessible = true;
                    int handle = field.GetInt(Control.EditText);

                    Drawable? handleDrawable = Resources?.GetDrawable(handle, null);

                    if(handleDrawable == null)
                    {
                        return;
                    }

                    if(Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        handleDrawable.SetColorFilter(
                            new BlendModeColorFilter(
                                Color.Accent.ToAndroid(),
                                BlendMode.SrcIn!));
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

                IntPtr intPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(intPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(Control.EditText.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Error setting the cursor color for CustomEntry.");
            }
        }
    }
}