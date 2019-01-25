using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.Parameters;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public interface IModifyPaymentViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Indicates if the PaymentViewModel is a transfer.
        /// </summary>
        bool IsTransfer { get; }

        /// <summary>
        ///     Indicates if the reminder is endless
        /// </summary>
        bool IsEndless { get; }

        /// <summary>
        ///     The Enddate for recurring PaymentViewModel
        /// </summary>
        DateTime EndDate { get; }

        /// <summary>
        ///     The selected recurrence
        /// </summary>
        PaymentRecurrence Recurrence { get; }

        /// <summary>
        ///     List with the different recurrence types.
        ///     This has to have the same order as the enum
        /// </summary>
        List<PaymentRecurrence> RecurrenceList { get; }

        /// <summary>
        ///     The selected PaymentViewModel
        /// </summary>
        PaymentViewModel SelectedPayment { get; }

        /// <summary>
        ///     Gives access to all accounts for Charged Dropdown list
        /// </summary>
        ObservableCollection<AccountViewModel> ChargedAccounts { get; }

        /// <summary>
        ///     Gives access to all accounts for Target Dropdown list
        /// </summary>
        ObservableCollection<AccountViewModel> TargetAccounts { get; }

        /// <summary>
        ///     Returns the Title for the page
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     Returns the Header for the AccountViewModel field
        /// </summary>
        string AccountHeader { get; }

        /// <summary>
        ///     The PaymentViewModel date
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        ///     Updates the targetAccountViewModel and chargedAccountViewModel Comboboxes' dropdown lists.
        /// </summary>
        IMvxCommand SelectedItemChangedCommand { get; }

        /// <summary>
        ///     Saves the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        IMvxAsyncCommand SaveCommand { get; }

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        IMvxAsyncCommand GoToSelectCategorydialogCommand { get; }

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        IMvxAsyncCommand CancelCommand { get; }

        /// <summary>
        ///     Resets the CategoryViewModel of the currently selected PaymentViewModel
        /// </summary>
        IMvxCommand ResetCategoryCommand { get; }
    }

    /// <summary>
    ///     Handles the logic of the ModifyPayment view
    /// </summary>
    public abstract class ModifyPaymentViewModel : BaseNavigationViewModel<ModifyPaymentParameter>, IModifyPaymentViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IBackupManager backupManager;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        private double amount;
        private DateTime endDate;
        private bool isEdit;
        private bool isEndless;
        private bool isTransfer;

        protected ModifyPaymentParameter passedParameter;

        private bool preventNullingSelected;
        private PaymentRecurrence recurrence;
        private PaymentViewModel selectedPayment;
        private string title;

        /// <summary>
        ///     Default constructor
        /// </summary>
        protected ModifyPaymentViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IMvxMessenger messenger,
            IBackupManager backupManager,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.backupManager = backupManager;
            this.navigationService = navigationService;

            token = messenger.Subscribe<CategorySelectedMessage>(ReceiveMessage);
        }

        /// <inheritdoc />
        public override void Prepare(ModifyPaymentParameter parameter)
        {
            passedParameter = parameter;
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            var accounts = await crudServices.ReadManyNoTracked<AccountViewModel>().ToListAsync();
            ChargedAccounts = new ObservableCollection<AccountViewModel>(TargetAccounts);
            TargetAccounts = new ObservableCollection<AccountViewModel>(accounts);
        }

        //private void PrepareDefault(PaymentType type)
        //{
        //    SelectedPayment = new PaymentViewModel
        //    {
        //        Type = type,
        //        Date = DateTime.Now,
        //        ChargedAccount = ChargedAccounts.FirstOrDefault()
        //    };
        //    IsTransfer = type == PaymentType.Transfer;
        //    EndDate = DateTime.Now;
        //}

        //private void PrepareEdit()
        //{
        //    // we have to set the AccountViewModel objects here again to ensure that they are identical to the
        //    // objects in the AccountViewModel collections.
        //    SelectedPayment.ChargedAccount =
        //        ChargedAccounts.FirstOrDefault(x => x.Id == selectedPayment.ChargedAccountId);

        //    if (SelectedPayment.Type == PaymentType.Transfer)
        //        SelectedPayment.TargetAccount =
        //            TargetAccounts.FirstOrDefault(x => x.Id == selectedPayment.TargetAccountId);

        //    IsTransfer = SelectedPayment.IsTransfer;
        //    // set the private amount property. This will get properly formatted and then displayed.
        //    amount = SelectedPayment.Amount;
        //    Recurrence = SelectedPayment.IsRecurring
        //        ? SelectedPayment.RecurringPayment.Recurrence
        //        : PaymentRecurrence.Daily;

        //    if (SelectedPayment.RecurringPayment?.EndDate != null)
        //        EndDate = SelectedPayment.IsRecurring && !SelectedPayment.RecurringPayment.IsEndless
        //            ? SelectedPayment.RecurringPayment.EndDate.Value
        //            : DateTime.Now;
        //    IsEndless = !SelectedPayment.IsRecurring || SelectedPayment.RecurringPayment.IsEndless;
        //}

        protected abstract Task SavePayment();

        private async Task SavePaymentBase()
        {
            if (SelectedPayment.ChargedAccount == null)
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage);
                return;
            }

            await SavePayment();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            await backupManager.EnqueueBackupTask();
        }

        /// <inheritdoc />
        public override void ViewDisappearing()
        {
            if (!preventNullingSelected) SelectedPayment = null;
        }

        /// <summary>
        ///     Moved to own method for debugg reasons
        /// </summary>
        /// <param name="message">Message stent.</param>
        private void ReceiveMessage(CategorySelectedMessage message)
        {
            if (SelectedPayment == null || message == null) return;
            SelectedPayment.Category = message.SelectedCategory;
            preventNullingSelected = false;
        }

