using MoneyFox;
using MoneyFox.iOS.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(AppShell), typeof(CustomShellRenderer))]
namespace MoneyFox.iOS.Renderer
{
    using CoreGraphics;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    ///     This file is a work around for an issue with the titleview in iOS 16 and can be removed once it is fixed
    ///     in Xamarin Forms and / or .net MAUI
    /// </summary>
    public class CustomShellRenderer : ShellRenderer
    {
        protected override IShellPageRendererTracker CreatePageRendererTracker()
        {
            return new CustomShellPageRendererTracker(this);
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

        public override CGSize SizeThatFits(CGSize size)
        {
            return size;
        }
    }
}
