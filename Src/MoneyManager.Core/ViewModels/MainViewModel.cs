using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyManager.Core.Manager;

namespace MoneyManager.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly TransactionManager transactionManager;
        private readonly AccountManager accountManager;

        /// <summary>
        ///     Creates an MainViewModel object.
        /// </summary>
        /// <param name="transactionManager">Instance of <see cref="TransactionManager"/></param>
        /// <param name="navigationService">Instance of <see cref="INavigationService"/></param>
        /// <param name="accountManager">Instance of <see cref="AccountManager"/></param>
        public MainViewModel(TransactionManager transactionManager, INavigationService navigationService, AccountManager accountManager)
        {
            this.navigationService = navigationService;
            this.accountManager = accountManager;
            this.transactionManager = transactionManager;

            GoToAddTransactionCommand = new RelayCommand<string>(GoToAddTransaction);
            GoToAddAccountCommand = new RelayCommand(GoToAddAccount);
        }

        /// <summary>
        ///     Prepare everything and navigate to AddTransactionView
        /// </summary>
        public RelayCommand<string> GoToAddTransactionCommand { get; private set; }

        /// <summary>
        ///     Prepare everything and navigate to AddAccountView
        /// </summary>
        public RelayCommand GoToAddAccountCommand { get; private set; }

        private void GoToAddTransaction(string type)
        {
            transactionManager.PrepareCreation(type);
            navigationService.NavigateTo("AddTransactionView");
        }

        private void GoToAddAccount()
        {
            accountManager.PrepareCreation();
            navigationService.NavigateTo("AddAccountView");
        }
    }
}