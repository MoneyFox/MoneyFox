using System;
using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Helper;
using MoneyManager.Core.Logic;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyTransactionViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IDialogService dialogService;
        private readonly ITransactionRepository transactionRepository;

        public ModifyTransactionViewModel(ITransactionRepository transactionRepository,
            IRepository<Account> accountRepository,
            IDialogService dialogService)
        {
            this.transactionRepository = transactionRepository;
            this.dialogService = dialogService;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        ///     Handels everything when the page is loaded.
        /// </summary>
        public IMvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Saves the transaction or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxCommand SaveCommand => new MvxCommand(Save);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        public DateTime EndDate { get; set; }
        public bool IsEndless { get; set; } = true;
        public bool IsEdit { get; set; } = false;
        public int Recurrence { get; set; }
        public bool IsTransfer { get; set; }
        public bool RefreshRealtedList { get; set; }

        /// <summary>
        ///     The selected transaction
        /// </summary>
        public FinancialTransaction SelectedTransaction
        {
            get { return transactionRepository.Selected; }
            set { transactionRepository.Selected = value; }
        }

        /// <summary>
        ///     Gives access to all accounts
        /// </summary>
        public ObservableCollection<Account> AllAccounts => accountRepository.Data;

        /// <summary>
        ///     Returns the Title for the page
        /// </summary>
        public string Title
        {
            get
            {
                var text = IsEdit
                    ? Strings.EditTitle
                    : Strings.AddTitle;

                var type = TransactionTypeHelper.GetViewTitleForType(transactionRepository.Selected.Type);

                return string.Format(text, type);
            }
        }

        /// <summary>
        ///     The transaction date
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (!IsEdit && SelectedTransaction.Date == DateTime.MinValue)
                {
                    SelectedTransaction.Date = DateTime.Now;
                }
                return SelectedTransaction.Date;
            }
            set { SelectedTransaction.Date = value; }
        }

        private void Loaded()
        {
            if (IsEdit)
            {
                AccountLogic.RemoveTransactionAmount(SelectedTransaction);
            }
        }

        private async void Save()
        {
            if (transactionRepository.Selected.ChargedAccount == null)
            {
                ShowAccountRequiredMessage();
                return;
            }

            if (IsEdit)
            {
                await TransactionLogic.UpdateTransaction(transactionRepository.Selected);
            }
            else
            {
                TransactionLogic.SaveTransaction(transactionRepository.Selected, RefreshRealtedList);
            }
            Close(this);
        }

        private async void ShowAccountRequiredMessage()
        {
            await dialogService.ShowMessage(Strings.MandatoryFieldEmptryTitle,
                Strings.AccountRequiredMessage);
        }

        private void Cancel()
        {
            if (IsEdit)
            {
                AccountLogic.AddTransactionAmount(transactionRepository.Selected);
            }
            Close(this);
        }
    }
}