using MoneyFox.iOS.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(handler: typeof(NavigationPage), target: typeof(NavigationPageRenderer))]

namespace MoneyFox.iOS.Renderer
{

    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class NavigationPageRenderer : NavigationRenderer
    {
        public override void WillMoveToParentViewController(UIViewController parent)
        {
            if (parent == null)
            {
                return;
            }

            parent.ModalPresentationStyle = UIModalPresentationStyle.Automatic;
            base.WillMoveToParentViewController(parent);
        }
    }

}
