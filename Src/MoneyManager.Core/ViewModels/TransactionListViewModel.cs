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
        private readonly BalanceViewModel balanceViewModel;
        private readonly ModifyTransactionViewModel modifyTransactionViewModel;

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
        public MvxCommand LoadedCommand => new MvxCommand(LoadTransactions);
        public MvxCommand UnloadedCommand => new MvxCommand(UnloadTransactions);
        public MvxCommand<FinancialTransaction> EditCommand => new MvxCommand<FinancialTransaction>(Edit);
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
        ///     Currentli selected Item
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
            ShowViewModel<ModifyTransactionViewModel>();
        }

        private void Edit(FinancialTransaction transaction)
        {
            if (transaction == null) return;

            transactionManager.PrepareEdit(transaction);
            SelectedTransaction = null;
            ShowViewModel<ModifyTransactionViewModel>();
        }

        private async void Delete(FinancialTransaction transaction)
        {
            await transactionManager.DeleteTransaction(transaction);
            balanceViewModel.UpdateBalance();
        }
    }
}