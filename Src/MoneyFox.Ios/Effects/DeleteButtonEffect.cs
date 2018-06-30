using System;
using CoreGraphics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(DeleteButtonEffect), "DeleteButtonEffect")]
namespace MoneyFox.iOS.Effects
{
    /// <summary>
    ///     Applies a custom button effect for delete buttons on IOS.
    /// </summary>
    public class DeleteButtonEffect : PlatformEffect
    {
        protected override void OnAttached() {
            try
            {
                var button = (UIButton)Control;
                button.Layer.BorderWidth = 2;
                button.Layer.BackgroundColor = UIColor.Red.CGColor;
                button.Layer.BorderColor = UIColor.White.CGColor;
                button.SetTitleColor(UIColor.White, UIControlState.Normal);
                button.SetTitleColor(UIColor.LightGray, UIControlState.Disabled);
                button.ClipsToBounds = true;
                button.Frame = new CGRect(button.Frame.X, button.Frame.Y, button.Frame.Width, 37);
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        protected override void OnDetached() {
        }
    }
}