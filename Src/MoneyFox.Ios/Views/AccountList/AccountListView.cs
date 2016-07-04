using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Views;

namespace MoneyFox.Ios.Views.AccountList {
    [MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true)]
    public partial class AccountListView : MvxViewController<AccountListViewModel> {

        public override void ViewDidLoad() {
            base.ViewDidLoad();

            var source = new MvxSimpleTableViewSource(AccountList, AccountViewCell.Key);
            this.CreateBinding(source).To<AccountListViewModel>(vm => vm.AllAccounts).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To<AccountListViewModel>(vm => vm.OpenOverviewCommand).Apply();
            AccountList.RowHeight = 55;
            AccountList.Source = source;
            AccountList.ReloadData();
        }

        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);

            Title = Strings.AccountsLabel;
        }
    }
}