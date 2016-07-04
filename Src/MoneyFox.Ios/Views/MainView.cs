using MoneyFox.Shared.ViewModels;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Views;

namespace MoneyFox.Ios.Views {
    [MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true)]

    public partial class MainView : BaseViewController<MainViewModel> {

        public override void ViewDidLoad() {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}