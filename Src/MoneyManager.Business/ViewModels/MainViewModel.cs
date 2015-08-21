using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyManager.Business.Manager;

namespace MoneyManager.Business.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private readonly TransactionManager transactionManager;
        private readonly INavigationService navigationService;

        public RelayCommand<string> GoToAddTransactionCommand { get; private set; }
        public RelayCommand GoToAddAccountCommand { get; private set; }
        
        public MainViewModel(TransactionManager transactionManager, INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.transactionManager = transactionManager;

            GoToAddTransactionCommand = new RelayCommand<string>(GoToAddTransaction);
            GoToAddAccountCommand = new RelayCommand(() => navigationService.NavigateTo("AddAccountView"));
        }

        private void GoToAddTransaction(string type)
        {
            transactionManager.PrepareCreation(type);
            navigationService.NavigateTo("AddTransactionView");
        }
    }
}
