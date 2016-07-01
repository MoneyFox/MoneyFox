using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Messages;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels {
    [ImplementPropertyChanged]
    public class ModifyPaymentViewModel : BaseViewModel {
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
            IDefaultManager defaultManager) {
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;
            this.paymentManager = paymentManager;
            this.defaultManager = defaultManager;
            this.accountRepository = accountRepository;

            token = MessageHub.Subscribe<CategorySelectedMessage>(ReceiveMessage);
        }

        /// <summary>
        ///     Init the view. Is executed after the constructor call
        /// </summary>
        /// <param name="typeString">Type of the payment.</param>
        /// <param name="isEdit">Weather the payment is in edit mode or not.</param>
        public void Init(string typeString, bool isEdit = false) {
            IsEdit = isEdit;
            IsEndless = true;

            amount = 0;

            if (IsEdit) {
                PrepareEdit();
            }
            else
            {
                // TODO: Remove magic string and just pass in the enum - Seth Bartlett 7/1/2016 12:08PM
                PrepareDefault(typeString);
            }

            AccountBeforeEdit = SelectedPayment.ChargedAccount;
        }

        private void PrepareEdit() {
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

        private void PrepareDefault(string typeString) {
            var type = (PaymentType) Enum.Parse(typeof(PaymentType), typeString);

            SetDefaultPayment(type);
            SelectedPayment.ChargedAccount = defaultManager.GetDefaultAccount();
            IsTransfer = type == PaymentType.Transfer;
            EndDate = DateTime.Now;
        }

        private void SetDefaultPayment(PaymentType paymentType) {
            SelectedPayment = new Payment {
                Type = (int) paymentType,
                Date = DateTime.Now,
                // Assign empty category to reset the GUI
                Category = new Category()
            };
        }

        /// <summary>
        ///     Moved to own method for debugg reasons
        /// </summary>
        /// <param name="message">Message sent.</param>
        private void ReceiveMessage(CategorySelectedMessage message) {
            if (SelectedPayment == null && message == null) return;

            SelectedPayment.Category = message.SelectedCategory;
        }

        private async void Save() {
            if (SelectedPayment.ChargedAccount == null) {
                ShowAccountRequiredMessage();
                return;
            }

            if (SelectedPayment.IsRecurring && !IsEndless && EndDate.Date <= DateTime.Today) {
                ShowInvalidEndDateMessage();
                return;
            }

            // Make sure that the old amount is removed to not count the amount twice.
            RemoveOldAmount();
            SelectedPayment.Amount = amount;

            //Create a recurring payment based on the payment or update an existing
            await PrepareRecurringPayment();

            // Save item or update the payment and add the amount to the account
            bool paymentSucceded = paymentRepository.Save(SelectedPayment);
            bool accountSucceded = accountRepository.AddPaymentAmount(SelectedPayment);
            if (paymentSucceded && accountSucceded)
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;

            Close(this);
        }

        private void RemoveOldAmount() {
            if (IsEdit) {
                accountRepository.RemovePaymentAmount(SelectedPayment, AccountBeforeEdit);
            }
        }

        private async Task PrepareRecurringPayment() {
            if ((IsEdit && await paymentManager.CheckForRecurringPayment(SelectedPayment))
                || SelectedPayment.IsRecurring) {
                SelectedPayment.RecurringPayment = RecurringPaymentHelper.
                    GetRecurringFromPayment(SelectedPayment,
                        IsEndless,
                        Recurrence,
                        EndDate);
            }
        }

        private void OpenSelectCategoryList() {
            ShowViewModel<SelectCategoryListViewModel>();
        }

        private async void Delete() {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) {
                if (await paymentManager.CheckForRecurringPayment(SelectedPayment)) {
                    paymentRepository.DeleteRecurring(SelectedPayment);
                }

                bool paymentSucceded = paymentRepository.Delete(paymentRepository.Selected);
                bool accountSucceded = accountRepository.RemovePaymentAmount(SelectedPayment);
                if (paymentSucceded && accountSucceded)
                    SettingsHelper.LastDatabaseUpdate = DateTime.Now;
                Close(this);
            }
        }

        private async void ShowAccountRequiredMessage() {
            await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle,
                Strings.AccountRequiredMessage);
        }

        private async void ShowInvalidEndDateMessage() {
            await dialogService.ShowMessage(Strings.InvalidEnddateTitle,
                Strings.InvalidEnddateMessage);
        }

        private void ResetSelection() {
            SelectedPayment.Category = null;
        }

        private void Cancel() {
            Close(this);
        }

        #region Commands

        /// <summary>
        ///     Saves the payment or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxCommand SaveCommand => new MvxCommand(Save);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public IMvxCommand GoToSelectCategorydialogCommand => new MvxCommand(OpenSelectCategoryList);

        /// <summary>
        ///     Delets the payment or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxCommand DeleteCommand => new MvxCommand(Delete);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     Resets the category of the currently selected payment
        /// </summary>
        public IMvxCommand ResetCategoryCommand => new MvxCommand(ResetSelection);

        #endregion

        #region Properties

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
        public string AmountString {
            get { return Utilities.FormatLargeNumbers(amount); }
            set {
                double convertedValue;
                if (double.TryParse(value, out convertedValue)) {
                    amount = convertedValue;
                }
            }
        }

        /// <summary>
        ///     List with the different recurrence types.
        /// </summary>
        public List<string> RecurrenceList => new List<string> {
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
        public Payment SelectedPayment {
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
        public DateTime Date {
            get {
                if (!IsEdit && SelectedPayment.Date == DateTime.MinValue) {
                    SelectedPayment.Date = DateTime.Now;
                }
                return SelectedPayment.Date;
            }
            set { SelectedPayment.Date = value; }
        }

        private Account AccountBeforeEdit { get; set; }

        #endregion
    }
}