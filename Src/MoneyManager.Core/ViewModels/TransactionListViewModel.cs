using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Helper;
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
        private readonly BalanceViewModel balanceViewModel;
        private readonly ModifyTransactionViewModel modifyTransactionViewModel;
        private readonly TransactionManager transactionManager;
        private readonly ITransactionRepository transactionRepository;

        public TransactionListViewModel(ITransactionRepository transactionRepository,
            IRepository<Account> accountRepository,
            TransactionManager transactionManager,
            BalanceViewModel balanceViewModel,
            ModifyTransactionViewModel modifyTransactionViewModel)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.transactionManager = transactionManager;
            this.balanceViewModel = balanceViewModel;
            this.modifyTransactionViewModel = modifyTransactionViewModel;
        }

        public MvxCommand<string> GoToAddTransactionCommand => new MvxCommand<string>(GoToAddTransaction);
        public MvxCommand DeleteAccountCommand => new MvxCommand(DeleteAccount);
        public MvxCommand LoadedCommand => new MvxCommand(LoadTransactions);
        public MvxCommand UnloadedCommand => new MvxCommand(UnloadTransactions);
        public MvxCommand EditCommand => new MvxCommand(Edit);
        public MvxCommand<FinancialTransaction> DeleteCommand => new MvxCommand<FinancialTransaction>(Delete);

        /// <summary>
        ///     Returns all Transaction who are assigned to this repository
        /// </summary>
        public List<FinancialTransaction> RelatedTransactions { set; get; }

        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title => accountRepository.Selected.Name;

        /// <summary>
        ///     Currently selected Item
        /// </summary>
        public FinancialTransaction SelectedTransaction { get; set; }

        private void LoadTransactions()
        {
            balanceViewModel.IsTransactionView = true;
            SelectedTransaction = null;
            RelatedTransactions = transactionRepository
                .GetRelatedTransactions(accountRepository.Selected)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        private void UnloadTransactions()
        {
            balanceViewModel.IsTransactionView = false;
            balanceViewModel.UpdateBalance();
        }

        private void GoToAddTransaction(string type)
        {
            modifyTransactionViewModel.IsEdit = false;
            ShowViewModel<ModifyTransactionViewModel>(new { typeString = type });
        }

        private void DeleteAccount()
        {
            accountRepository.Delete(accountRepository.Selected);
            Close(this);
        }

        private void Edit()
        {
            if (SelectedTransaction == null)
            {
                return;
            }

            transactionRepository.Selected = SelectedTransaction;

            ShowViewModel<ModifyTransactionViewModel>(new { typeString = TransactionTypeHelper.GetTypeString(SelectedTransaction.Type), isEdit = true });
            SelectedTransaction = null;
        }


        private void Delete(FinancialTransaction transaction)
        {
            transactionManager.DeleteTransaction(transaction);
            balanceViewModel.UpdateBalance();
        }
    }
}