using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.ServiceLayer.ViewModels.DesignTime;
using MoneyFox.ServiceLayer.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : MvxViewModel, IAccountListViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts => new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>
        {
            new AlphaGroupListGroupCollection<AccountViewModel>("Included")
            {
                new AccountViewModel{Name =  "Income", CurrentBalance = 1234}
            },
            new AlphaGroupListGroupCollection<AccountViewModel>("Excluded")
            {
                new AccountViewModel{Name =  "Savings", CurrentBalance = 4325}
            }
        };

        public bool HasNoAccounts { get; } = false;
        public IBalanceViewModel BalanceViewModel { get; } = new DesignTimeBalanceViewViewModel();
        public IAccountListViewActionViewModel ViewActionViewModel { get; }
        public MvxAsyncCommand<AccountViewModel> OpenOverviewCommand { get; }
        public MvxAsyncCommand<AccountViewModel> EditAccountCommand { get; }
        public MvxAsyncCommand<AccountViewModel> DeleteAccountCommand { get; }
        public MvxAsyncCommand GoToAddAccountCommand { get; }
    }
}
