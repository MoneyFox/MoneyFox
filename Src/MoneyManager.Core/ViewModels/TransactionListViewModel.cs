using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class TransactionListViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly TransactionManager transactionManager;
        private readonly ITransactionRepository transactionRepository;

        public TransactionListViewModel(ITransactionRepository transactionRepository,
            IRepository<Account> accountRepository, 
            TransactionManager transactionManager)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.transactionManager = transactionManager;

            GoToAddTransactionCommand = new MvxCommand<string>(GoToAddTransaction);
        }

        public MvxCommand<string> GoToAddTransactionCommand { get; private set; }

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
            ShowViewModel<AddTransactionViewModel>();
        }
    }
}