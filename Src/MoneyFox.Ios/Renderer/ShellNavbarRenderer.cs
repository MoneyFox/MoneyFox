using MoneyFox;
using MoneyFox.iOS.Renderer;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AppShell), typeof(ShellNavbarRenderer))]

namespace MoneyFox.iOS.Renderer
{
    public class ShellNavbarRenderer : ShellRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) =>
            base.OnElementPropertyChanged(sender, e);

        protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker() =>
            new NoLineAppearanceTracker();
    }

    public class NoLineAppearanceTracker : IShellNavBarAppearanceTracker
    {
        public void Dispose()
        {
        }

        public void ResetAppearance(UINavigationController controller)
        {
        }

        public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
        {
            Color tintColor = AppInfo.RequestedTheme == AppTheme.Dark
                ? Color.White
                : Color.FromHex("#323130");

            UINavigationBar? navBar = controller.NavigationBar;
            navBar.TintColor = tintColor.ToUIColor();

            var navigationBarAppearance = new UINavigationBarAppearance();
            navigationBarAppearance.ConfigureWithOpaqueBackground();

            navigationBarAppearance.ShadowColor = UIColor.Clear;
            navigationBarAppearance.BackgroundColor = UIColor.Clear;
            navBar.ScrollEdgeAppearance = navBar.StandardAppearance = navigationBarAppearance;
        }

        public void SetHasShadow(UINavigationController controller, bool hasShadow)
        {
        }

        public void UpdateLayout(UINavigationController controller)
        {
        }
    }
}