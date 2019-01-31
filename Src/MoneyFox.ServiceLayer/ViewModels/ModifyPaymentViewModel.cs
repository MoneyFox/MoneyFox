using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
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
    public abstract class ModifyPaymentViewModel : BaseNavigationViewModel<ModifyPaymentParameter>,
        IModifyPaymentViewModel
    {
        private readonly IBackupService backupService;
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        protected ModifyPaymentParameter PassedParameter;

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
            IMvxMessenger messenger,
            IBackupService backupService,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.navigationService = navigationService;

            token = messenger.Subscribe<CategorySelectedMessage>(ReceiveMessage);
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

        /// <inheritdoc />
        public override void Prepare(ModifyPaymentParameter parameter)
        {
            PassedParameter = parameter;
            RaisePropertyChanged(nameof(Title));
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            var accounts = await crudServices.ReadManyNoTracked<AccountViewModel>()
                                             .ToListAsync()
                                             .ConfigureAwait(true);
            ChargedAccounts = new ObservableCollection<AccountViewModel>(accounts);
            TargetAccounts = new ObservableCollection<AccountViewModel>(accounts);
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            // We have to raise this here so that the UI knows enables the button again.
            RaisePropertyChanged(nameof(GoToSelectCategorydialogCommand));
        }

        protected abstract Task SavePayment();

        private async Task SavePaymentBase()
        {
            if (SelectedPayment.ChargedAccount == null)
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage)
                                   .ConfigureAwait(true);
                return;
            }

            await SavePayment().ConfigureAwait(true);

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask().ConfigureAwait(true);
#pragma warning restore 4014
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
            await navigationService.Navigate<SelectCategoryListViewModel>()
                                   .ConfigureAwait(true);
        }

        private void ResetSelection()
        {
            SelectedPayment.Category = null;
        }

        private async Task Cancel()
        {
            await navigationService.Close(this).ConfigureAwait(true);
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
    }
}