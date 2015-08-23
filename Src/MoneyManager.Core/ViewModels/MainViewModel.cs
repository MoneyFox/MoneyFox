using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyManager.Core.Manager;

namespace MoneyManager.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly TransactionManager transactionManager;

        public MainViewModel(TransactionManager transactionManager, INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.transactionManager = transactionManager;

            GoToAddTransactionCommand = new RelayCommand<string>(GoToAddTransaction);
            GoToAddAccountCommand = new RelayCommand(() => navigationService.NavigateTo("AddAccountView"));
        }

        public RelayCommand<string> GoToAddTransactionCommand { get; private set; }
        public RelayCommand GoToAddAccountCommand { get; private set; }

        private void GoToAddTransaction(string type)
        {
            transactionManager.PrepareCreation(type);
            navigationService.NavigateTo("AddTransactionView");
        }
    }
}