using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.Commands;
using NLog;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IModifyPaymentViewModel
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
        RelayCommand SelectedItemChangedCommand { get; }

        /// <summary>
        ///     Saves the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        AsyncCommand SaveCommand { get; }

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        RelayCommand GoToSelectCategoryDialogCommand { get; }

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        RelayCommand CancelCommand { get; }

        /// <summary>
        ///     Resets the CategoryViewModel of the currently selected PaymentViewModel
        /// </summary>
        RelayCommand ResetCategoryCommand { get; }
    }

    /// <summary>
    ///     Handles the logic of the ModifyPayment view
    /// </summary>
    public abstract class ModifyPaymentViewModel : ViewModelBase, IModifyPaymentViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly IBackupService backupService;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;
        private ObservableCollection<AccountViewModel> chargedAccounts = new ObservableCollection<AccountViewModel>();

        private PaymentRecurrence recurrence;
        private PaymentViewModel selectedPayment;
        private ObservableCollection<AccountViewModel> targetAccounts = new ObservableCollection<AccountViewModel>();
        private string title;

        /// <summary>
        ///     Default constructor
        /// </summary>
        protected ModifyPaymentViewModel(IMediator mediator,
                                         IMapper mapper,
                                         IDialogService dialogService,
                                         ISettingsFacade settingsFacade,
                                         IBackupService backupService,
                                         INavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.navigationService = navigationService;
            this.mediator = mediator;
            this.mapper = mapper;

            selectedPayment = new PaymentViewModel();

            MessengerInstance.Register<CategorySelectedMessage>(this, async (message) => await ReceiveMessage(message));
        }

        /// <summary>
        ///     Updates the targetAccountViewModel and chargedAccountViewModel Comboboxes' dropdown lists.
        /// </summary>
        public RelayCommand SelectedItemChangedCommand => new RelayCommand(UpdateOtherComboBox);

        /// <summary>
        ///     Saves the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public AsyncCommand SaveCommand => new AsyncCommand(SavePaymentBase);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public RelayCommand GoToSelectCategoryDialogCommand => new RelayCommand(OpenSelectCategoryList);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        /// <summary>
        ///     Resets the CategoryViewModel of the currently selected PaymentViewModel
        /// </summary>
        public RelayCommand ResetCategoryCommand => new RelayCommand(ResetSelection);

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
                RaisePropertyChanged(nameof(IsTransfer));
                RaisePropertyChanged(nameof(AmountString));
            }
        }

        private string amountString;
        public string AmountString
        {
            get => amountString;
            set
            {
                if (amountString == value) return;
                amountString = value;
                RaisePropertyChanged();
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

        public virtual string Title
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

        protected abstract Task SavePayment();

        protected virtual async Task Initialize()
        {
            var accounts = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));

            ChargedAccounts = new ObservableCollection<AccountViewModel>(accounts);
            TargetAccounts = new ObservableCollection<AccountViewModel>(accounts);
        }

        private async Task SavePaymentBase()
        {
            if (SelectedPayment.ChargedAccount == null)
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage);
                return;
            }

            if (decimal.TryParse(AmountString, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal convertedValue))
            {
                SelectedPayment.Amount = convertedValue;
            }
            else
            {
                logManager.Warn($"Amount string {AmountString} could not be parsed to double.");
            }

            if (SelectedPayment.Amount < 0)
            {
                await dialogService.ShowMessage(Strings.AmountMayNotBeNegativeTitle, Strings.AmountMayNotBeNegativeMessage);
                return;
            }

            await SavePayment();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;

#pragma warning disable 4014
            backupService.UploadBackupAsync();
#pragma warning restore 4014
        }

        /// <summary>
        ///     Moved to own method for debugg reasons
        /// </summary>
        /// <param name="message">Message stent.</param>
        private async Task ReceiveMessage(CategorySelectedMessage message)
        {
            if (SelectedPayment == null || message == null) return;
            SelectedPayment.Category = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(message.SelectedCategoryId)));
        }

        private void OpenSelectCategoryList()
        {
            navigationService.NavigateToModal(ViewModelLocator.SelectCategoryList);
        }

        private void ResetSelection()
        {
            SelectedPayment.Category = null;
        }

        private void Cancel()
        {
            navigationService.GoBack();
        }

        private void UpdateOtherComboBox()
        {
            var tempCollection = new ObservableCollection<AccountViewModel>(ChargedAccounts);
            foreach (AccountViewModel account in TargetAccounts)
            {
                if (!tempCollection.Contains(account)) tempCollection.Add(account);
            }

            foreach (AccountViewModel account in tempCollection)
            {
                //fills targetaccounts
                if (!TargetAccounts.Contains(account)) TargetAccounts.Add(account);

                //fills chargedaccounts
                if (!ChargedAccounts.Contains(account)) ChargedAccounts.Add(account);
            }

            TargetAccounts.Remove(selectedPayment.ChargedAccount);
        }
    }
}
