using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using MoneyManager.Windows.Concrete;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class TransactionListViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IDialogService dialogService;
        private readonly ITransactionRepository transactionRepository;

        public TransactionListViewModel(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IBalanceViewModel balanceViewModel,
            IDialogService dialogService)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.balanceViewModel = balanceViewModel;
            this.dialogService = dialogService;
        }

        public MvxCommand<string> GoToAddTransactionCommand => new MvxCommand<string>(GoToAddTransaction);
        public MvxCommand DeleteAccountCommand => new MvxCommand(DeleteAccount);
        public virtual MvxCommand LoadedCommand => new MvxCommand(LoadTransactions);
        public MvxCommand EditCommand { get; private set; }

        public MvxCommand<FinancialTransaction> DeleteTransactionCommand
            => new MvxCommand<FinancialTransaction>(DeleteTransaction);

        /// <summary>
        ///     Returns all Transaction who are assigned to this repository
        ///     This has to stay until the android list with headers is implemented.
        /// </summary>
        public ObservableCollection<FinancialTransaction> RelatedTransactions { set; get; }

        /// <summary>
        ///     Returns groupped related transactions 
        /// </summary>
        public ObservableCollection<DateListGroup<FinancialTransaction>> Source { set; get; }

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

            Source = new ObservableCollection<DateListGroup<FinancialTransaction>>(
                DateListGroup<FinancialTransaction>.CreateGroups(RelatedTransactions,
                    CultureInfo.CurrentUICulture,
                    s => s.Date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + s.Date.Year,
                    s => s.Date, true));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new MvxCommand(Edit);
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
                balanceViewModel.UpdateBalance();
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
                new {isEdit = true, typeString = SelectedTransaction.Type.ToString()});
            SelectedTransaction = null;
        }


        private async void DeleteTransaction(FinancialTransaction transaction)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteTransactionConfirmationMessage))
                return;

            accountRepository.RemoveTransactionAmount(transaction);
            transactionRepository.Delete(transaction);
            RelatedTransactions.Remove(transaction);
            balanceViewModel.UpdateBalance(true);
        }
    }
}