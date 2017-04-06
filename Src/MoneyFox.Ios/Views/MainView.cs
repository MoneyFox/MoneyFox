using Foundation;
using MoneyFox.Business.ViewModels;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Views;

namespace MoneyFox.Ios.Views
{
    [Register("MainView")]
	[MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, false)]
    public class MainView : MvxViewController<MainViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.ShowMenuAndFirstDetail();
        }
    }
}
