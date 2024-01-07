// ReSharper disable once CheckNamespace
namespace MoneyFox.Ui;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Microsoft.Identity.Client;
using MoneyFox.Infrastructure.DbBackup.Legacy;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize
                           | ConfigChanges.Orientation
                           | ConfigChanges.UiMode
                           | ConfigChanges.ScreenLayout
                           | ConfigChanges.SmallestScreenSize
                           | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        ParentActivityWrapper.ParentActivity = this;
        base.OnCreate(savedInstanceState);
        Platform.Init(activity: this, bundle: savedInstanceState);
        SetStatusBarColor();
    }

    private void SetStatusBarColor()
    {
        Color backgroundColor;
        if (Microsoft.Maui.Controls.Application.Current?.RequestedTheme == AppTheme.Light)
        {
            backgroundColor = new(r: 239, g: 242, b: 245);
            if (OperatingSystem.IsAndroidVersionAtLeast(30))
            {
                Window?.InsetsController?.SetSystemBarsAppearance(
                    appearance: (int)WindowInsetsControllerAppearance.LightStatusBars,
                    mask: (int)WindowInsetsControllerAppearance.LightStatusBars);
            }

            if (!OperatingSystem.IsAndroidVersionAtLeast(30) && OperatingSystem.IsAndroidVersionAtLeast(23) && Window is not null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
        else
        {
            backgroundColor = new(r: 18, g: 18, b: 18);
        }

        Window?.SetStatusBarColor(backgroundColor);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode: requestCode, resultCode: resultCode, data: data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode: requestCode, resultCode: resultCode, data: data);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
        base.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
    }
}
