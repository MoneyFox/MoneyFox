using System.Collections.ObjectModel;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class TransactionListViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly BalanceViewModel balanceViewModel;
        private readonly IDialogService dialogService;
        private readonly ModifyTransactionViewModel modifyTransactionViewModel;
        private readonly ITransactionRepository transactionRepository;

        public TransactionListViewModel(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            BalanceViewModel balanceViewModel,
            ModifyTransactionViewModel modifyTransactionViewModel,
            IDialogService dialogService)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.balanceViewModel = balanceViewModel;
            this.modifyTransactionViewModel = modifyTransactionViewModel;
            this.dialogService = dialogService;
        }

        public MvxCommand<string> GoToAddTransactionCommand => new MvxCommand<string>(GoToAddTransaction);
        public MvxCommand DeleteAccountCommand => new MvxCommand(DeleteAccount);
        public MvxCommand LoadedCommand => new MvxCommand(LoadTransactions);
        public MvxCommand UnloadedCommand => new MvxCommand(UnloadTransactions);
        public MvxCommand EditCommand => new MvxCommand(Edit);

        public MvxCommand<FinancialTransaction> DeleteTransactionCommand
            => new MvxCommand<FinancialTransaction>(DeleteTransaction);

        /// <summary>
        ///     Returns all Transaction who are assigned to this repository
        /// </summary>
        public ObservableCollection<FinancialTransaction> RelatedTransactions { set; get; }

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
            //Refresh balance control with the current account
            balanceViewModel.UpdateBalance(true);

            SelectedTransaction = null;
            RelatedTransactions = new ObservableCollection<FinancialTransaction>(transactionRepository
                .GetRelatedTransactions(accountRepository.Selected)
                .OrderByDescending(x => x.Date)
                .ToList());
        }

        private void UnloadTransactions()
        {
            // Set balance control back to all accounts
            balanceViewModel.UpdateBalance();
        }

        private void GoToAddTransaction(string type)
        {
            ShowViewModel<ModifyTransactionViewModel>(new {isEdit = false, typeString = type});
        }

        private async void DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                accountRepository.Delete(accountRepository.Selected);
                accountRepository.RemoveTransactionAmount(SelectedTransaction);
                balanceViewModel.UpdateBalance(true);
                Close(this);
            }
        }

        private void Edit()
        {
            if (SelectedTransaction == null)
            {
                return;
            }

            transactionRepository.Selected = SelectedTransaction;

            ShowViewModel<ModifyTransactionViewModel>(
                new {isEdit = true});
            SelectedTransaction = null;
        }


        private async void DeleteTransaction(FinancialTransaction transaction)
        {
            if (
                await
                    dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteTransactionConfirmationMessage))
            {
                transactionRepository.Delete(transaction);
                RelatedTransactions.Remove(transaction);
                balanceViewModel.UpdateBalance();
            }
        }
    }
}