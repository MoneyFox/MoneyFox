using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyManager.Business.Manager;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class TransactionListViewModel : ViewModelBase
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IRepository<Account> accountRepository;
        private readonly TransactionManager transactionManager;
        private readonly INavigationService navigationService;

        public TransactionListViewModel(ITransactionRepository transactionRepository, IRepository<Account> accountRepository, TransactionManager transactionManager, INavigationService navigationService)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.transactionManager = transactionManager;
            this.navigationService = navigationService;

            GoToAddTransactionCommand = new RelayCommand<string>(GoToAddTransaction);
        }

        public RelayCommand<string> GoToAddTransactionCommand { get; private set; }
        
        /// <summary>
        ///     Returns all Transaction who are assigned to this repository
        /// </summary>
        public List<FinancialTransaction> RelatedTransactions { set; get; }
        
        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title => accountRepository.Selected.Name;

        public void SetRelatedTransactions(Account account)
        {
            RelatedTransactions = transactionRepository
                .GetRelatedTransactions(account)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        private void GoToAddTransaction(string type)
        {
            transactionManager.PrepareCreation(type);
            navigationService.NavigateTo("AddTransactionView");
        }
    }
}