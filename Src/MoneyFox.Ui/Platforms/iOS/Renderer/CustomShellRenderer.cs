namespace MoneyFox.Ui.Platforms.iOS.Renderer;

using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

public class CustomShellRenderer : ShellRenderer
{
    protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
    {
        return new NoLineAppearanceTracker();
    }
}

public class NoLineAppearanceTracker : IShellNavBarAppearanceTracker
{
    public void Dispose() { }

    public void ResetAppearance(UINavigationController controller) { }

    public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
    {
        var navBar = controller.NavigationBar;
        var navigationBarAppearance = new UINavigationBarAppearance();
        navigationBarAppearance.ConfigureWithOpaqueBackground();
        navigationBarAppearance.ShadowColor = UIColor.Clear;
        navigationBarAppearance.BackgroundColor = UIColor.Clear;
        navBar.ScrollEdgeAppearance = navBar.StandardAppearance = navigationBarAppearance;
    }

    public void SetHasShadow(UINavigationController controller, bool hasShadow) { }

    public void UpdateLayout(UINavigationController controller) { }
}
