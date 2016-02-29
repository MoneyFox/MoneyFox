using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Messages;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyPaymentViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDefaultManager defaultManager;
        private readonly IDialogService dialogService;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;

        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        // This has to be static in order to keep the value even if you leave the page to select a category.
        private double amount;

        public ModifyPaymentViewModel(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IDialogService dialogService,
            IPaymentManager paymentManager,
            IDefaultManager defaultManager)
        {
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;
            this.paymentManager = paymentManager;
            this.defaultManager = defaultManager;
            this.accountRepository = accountRepository;

            token =
                MessageHub.Subscribe<CategorySelectedMessage>(
                    message => SelectedPayment.Category = message.SelectedCategory);
        }

        /// <summary>
        ///     Saves the payment or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public RelayCommand SaveCommand => new RelayCommand(Save);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public RelayCommand GoToSelectCategorydialogCommand => new RelayCommand(OpenSelectCategoryList);

        /// <summary>
        ///     Delets the payment or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public RelayCommand DeleteCommand => new RelayCommand(Delete);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        /// <summary>
        ///     Resets the category of the currently selected payment
        /// </summary>
        public RelayCommand ResetCategoryCommand => new RelayCommand(ResetSelection);


        /// <summary>
        ///     Indicates if the view is in Edit mode.
        /// </summary>
        public bool IsEdit { get; private set; }

        /// <summary>
        ///     Indicates if the payment is a transfer.
        /// </summary>
        public bool IsTransfer { get; private set; }

        /// <summary>
        ///     Indicates if the reminder is endless
        /// </summary>
        public bool IsEndless { get; set; }

        /// <summary>
        ///     The Enddate for recurring payment
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
            get { return Utilities.FormatLargeNumbers(amount); }
            set
            {
                double convertedValue;
                if (double.TryParse(value, out convertedValue))
                {
                    amount = convertedValue;
                }
            }
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
            Strings.YearlyLabel,
            Strings.BiweeklyLabel
        };

        /// <summary>
        ///     The selected payment
        /// </summary>
        public Payment SelectedPayment
        {
            get { return paymentRepository.Selected; }
            set { paymentRepository.Selected = value; }
        }

        /// <summary>
        ///     Gives access to all accounts
        /// </summary>
        public ObservableCollection<Account> AllAccounts => accountRepository.Data;

        /// <summary>
        ///     Returns the Title for the page
        /// </summary>
        public string Title => PaymentTypeHelper.GetViewTitleForType(SelectedPayment.Type, IsEdit);

        /// <summary>
        ///     Returns the Header for the account field
        /// </summary>
        public string AccountHeader
            => SelectedPayment?.Type == (int) PaymentType.Income
                ? Strings.TargetAccountLabel
                : Strings.ChargedAccountLabel;

        /// <summary>
        ///     The payment date
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (!IsEdit && SelectedPayment.Date == DateTime.MinValue)
                {
                    SelectedPayment.Date = DateTime.Now;
                }
                return SelectedPayment.Date;
            }
            set { SelectedPayment.Date = value; }
        }

        private Account AccountBeforeEdit { get; set; }

        /// <summary>
        ///     Init the view. Is executed after the constructor call
        /// </summary>
        /// <param name="typeString">Type of the payment.</param>
        /// <param name="isEdit">Weather the payment is in edit mode or not.</param>
        public void Init(string typeString, bool isEdit = false)
        {
            IsEdit = isEdit;
            IsEndless = true;

            amount = 0;

            if (IsEdit)
            {
                PrepareEdit();
            }
            else
            {
                PrepareDefault(typeString);
            }

            AccountBeforeEdit = SelectedPayment.ChargedAccount;
        }

        private void PrepareEdit()
        {
            IsTransfer = SelectedPayment.IsTransfer;
            // set the private amount property. This will get properly formatted and then displayed.
            amount = SelectedPayment.Amount;
            Recurrence = SelectedPayment.IsRecurring
                ? SelectedPayment.RecurringPayment.Recurrence
                : 0;
            EndDate = SelectedPayment.IsRecurring
                ? SelectedPayment.RecurringPayment.EndDate
                : DateTime.Now;
            IsEndless = !SelectedPayment.IsRecurring || SelectedPayment.RecurringPayment.IsEndless;
        }

        private void PrepareDefault(string typeString)
        {
            var type = (PaymentType) Enum.Parse(typeof (PaymentType), typeString);

            SetDefaultPayment(type);
            SelectedPayment.ChargedAccount = defaultManager.GetDefaultAccount();
            IsTransfer = type == PaymentType.Transfer;
            EndDate = DateTime.Now;
        }

        private void SetDefaultPayment(PaymentType paymentType)
        {
            SelectedPayment = new Payment
            {
                Type = (int) paymentType,
                Date = DateTime.Now,
                // Assign empty category to reset the GUI
                Category = new Category()
            };
        }

        private async void Save()
        {
            if (SelectedPayment.ChargedAccount == null)
            {
                ShowAccountRequiredMessage();
                return;
            }

            if (SelectedPayment.IsRecurring && !IsEndless && EndDate.Date <= DateTime.Today)
            {
                ShowInvalidEndDateMessage();
                return;
            }

            // Make sure that the old amount is removed to not count the amount twice.
            RemoveOldAmount();
            SelectedPayment.Amount = amount;

            //Create a recurring payment based on the payment or update an existing
            await PrepareRecurringPayment();

            // Save item or update the payment and add the amount to the account
            paymentRepository.Save(SelectedPayment);
            accountRepository.AddPaymentAmount(SelectedPayment);

            Close(this);
        }

        private void RemoveOldAmount()
        {
            if (IsEdit)
            {
                accountRepository.RemovePaymentAmount(SelectedPayment, AccountBeforeEdit);
            }
        }

        private async Task PrepareRecurringPayment()
        {
            if ((IsEdit && await paymentManager.CheckForRecurringPayment(SelectedPayment))
                || SelectedPayment.IsRecurring)
            {
                SelectedPayment.RecurringPayment = RecurringPaymentHelper.
                    GetRecurringFromPayment(SelectedPayment,
                        IsEndless,
                        Recurrence,
                        EndDate);
            }
        }

        private void OpenSelectCategoryList()
        {
            ShowViewModel<SelectCategoryListViewModel>();
        }

        private async void Delete()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage))
            {
                paymentRepository.Delete(paymentRepository.Selected);
                accountRepository.RemovePaymentAmount(SelectedPayment);
                Close(this);
            }
        }

        private async void ShowAccountRequiredMessage()
        {
            await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle,
                Strings.AccountRequiredMessage);
        }

        private async void ShowInvalidEndDateMessage()
        {
            await dialogService.ShowMessage(Strings.InvalidEnddateTitle,
                Strings.InvalidEnddateMessage);
        }


        private void ResetSelection()
        {
            SelectedPayment.Category = null;
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}