using Foundation;
using MoneyFox.Shared.ViewModels;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Views;

namespace MoneyFox.Ios.Views
{
    [Register("MainView")]
	[MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true)]
    public class MainView : MvxViewController<MainViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.ShowMenuAndFirstDetail();
        }
    }
}
