using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using UIKit;

namespace MoneyFox.Ios 
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate {
        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Window.BackgroundColor = UIColor.FromRGB(236, 239, 241);

            var setup = new Setup(this, Window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            Window.MakeKeyAndVisible();

            return true;
        }
    }
}
