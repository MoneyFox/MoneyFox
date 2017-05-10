using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.Helpers;
using MoneyFox.Business.Messages;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service;
using MoneyFox.Service.DataServices;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Handles the logic of the ModifyPayment view
    /// </summary>
    public class ModifyPaymentViewModel : BaseViewModel
    {
        private readonly IDialogService dialogService;
        private readonly IPaymentService paymentService;
        private readonly IAccountService accountService;
        private readonly ISettingsManager settingsManager;
        private readonly IBackupManager backupManager;

        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        // This has to be static in order to keep the value even if you leave the page to select a CategoryViewModel.
        private double amount;
        private PaymentViewModel selectedPayment;
        private PaymentRecurrence recurrence;
        private DateTime endDate;
        private bool isEndless;
        private bool isTransfer;
        private bool isEdit;
        private int paymentId;


        /// <summary>
        ///     Default constructor
        /// </summary>
        public ModifyPaymentViewModel(IPaymentService paymentService,
            IAccountService accountService,
            IDialogService dialogService,
            ISettingsManager settingsManager, 
            IMvxMessenger messenger, IBackupManager backupManager)
        {
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.backupManager = backupManager;
            this.paymentService = paymentService;
            this.accountService = accountService;

            token = messenger.Subscribe<CategorySelectedMessage>(ReceiveMessage);
        }

        #region Commands

        /// <summary>
        ///     Updates the targetAccountViewModel and chargedAccountViewModel Comboboxes' dropdown lists.
        /// </summary>
        public IMvxCommand SelectedItemChangedCommand => new MvxCommand(UpdateOtherComboBox);

        /// <summary>
        ///     Saves the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxCommand SaveCommand => new MvxCommand(Save);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public IMvxCommand GoToSelectCategorydialogCommand => new MvxCommand(OpenSelectCategoryList);

        /// <summary>
        ///     Delets the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxCommand DeleteCommand => new MvxCommand(Delete);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     Resets the CategoryViewModel of the currently selected PaymentViewModel
        /// </summary>
        public IMvxCommand ResetCategoryCommand => new MvxCommand(ResetSelection);

        #endregion

        #region Properties

        /// <summary>
        ///     Indicates if the view is in Edit mode.
        /// </summary>
        public bool IsEdit
        {
            get { return isEdit; }
            private set
            {
                if (isEdit == value) return;
                isEdit = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicates if the PaymentViewModel is a transfer.
        /// </summary>
        public bool IsTransfer
        {
            get { return isTransfer; }
            private set
            {
                if (isTransfer == value) return;
                isTransfer = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicates if the reminder is endless
        /// </summary>
        public bool IsEndless
        {
            get { return isEndless; }
            set
            {
                if (isEndless == value) return;
                isEndless = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The Enddate for recurring PaymentViewModel
        /// </summary>
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The selected recurrence
        /// </summary>
        public PaymentRecurrence Recurrence
        {
            get { return recurrence; }
            set
            {
                recurrence = value;
                RaisePropertyChanged();
            }
        }

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
                // we remove all separator chars to ensure that it works in all regions
                string amountstring = Utilities.RemoveGroupingSeparators(value.ToString());

                double convertedValue;
                if (double.TryParse(amountstring, NumberStyles.Any, CultureInfo.CurrentCulture, out convertedValue))
                {
                    amount = convertedValue;
                }
            }
        }

        /// <summary>
        ///     List with the different recurrence types.
        ///     This has to have the same order as the enum
        /// </summary>
        public List<PaymentRecurrence> RecurrenceList => new List<PaymentRecurrence>
        {
            PaymentRecurrence.Daily,
            PaymentRecurrence.DailyWithoutWeekend,
            PaymentRecurrence.Weekly,
            PaymentRecurrence.Biweekly,
            PaymentRecurrence.Monthly,
            PaymentRecurrence.Bimonthly,
            PaymentRecurrence.Quarterly,
            PaymentRecurrence.Biannually,
            PaymentRecurrence.Yearly
        };

        /// <summary>
        ///     The selected PaymentViewModel
        /// </summary>
        public PaymentViewModel SelectedPayment
        {
            get { return selectedPayment; }
            set
            {
                if (value == null)
                {
                    return;
                }
                selectedPayment = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Gives access to all accounts for Charged Dropdown list
        /// </summary>
        public ObservableCollection<AccountViewModel> ChargedAccounts { get; private set; }

        /// <summary>
        ///     Gives access to all accounts for Target Dropdown list
        /// </summary>
        public ObservableCollection<AccountViewModel> TargetAccounts { get; private set; }

        /// <summary>
        ///     Returns the Title for the page
        /// </summary>
        public string Title => PaymentTypeHelper.GetViewTitleForType(SelectedPayment.Type, IsEdit);

        /// <summary>
        ///     Returns the Header for the AccountViewModel field
        /// </summary>
        public string AccountHeader
            => SelectedPayment?.Type == PaymentType.Income
                ? Strings.TargetAccountLabel
                : Strings.ChargedAccountLabel;

        /// <summary>
        ///     The PaymentViewModel date
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (!IsEdit && (SelectedPayment.Date == DateTime.MinValue))
                {
                    SelectedPayment.Date = DateTime.Now;
                }
                return SelectedPayment.Date;
            }
            set { SelectedPayment.Date = value; }
        }

        private AccountViewModel AccountViewModelBeforeEdit { get; set; }


        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        public int PaymentId
        {
            get { return paymentId; }
            private set
            {
                paymentId = value; 
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        ///     Init the view for a new PaymentViewModel. Is executed after the constructor call.
        /// </summary>
        /// <param name="type">Type of the PaymentViewModel. Is ignored when paymentId is passed.</param>
        /// <param name="paymentId">The id of the PaymentViewModel to edit.</param>
        public async void Init(PaymentType type, int paymentId = 0)
        {
            var accounts = await accountService.GetAllAccounts();
            TargetAccounts = new ObservableCollection<AccountViewModel>(accounts.Select(x => new AccountViewModel(x)));
            ChargedAccounts = new ObservableCollection<AccountViewModel>(TargetAccounts);

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
                SelectedPayment = new PaymentViewModel(await paymentService.GetById(PaymentId));
                PrepareEdit();
            }

            AccountViewModelBeforeEdit = SelectedPayment.ChargedAccount;
        }

        private void PrepareDefault(PaymentType type)
        {
            SelectedPayment = new PaymentViewModel
            {
                Type = type,
                Date = DateTime.Now,
                ChargedAccount = ChargedAccounts.FirstOrDefault(),
            };
            IsTransfer = type == PaymentType.Transfer;
            EndDate = DateTime.Now;
        }

        private void PrepareEdit()
        {            
            // we have to set the AccountViewModel objects here again to ensure that they are identical to the
            // objects in the AccountViewModel collections.
            SelectedPayment.ChargedAccount =
                ChargedAccounts.FirstOrDefault(x => x.Id == selectedPayment.ChargedAccountId);

            if (SelectedPayment.Type == PaymentType.Transfer)
            {
                SelectedPayment.TargetAccount =
                    TargetAccounts.FirstOrDefault(x => x.Id == selectedPayment.TargetAccountId);
            }

            IsTransfer = SelectedPayment.IsTransfer;
            // set the private amount property. This will get properly formatted and then displayed.
            amount = SelectedPayment.Amount;
            Recurrence = SelectedPayment.IsRecurring 
                ? SelectedPayment.RecurringPayment.Recurrence
                : PaymentRecurrence.Daily;

            EndDate = SelectedPayment.IsRecurring && !SelectedPayment.RecurringPayment.IsEndless
                ? SelectedPayment.RecurringPayment.EndDate.Value
                : DateTime.Now;
            IsEndless = !SelectedPayment.IsRecurring || SelectedPayment.RecurringPayment.IsEndless;
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
            if (amount < 0)
            {
                amount *= -1;
            }
            SelectedPayment.Amount = amount;

            //Create a recurring PaymentViewModel based on the PaymentViewModel or update an existing
            await PrepareRecurringPayment();

            // Save item or update the PaymentViewModel and add the amount to the AccountViewModel
            await paymentService.SavePayment(SelectedPayment.Payment);
            settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
            backupManager.EnqueueBackupTask();
#pragma warning restore 4014

            Close(this);
        }

        private void RemoveOldAmount()
        {
            if (IsEdit)
            {
                PaymentAmountHelper.RemovePaymentAmount(SelectedPayment.Payment);
            }
        }

        private async Task PrepareRecurringPayment()
        {
            if (IsEdit
                && selectedPayment.IsRecurring
                && await dialogService.ShowConfirmMessage(Strings.ChangeSubsequentPaymentTitle,
                                                          Strings.ChangeSubsequentPaymentMessage,
                                                          Strings.UpdateAllLabel, Strings.JustThisLabel)
                || !IsEdit && SelectedPayment.IsRecurring)
            {
                SelectedPayment.RecurringPayment = new RecurringPaymentViewModel(
                    RecurringPaymentHelper.GetRecurringFromPayment(SelectedPayment.Payment,
                                                                   IsEndless,
                                                                   Recurrence,
                                                                   EndDate));
            }
        }

        private void OpenSelectCategoryList()
        {
            ShowViewModel<SelectCategoryListViewModel>();
        }

        private async void Delete()
        {
            if (!await dialogService
                .ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) return;

            try
            {
                await paymentService.DeletePayment(SelectedPayment.Payment);
                settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014
                Close(this);
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(Strings.SomethingWentWrongTitle, Strings.ErrorMessageDelete);
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

        private void UpdateOtherComboBox()
        {
            //var tempCollection = new ObservableCollection<AccountViewModel>(ChargedAccounts);
            //foreach (var account in TargetAccounts)
            //{
            //    if (!tempCollection.Contains(account))
            //    {
            //        tempCollection.Add(account);
            //    }
            //}
            //foreach (var account in tempCollection)
            //{
            //    //fills targetaccounts
            //    if (!TargetAccounts.Contains(account)) 
            //    {
            //        TargetAccounts.Add(account);
            //    }

            //    //fills chargedaccounts
            //    if (!ChargedAccounts.Contains(account)) 
            //    {
            //        ChargedAccounts.Add(account);
            //    }
            //}
            //ChargedAccounts.Remove(selectedPayment.TargetAccount);
            //TargetAccounts.Remove(selectedPayment.ChargedAccount);
        }
    }
}