using MoneyFox.iOS.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]

namespace MoneyFox.iOS.Renderer
{
    using NLog;
    using System;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class NavigationPageRenderer : NavigationRenderer
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            try
            {
                if(parent == null)
                {
                    return;
                }

                parent.ModalPresentationStyle = UIModalPresentationStyle.Automatic;
                base.WillMoveToParentViewController(parent);
            }
            catch(Exception ex)
            {
                logManager.Warn(ex);
            }
        }
    }
}