//        private async Task Save()
//        {
//            try
//            {
//                if (SelectedPayment.ChargedAccount == null)
//                {
//                    ShowAccountRequiredMessage();
//                    return;
//                }

//                if (SelectedPayment.IsRecurring && !IsEndless && EndDate.Date <= DateTime.Today)
//                {
//                    ShowInvalidEndDateMessage();
//                    return;
//                }

//                // Make sure that the old amount is removed to not count the amount twice.
//                RemoveOldAmount();
//                if (amount < 0) amount *= -1;
//                SelectedPayment.Amount = amount;

//                // We remove clearance, when the payment is now in the future.
//                if (SelectedPayment.Date > DateTime.Now) SelectedPayment.IsCleared = false;

//                //Create a recurring PaymentViewModel based on the PaymentViewModel or update an existing
//                await PrepareRecurringPayment();

//                // Save item or update the PaymentViewModel and add the amount to the AccountViewModel
//                await paymentService.SavePayments(SelectedPayment.Payment);
//                settingsManager.LastDatabaseUpdate = DateTime.Now;
//#pragma warning disable 4014
//                backupManager.EnqueueBackupTask();
//#pragma warning restore 4014

//                await navigationService.Close(this);
//            }
//            catch (Exception ex)
//            {
//                Crashes.TrackError(ex);
//                await dialogService.ShowMessage(Strings.GeneralErrorTitle, ex.ToString());
//            }
//        }

        //private void RemoveOldAmount()
        //{
        //    if (IsEdit) PaymentAmountHelper.RemovePaymentAmount(SelectedPayment.Payment);
        //}

        //private async Task PrepareRecurringPayment()
        //{
        //    if (IsEdit
        //        && SelectedPayment.IsRecurring
        //        && await dialogService.ShowConfirmMessage(Strings.ChangeSubsequentPaymentTitle,
        //            Strings.ChangeSubsequentPaymentMessage,
        //            Strings.UpdateAllLabel, Strings.JustThisLabel)
        //        || !IsEdit && SelectedPayment.IsRecurring)
        //    {
        //        // We save the ID of the recurring payment who was already saved and assign it afterwards again.
        //        var oldId = SelectedPayment.Payment.Data.RecurringPayment?.Id ?? 0;
        //        SelectedPayment.Payment.Data.RecurringPayment = RecurringPaymentHelper.GetRecurringFromPayment(
        //            SelectedPayment.Payment,
        //            IsEndless,
        //            Recurrence,
        //            EndDate).Data;
        //        SelectedPayment.Payment.Data.RecurringPayment.Id = oldId;
        //    }
        //}

        private async Task OpenSelectCategoryList()
        {
            preventNullingSelected = true;
            await navigationService.Navigate<SelectCategoryListViewModel>();
        }

