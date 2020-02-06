using System;
using MoneyFox.iOS.Renderer;
using NLog;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]
namespace MoneyFox.iOS.Renderer
{
    public class NavigationPageRenderer : NavigationRenderer
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            try
            {
                if (parent != null)
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    {
                        parent.ModalPresentationStyle = UIModalPresentationStyle.Automatic;
                    }
                }

                base.WillMoveToParentViewController(parent);
            }
            catch (Exception ex)
            {
                logManager.Warn(ex);
            }
        }
    }
}
