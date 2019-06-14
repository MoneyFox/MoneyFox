using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.ServiceLayer.ViewModels.DesignTime;
using MvvmCross.Commands;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts { get; } = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>
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
        public RelayCommand<AccountViewModel> OpenOverviewCommand { get; }
        public RelayCommand<AccountViewModel> EditAccountCommand { get; }
        public RelayCommand<AccountViewModel> DeleteAccountCommand { get; }
        public RelayCommand GoToAddAccountCommand { get; }
    }
}
