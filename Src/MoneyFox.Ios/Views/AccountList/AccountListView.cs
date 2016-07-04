using MoneyFox.Shared.ViewModels;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Views;

namespace MoneyFox.Ios.Views.AccountList {
    [MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true)]
    public partial class AccountListView : MvxViewController<AccountListViewModel> {
        public AccountListView() : base("AccountListView", null)
        {
        }
    }
}