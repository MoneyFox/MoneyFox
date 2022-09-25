using Microsoft.Maui.Controls.Compatibility;
using MoneyFox.Ui;
using MoneyFox.Ui.Platforms.iOS.Renderer;

[assembly: ExportRenderer(typeof(AppShell), typeof(CustomShellRenderer))]
namespace MoneyFox.Ui.Platforms.iOS.Renderer
{
    using CoreGraphics;
    using Microsoft.Maui.Controls.Handlers.Compatibility;
    using Microsoft.Maui.Controls.Platform.Compatibility;
    using UIKit;


    public class CustomShellRenderer : ShellRenderer
    {
        protected override IShellPageRendererTracker CreatePageRendererTracker()
        {
            return new CustomShellPageRendererTracker(this);
        }

        protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
        {
            return new NoLineAppearanceTracker();
        }
    }

    public class CustomShellPageRendererTracker : ShellPageRendererTracker
    {
        public CustomShellPageRendererTracker(IShellContext context)
            : base(context)
        {

        }

        protected override void UpdateTitleView()
        {
            if (ViewController == null || ViewController.NavigationItem == null)
                return;

            var titleView = Shell.GetTitleView(Page);

            if (titleView == null)
            {
                var view = ViewController.NavigationItem.TitleView;
                ViewController.NavigationItem.TitleView = null;
                view?.Dispose();
            }
            else
            {
                var view = new CustomTitleViewContainer(titleView);
                ViewController.NavigationItem.TitleView = view;
            }
        }
    }

    public class CustomTitleViewContainer : UIContainerView
    {
        public CustomTitleViewContainer(View view) : base(view)
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public override CGSize IntrinsicContentSize => UILayoutFittingExpandedSize;
    }

    public class NoLineAppearanceTracker : IShellNavBarAppearanceTracker
    {
        public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
        {
            var navBar = controller.NavigationBar;
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
