using System;
using CoreGraphics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("MoneyFox")]
[assembly: ExportEffect(typeof(PrimaryButtonEffect), nameof(PrimaryButtonEffect))]
namespace MoneyFox.iOS.Effects
{
    /// <summary>
    ///     Applies a custom button effect for buttons on IOS.
    /// </summary>
    public class PrimaryButtonEffect : PlatformEffect
    {
        protected override void OnAttached() {
            try
            {
                var button = (UIButton)Control;
                button.Layer.BorderWidth = 2;
                button.Layer.BorderColor = UIColor.DarkGray.CGColor;
                button.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);
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