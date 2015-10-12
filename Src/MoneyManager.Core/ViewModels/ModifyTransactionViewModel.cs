using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Helper;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyTransactionViewModel : BaseViewModel
    {
        /// <summary>
        ///     Locker to ensure that Init isn't executed when navigate back
        /// </summary>
        private static bool isInitCall = true;

        private readonly IRepository<Account> accountRepository;
        private readonly DefaultManager defaultManager;
        private readonly IDialogService dialogService;
        private readonly TransactionManager transactionManager;
        private readonly ITransactionRepository transactionRepository;

        private double oldAmount;

        public ModifyTransactionViewModel(ITransactionRepository transactionRepository,
            IRepository<Account> accountRepository,
            IDialogService dialogService,
            TransactionManager transactionManager,
            DefaultManager defaultManager)
        {
            this.transactionRepository = transactionRepository;
            this.dialogService = dialogService;
            this.transactionManager = transactionManager;
            this.defaultManager = defaultManager;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        ///     Init the view. Is executed after the constructor call
        /// </summary>
        /// <param name="typeString">Type of the transaction.</param>
        /// <param name="isEdit">Weather the transaction is in edit mode or not.</param>
        public void Init(bool isEdit, string typeString)
        {
            //Ensure that init is only called once
            if (!isInitCall) return;

            IsEdit = isEdit;
            IsEndless = true;

            if (IsEdit)
            {
                PrepareEdit();
            }
            else
            {
                PrepareDefault(typeString);
            }

            isInitCall = false;
        }

        private void PrepareEdit()
        {
            IsTransfer = SelectedTransaction.IsTransfer;
            Recurrence = SelectedTransaction.IsRecurring
                ? SelectedTransaction.RecurringTransaction.Recurrence
                : 0;
            oldAmount = SelectedTransaction.Amount;
            EndDate = SelectedTransaction.IsRecurring
                ? SelectedTransaction.RecurringTransaction.EndDate
                : DateTime.Now;
            IsEndless = !SelectedTransaction.IsRecurring || SelectedTransaction.RecurringTransaction.IsEndless;
        }

        private void PrepareDefault(string typeString)
        {
            var type = ((TransactionType) Enum.Parse(typeof (TransactionType), typeString));

            SetDefaultTransaction(type);
            SelectedTransaction.ChargedAccount = defaultManager.GetDefaultAccount();
            IsTransfer = type == TransactionType.Transfer;
            EndDate = DateTime.Now;
        }

        private void SetDefaultTransaction(TransactionType transactionType)
        {
            SelectedTransaction = new FinancialTransaction
            {
                Type = (int) transactionType,
                Date = DateTime.Now
            };
        }

        private void Loaded()
        {
            if (IsEdit)
            {
                //remove transaction on edit, on save or cancel the amount is added again.
                transactionManager.RemoveTransactionAmount(SelectedTransaction);
            }
        }

        private async void Save()
        {
            if (SelectedTransaction.ChargedAccount == null)
            {
                ShowAccountRequiredMessage();
                return;
            }

            //Create a recurring transaction based on the financial transaction or update an existing
            await SaveRecurringTransaction();

            // SaveItem or update the transaction and add the amount to the account
            transactionRepository.Save(SelectedTransaction);
            transactionManager.AddTransactionAmount(SelectedTransaction);

            ResetInitLocker();
            Close(this);
        }

        private async Task SaveRecurringTransaction()
        {
            if ((IsEdit && await transactionManager.CheckForRecurringTransaction(SelectedTransaction))
                || SelectedTransaction.IsRecurring)
            {
                SelectedTransaction.RecurringTransaction = RecurringTransactionHelper.
                    GetRecurringFromFinancialTransaction(SelectedTransaction,
                        IsEndless,
                        Recurrence,
                        EndDate);
            }
        }

        private void OpenSelectCategoryList()
        {
            ShowViewModel<CategoryListViewModel>();
        }

        private async void Delete()
        {
            if (
                await
                    dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteTransactionConfirmationMessage))
            {
                transactionManager.DeleteTransaction(transactionRepository.Selected);
                ResetInitLocker();
                Close(this);
            }
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
                //readd the previously removed transaction amount
                SelectedTransaction.Amount = oldAmount;
                transactionManager.AddTransactionAmount(SelectedTransaction);
            }
            ResetInitLocker();
            Close(this);
        }

        /// <summary>
        ///     Reset locker to enusre init gets called again
        /// </summary>
        private void ResetInitLocker()
        {
            isInitCall = true;
        }

        #region Commands

        /// <summary>
        ///     Handels everything when the page is loaded.
        /// </summary>
        public IMvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Saves the transaction or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxCommand SaveCommand => new MvxCommand(Save);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public IMvxCommand GoToSelectCategorydialogCommand => new MvxCommand(OpenSelectCategoryList);

        /// <summary>
        ///     Delets the transaction or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxCommand DeleteCommand => new MvxCommand(Delete);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        #endregion

        #region Properties

        /// <summary>
        ///     Indicates if the view is in Edit mode.
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        ///     Indicates if the transaction is a transfer.
        /// </summary>
        public bool IsTransfer { get; set; }

        /// <summary>
        ///     Indicates if the reminder is endless
        /// </summary>
        public bool IsEndless { get; set; }

        /// <summary>
        ///     The Enddate for recurring transaction
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     The selected recurrence
        /// </summary>
        public int Recurrence { get; set; }

        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        public string AmountString
        {
            get { return Utilities.FormatLargeNumbers(SelectedTransaction.Amount); }
            set { SelectedTransaction.Amount = Convert.ToDouble(value, CultureInfo.CurrentCulture); }
        }

        /// <summary>
        ///     List with the different recurrence types.
        /// </summary>
        public List<string> RecurrenceList => new List<string>
        {
            Strings.DailyLabel,
            Strings.DailyWithoutWeekendLabel,
            Strings.WeeklyLabel,
            Strings.MonthlyLabel,
            Strings.YearlyLabel
        };

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
        public string Title => TransactionTypeHelper.GetViewTitleForType(SelectedTransaction.Type, IsEdit);

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

        #endregion
    }
}