//        private async Task Delete()
//        {
//            if (!await dialogService
//                .ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) return;

//            try
//            {
//                if (SelectedPayment.IsRecurring
//                    && await dialogService.ShowConfirmMessage(Strings.DeleteRecurringPaymentTitle,
//                        Strings.DeleteRecurringPaymentMessage))
//                {
//                    var paymentToDelete = await paymentService.GetById(SelectedPayment.Id);
//                    await recurringPaymentService.DeletePayment(paymentToDelete.Data.RecurringPayment);
//                }

//                await paymentService.DeletePayment(SelectedPayment.Payment);
//                settingsManager.LastDatabaseUpdate = DateTime.Now;
//#pragma warning disable 4014
//                backupManager.EnqueueBackupTask();
//#pragma warning restore 4014
//                await navigationService.Close(this);
//            }
//            catch (Exception ex)
//            {
//                Crashes.TrackError(ex);
//                await dialogService.ShowMessage(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
//            }
//        }

        //private async void ShowInvalidEndDateMessage()
        //{
        //    await dialogService.ShowMessage(Strings.InvalidEnddateTitle,
        //        Strings.InvalidEnddateMessage);
        //}

        private void ResetSelection()
        {
            SelectedPayment.Category = null;
        }

        private async Task Cancel()
        {
            await navigationService.Close(this);
        }

        private void UpdateOtherComboBox()
        {
            //TODO: Refactor this
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

        /// <summary>
        ///     Updates the targetAccountViewModel and chargedAccountViewModel Comboboxes' dropdown lists.
        /// </summary>
        public IMvxCommand SelectedItemChangedCommand => new MvxCommand(UpdateOtherComboBox);

        /// <summary>
        ///     Saves the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public IMvxAsyncCommand SaveCommand => new MvxAsyncCommand(SavePaymentBase);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public IMvxAsyncCommand GoToSelectCategorydialogCommand => new MvxAsyncCommand(OpenSelectCategoryList);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public IMvxAsyncCommand CancelCommand => new MvxAsyncCommand(Cancel);

        /// <summary>
        ///     Resets the CategoryViewModel of the currently selected PaymentViewModel
        /// </summary>
        public IMvxCommand ResetCategoryCommand => new MvxCommand(ResetSelection);

        /// <summary>
        ///     Indicates if the PaymentViewModel is a transfer.
        /// </summary>
        public bool IsTransfer => SelectedPayment.IsTransfer;

        /// <summary>
        ///     Indicates if the reminder is endless
        /// </summary>
        public bool IsEndless
        {
            get => isEndless;
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
            get => endDate;
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
            get => recurrence;
            set
            {
                if (recurrence == value) return;

                recurrence = value;
                RaisePropertyChanged();
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
            get => selectedPayment;
            set
            {
                if (selectedPayment == value) return;
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
        public string Title
        {
            get => title;
            set
            {
                if (title == value) return;
                title = value;
                RaisePropertyChanged();
            }
        }

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
                if (!IsEdit && SelectedPayment.Date == DateTime.MinValue) SelectedPayment.Date = DateTime.Now.Date;
                return SelectedPayment.Date;
            }
            set
            {
                if (SelectedPayment.Date == value) return;
                SelectedPayment.Date = value;
                RaisePropertyChanged();
            }
        }

        private AccountViewModel AccountViewModelBeforeEdit { get; set; }
    }
}