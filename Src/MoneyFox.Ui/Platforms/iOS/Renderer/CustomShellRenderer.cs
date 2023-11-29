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
    public void ResetAppearance(UINavigationController controller)
    {
        // Not needed
    }

    public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
    {
        var navigationBarAppearance = new UINavigationBarAppearance();
        navigationBarAppearance.ShadowColor = UIColor.Clear;
    }

    public void SetHasShadow(UINavigationController controller, bool hasShadow)
    {
        // Not needed
    }

    public void UpdateLayout(UINavigationController controller)
    {
        // Not needed
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Nothing to dispose
    }
}
