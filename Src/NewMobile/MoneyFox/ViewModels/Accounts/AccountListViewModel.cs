using GalaSoft.MvvmLight.Command;
using MoneyFox.Extensions;
using MoneyFox.Groups;
using MoneyFox.Views.Accounts;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Accounts
{
    public class AccountListViewModel : BaseViewModel
    {
        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts { get; set; } = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>
        {
            new AlphaGroupListGroupCollection<AccountViewModel>("Included")
            {
                new AccountViewModel{ Name = "Income", CurrentBalance = 78542, IsExcluded = false, EndOfMonthBalance = 1234 },
                new AccountViewModel{ Name = "Expenses", CurrentBalance = 2451, IsExcluded = false, EndOfMonthBalance = 7854 },
            },
            new AlphaGroupListGroupCollection<AccountViewModel>("Included")
            {
                new AccountViewModel{ Name = "Investments", CurrentBalance = 1570, IsExcluded = true, EndOfMonthBalance = 2142 },
                new AccountViewModel{ Name = "Safety", CurrentBalance = 4455, IsExcluded = true, EndOfMonthBalance = 5522 }
            }
        };

        public RelayCommand GoToAddAccountCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddAccountRoute));

        public RelayCommand<AccountViewModel> GoToEditAccountCommand
            => new RelayCommand<AccountViewModel>(async (accountViewModel)
                => await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditAccountPage(accountViewModel.Id)) { BarBackgroundColor = Color.Transparent }));

        public RelayCommand<AccountViewModel> GoToTransactionListCommand
            => new RelayCommand<AccountViewModel>(async (accountViewModel)
                => await Shell.Current.GoToAsync($"{ViewModelLocator.PaymentListRoute}?accountId={accountViewModel.Id}"));
    }
}
