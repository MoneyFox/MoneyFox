using MoneyFox;
using MoneyFox.iOS.Renderer;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AppShell), typeof(ShellNavbarRenderer))]
namespace MoneyFox.iOS.Renderer
{
    public class ShellNavbarRenderer : ShellRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
        {
            return new NoLineAppearanceTracker();
        }
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
            var navBar = controller.NavigationBar;
            var navigationBarAppearance = new UINavigationBarAppearance();
            navigationBarAppearance.ConfigureWithOpaqueBackground();

            navigationBarAppearance.ShadowColor = UIColor.Clear;
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