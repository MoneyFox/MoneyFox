using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Messages;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Plugins.Messenger;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyPaymentViewModel : BaseViewModel
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IDialogService dialogService;
        private readonly IPaymentManager paymentManager;

        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        // This has to be static in order to keep the value even if you leave the page to select a category.
        private double amount;
        private Payment selectedPayment;

        public ModifyPaymentViewModel(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IDialogService dialogService,
            IPaymentManager paymentManager)
        {
            this.dialogService = dialogService;
            this.paymentManager = paymentManager;
            this.paymentRepository = paymentRepository;

            TargetAccounts = new ObservableCollection<Account>(accountRepository.GetList());
            ChargedAccounts = new ObservableCollection<Account>(TargetAccounts);

            token = MessageHub.Subscribe<CategorySelectedMessage>(ReceiveMessage);
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        public int PaymentId { get; private set; }

        /// <summary>
        ///     Init the view for a new Payment. Is executed after the constructor call.
        /// </summary>
        /// <param name="type">Type of the payment. Is ignored when paymentId is passed.</param>
        /// <param name="paymentId">The id of the payment to edit.</param>
        public void Init(PaymentType type, int paymentId = 0)
        {
            if (paymentId == 0)
            {
                IsEdit = false;
                IsEndless = true;

                amount = 0;
                PrepareDefault(type);
            }
            else
            {
                IsEdit = true;
                PaymentId = paymentId;
                selectedPayment = paymentRepository.FindById(PaymentId);
                PrepareEdit();
            }

            AccountBeforeEdit = SelectedPayment.ChargedAccount;
        }

        private void PrepareDefault(PaymentType type)
        {
            SetDefaultPayment(type);
            IsTransfer = type == PaymentType.Transfer;
            EndDate = DateTime.Now;
        }

        private void PrepareEdit()
        {
            IsTransfer = SelectedPayment.IsTransfer;
            // set the private amount property. This will get properly formatted and then displayed.
            amount = SelectedPayment.Amount;
            RecurrenceString = SelectedPayment.IsRecurring
                ? RecurrenceList[SelectedPayment.RecurringPayment.Recurrence] 
                : "";
            EndDate = SelectedPayment.IsRecurring
                ? SelectedPayment.RecurringPayment.EndDate
                : DateTime.Now;
            IsEndless = !SelectedPayment.IsRecurring || SelectedPayment.RecurringPayment.IsEndless;
        }

        private void SetDefaultPayment(PaymentType paymentType)
        {
            SelectedPayment = new Payment
            {
                Type = (int) paymentType,
                Date = DateTime.Now,
                // Assign empty category to reset the GUI
                Category = new Category(),
                ChargedAccount = DefaultHelper.GetDefaultAccount(ChargedAccounts.ToList())
            };
        }

        /// <summary>
        ///     Moved to own method for debugg reasons
        /// </summary>
        /// <param name="message">Message sent.</param>
        private void ReceiveMessage(CategorySelectedMessage message)
        {
            if (SelectedPayment == null || message == null) return;
            SelectedPayment.Category = message.SelectedCategory;
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
            var paymentSucceded = paymentManager.SavePayment(SelectedPayment);
            var accountSucceded = paymentManager.AddPaymentAmount(SelectedPayment);
            if (paymentSucceded && accountSucceded)
            {
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;
            }

            Close(this);
        }

        private void RemoveOldAmount()
        {
            if (IsEdit)
            {
                paymentManager.RemovePaymentAmount(SelectedPayment, AccountBeforeEdit);
            }
        }

        private async Task PrepareRecurringPayment()
        {
            if ((IsEdit && await paymentManager.CheckRecurrenceOfPayment(SelectedPayment))
                || SelectedPayment.IsRecurring)
            {
                SelectedPayment.RecurringPayment = RecurringPaymentHelper.
                    GetRecurringFromPayment(SelectedPayment,
                        IsEndless,
                        GetEnumIntFromString,
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
                if (await paymentManager.CheckRecurrenceOfPayment(SelectedPayment))
                {
                    paymentManager.RemoveRecurringForPayment(SelectedPayment);
                }

                var paymentSucceded = paymentRepository.Delete(SelectedPayment);
                var accountSucceded = paymentManager.RemovePaymentAmount(SelectedPayment);
                if (paymentSucceded && accountSucceded)
                    SettingsHelper.LastDatabaseUpdate = DateTime.Now;
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
            SelectedPayment = paymentRepository.FindById(selectedPayment.Id);
            Close(this);
        }

        private int GetEnumIntFromString => RecurrenceList.IndexOf(RecurrenceString);

        private void UpdateOtherComboBox()
        {
           ObservableCollection<Account> tempCollection = new ObservableCollection<Account>(ChargedAccounts);
            foreach (Account i in TargetAccounts)
            {
                if (!tempCollection.Contains(i))
                    tempCollection.Add(i);
            }
            foreach (Account i in tempCollection)
            {
                if (!TargetAccounts.Contains(i))//fills targetaccounts
                   TargetAccounts.Add(i);
                
                if (!ChargedAccounts.Contains(i))//fills chargedaccounts
                   ChargedAccounts.Add(i);
            }
            ChargedAccounts.Remove(selectedPayment.TargetAccount);
            TargetAccounts.Remove(selectedPayment.ChargedAccount);
        }


        #region Commands

        /// <summary>
        ///     Updates the TargetAccount and ChargedAccount Comboboxes' dropdown lists.
        /// </summary>
        public IMvxCommand SelectedItemChangedCommand => new MvxCommand(UpdateOtherComboBox);

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
        public string RecurrenceString { get; set; }

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
        ///     This has to have the same order as the enum
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
            get { return selectedPayment; }
            set
            {
                if (value == null) return;
                selectedPayment = value;
            }
        }

        /// <summary>
        ///     Gives access to all accounts for Charged Dropdown list
        /// </summary>
        public ObservableCollection<Account> ChargedAccounts { get; set;}

        /// <summary>
        ///     Gives access to all accounts for Target Dropdown list
        /// </summary>
        public ObservableCollection<Account> TargetAccounts { get; set;}

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

        #endregion
    }
}