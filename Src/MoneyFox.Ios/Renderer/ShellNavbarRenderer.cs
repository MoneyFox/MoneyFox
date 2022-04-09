using MoneyFox;
using MoneyFox.iOS.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(handler: typeof(AppShell), target: typeof(ShellNavbarRenderer))]

namespace MoneyFox.iOS.Renderer
{

    using UIKit;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    public class ShellNavbarRenderer : ShellRenderer
    {
        protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
        {
            return new NoLineAppearanceTracker();
        }
    }

    public class NoLineAppearanceTracker : IShellNavBarAppearanceTracker
    {
        public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
        {
            var tintColor = AppInfo.RequestedTheme == AppTheme.Dark ? Color.White : Color.FromHex("#323130");
            var navBar = controller.NavigationBar;
            navBar.TintColor = tintColor.ToUIColor();
            var navigationBarAppearance = new UINavigationBarAppearance();
            navigationBarAppearance.ConfigureWithOpaqueBackground();
            navigationBarAppearance.ShadowColor = UIColor.Clear;
            navigationBarAppearance.BackgroundColor = UIColor.Clear;
            navBar.ScrollEdgeAppearance = navBar.StandardAppearance = navigationBarAppearance;
        }

        public void SetHasShadow(UINavigationController controller, bool hasShadow)
        {
            // Only needed for interface implementation.
        }

        public void UpdateLayout(UINavigationController controller)
        {
            // Only needed for interface implementation.
        }

        public void ResetAppearance(UINavigationController controller)
        {
            // Only needed for interface implementation.
        }

        public void Dispose()
        {
            // Only needed for interface implementation.
        }
    }

}
