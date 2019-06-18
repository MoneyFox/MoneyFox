using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Parameters;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using HelperFunctions = MoneyFox.Presentation.Utilities.HelperFunctions;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IModifyPaymentViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Indicates if the PaymentViewModel is a transfer.
        /// </summary>
        bool IsTransfer { get; }

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
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        string AmountString { get; }

        /// <summary>
        ///     Gives access to all accounts for Charged Drop down list
        /// </summary>
        ObservableCollection<AccountViewModel> ChargedAccounts { get; }

        /// <summary>
        ///     Gives access to all accounts for Target Drop down list
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
    public abstract class ModifyPaymentViewModel : BaseViewModel, IModifyPaymentViewModel
    {
        private readonly IBackupService backupService;
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        public ModifyPaymentParameter PassedParameter { get; set; }

        private PaymentRecurrence recurrence;
        private PaymentViewModel selectedPayment;
        private ObservableCollection<AccountViewModel> chargedAccounts;
        private ObservableCollection<AccountViewModel> targetAccounts;

        /// <summary>
        ///     Default constructor
        /// </summary>
        protected ModifyPaymentViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            INavigationService navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.navigationService = navigationService;

            MessengerInstance.Register<CategorySelectedMessage>(this, ReceiveMessage);
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
                RaisePropertyChanged(nameof(AccountHeader));
            }
        }

        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        public string AmountString
        {
            get => HelperFunctions.FormatLargeNumbers(SelectedPayment.Amount);
            set
            {
                // we remove all separator chars to ensure that it works in all regions
                string amountString = HelperFunctions.RemoveGroupingSeparators(value);
                if (double.TryParse(amountString, NumberStyles.Any, CultureInfo.CurrentCulture, out double convertedValue))
                {
                    SelectedPayment.Amount = convertedValue;
                }
            }
        }

        /// <summary>
        ///     Gives access to all accounts for Charged Dropdown list
        /// </summary>
        public ObservableCollection<AccountViewModel> ChargedAccounts
        {
            get => chargedAccounts;
            private set
            {
                chargedAccounts = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Gives access to all accounts for Target Dropdown list
        /// </summary>
        public ObservableCollection<AccountViewModel> TargetAccounts
        {
            get => targetAccounts;
            private set
            {
                targetAccounts = value;
                RaisePropertyChanged();
            }
        }

        public virtual string Title { get; set; }

        /// <summary>
        ///     Returns the Header for the AccountViewModel field
        /// </summary>
        public string AccountHeader
            => SelectedPayment?.Type == PaymentType.Income
                ? Strings.TargetAccountLabel
                : Strings.ChargedAccountLabel;

        protected virtual async Task Initialize()
        {
            var accounts = await crudServices.ReadManyNoTracked<AccountViewModel>()
                                             .OrderByName()
                                             .ToListAsync();

            ChargedAccounts = new ObservableCollection<AccountViewModel>(accounts);
            TargetAccounts = new ObservableCollection<AccountViewModel>(accounts);
        }

        protected abstract Task SavePayment();

        private async Task SavePaymentBase()
        {
            if (SelectedPayment.ChargedAccount == null)
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage)
                                   ;
                return;
            }

            await SavePayment();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            if (settingsFacade.IsBackupAutouploadEnabled)
            {
#pragma warning disable 4014
                backupService.EnqueueBackupTask();
#pragma warning restore 4014
            }
        }

        /// <summary>
        ///     Moved to own method for debugg reasons
        /// </summary>
        /// <param name="message">Message stent.</param>
        private void ReceiveMessage(CategorySelectedMessage message)
        {
            if (SelectedPayment == null || message == null) return;
            SelectedPayment.Category = message.SelectedCategory;
        }

        private async Task OpenSelectCategoryList()
        {
            await navigationService.Navigate<SelectCategoryListViewModel>();
        }

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
            var tempCollection = new ObservableCollection<AccountViewModel>(ChargedAccounts);
            foreach (var account in TargetAccounts)
            {
                if (!tempCollection.Contains(account))
                {
                    tempCollection.Add(account);
                }
            }
            foreach (var account in tempCollection)
            {
                //fills targetaccounts
                if (!TargetAccounts.Contains(account))
                {
                    TargetAccounts.Add(account);
                }

                //fills chargedaccounts
                if (!ChargedAccounts.Contains(account))
                {
                    ChargedAccounts.Add(account);
                }
            }
            TargetAccounts.Remove(selectedPayment.ChargedAccount);
        }
    }
}