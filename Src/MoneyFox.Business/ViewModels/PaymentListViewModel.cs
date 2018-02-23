using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Messages;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the payment list view.
    /// </summary>
    public class PaymentListViewModel : MvxViewModel<PaymentListParameter>, IPaymentListViewModel
    {
        private readonly IAccountService accountService;
        private readonly IPaymentService paymentService;
        private readonly IDialogService dialogService;
        private readonly ISettingsManager settingsManager;
        private readonly IBalanceCalculationManager balanceCalculationManager;
        private readonly IBackupManager backupManager;
        private readonly IModifyDialogService modifyDialogService;
        private readonly IMvxNavigationService navigationService;
        private readonly IMvxMessenger messenger;

        private ObservableCollection<PaymentViewModel> relatedPayments;
        private ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> source;
        private IBalanceViewModel balanceViewModel;
        private int accountId;
        private string title;
        private IPaymentListViewActionViewModel viewActionViewModel;

        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public PaymentListViewModel(IAccountService accountService,
            IPaymentService paymentService,
            IDialogService dialogService,
            ISettingsManager settingsManager,
            IBalanceCalculationManager balanceCalculationManager,
            IBackupManager backupManager,
            IModifyDialogService modifyDialogService, 
            IMvxNavigationService navigationService,
            IMvxMessenger messenger)
        {
            this.accountService = accountService;
            this.paymentService = paymentService;
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.balanceCalculationManager = balanceCalculationManager;
            this.backupManager = backupManager;
            this.modifyDialogService = modifyDialogService;
            this.navigationService = navigationService;
            this.messenger = messenger;

            token = messenger.Subscribe<PaymentListFilterChangedMessage>(async message => await LoadPayments(message));
        }

        #region Properties

        /// <summary>
        ///     Indicator if there are payments or not.
        /// </summary>
        public bool IsPaymentsEmtpy => RelatedPayments != null && !RelatedPayments.Any();

        /// <summary>
        ///     Id for the current account.
        /// </summary>
        public int AccountId
        {
            get => accountId;
            private set
            {
                accountId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///      View Model for the balance subview.
        /// </summary>
        public IBalanceViewModel BalanceViewModel
        {
            get => balanceViewModel;
            private set
            {
                balanceViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     View Model for the global actions on the view.
        /// </summary>
        public IPaymentListViewActionViewModel ViewActionViewModel
        {
            get => viewActionViewModel;
            private set {
                if (viewActionViewModel == value) return;
                viewActionViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns all PaymentViewModel who are assigned to this repository
        ///     Currently only used for Android to get the selected PaymentViewModel.
        /// </summary>
        public ObservableCollection<PaymentViewModel> RelatedPayments
        {
            get => relatedPayments;
            set
            {
                if (relatedPayments == value) return;
                relatedPayments = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns groupped related payments
        /// </summary>
        public ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source
        {
            get => source;
            set
            {
                source = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsPaymentsEmtpy));
            }
        }

        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title
        {
            get => title;
            private set
            {
                if (title == value) return;
                title = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Loads the data for this view.
        /// </summary>
        public MvxAsyncCommand LoadCommand => new MvxAsyncCommand(Load);

        /// <summary>
        ///     Opens the Edit Dialog for the passed Payment
        /// </summary>
        public MvxAsyncCommand<PaymentViewModel> EditPaymentCommand => new MvxAsyncCommand<PaymentViewModel>(EditPayment);

        /// <summary>
        ///     Opens a option dialog to select the modify operation
        /// </summary>
        public MvxAsyncCommand<PaymentViewModel> OpenContextMenuCommand => new MvxAsyncCommand<PaymentViewModel>(OpenContextMenu);

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand => new MvxAsyncCommand<PaymentViewModel>(DeletePayment);

        #endregion


        /// <inheritdoc />
        public override void Prepare(PaymentListParameter parameter)
        {
            AccountId = parameter.AccountId;
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            BalanceViewModel = new PaymentListBalanceViewModel(accountService, balanceCalculationManager, AccountId);
            ViewActionViewModel = new PaymentListViewActionViewModel(accountService,
                                                                     settingsManager,
                                                                     dialogService,
                                                                     BalanceViewModel,
                                                                     navigationService,
                                                                     messenger,
                                                                     AccountId);

            await Task.CompletedTask;
        }

        private async Task Load()
        {
            await LoadPayments(new PaymentListFilterChangedMessage(this));

            //Refresh balance control with the current account
            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();
        }

        private async Task LoadPayments(PaymentListFilterChangedMessage filterMessage)
        {
            Title = await accountService.GetAccountName(AccountId);

            var paymentQuery = await paymentService.GetPaymentsByAccountId(AccountId);

            if (filterMessage.IsClearedFilterActive)
            {
                paymentQuery = paymentQuery.Where(x => x.Data.IsCleared);
            }
            if (filterMessage.IsRecurringFilterActive)
            {
                paymentQuery = paymentQuery.Where(x => x.Data.IsRecurring);
            }

            paymentQuery = paymentQuery.Where(x => x.Data.Date <= filterMessage.TimeRangeStart);
            paymentQuery = paymentQuery.Where(x => x.Data.Date >= filterMessage.TimeRangeEnd);

            RelatedPayments = new ObservableCollection<PaymentViewModel>(
                paymentQuery
                    .OrderByDescending(x => x.Data.Date)
                    .Select(x => new PaymentViewModel(x)));

            CreateNestedLists();
        }

        private void CreateNestedLists()
        {
            foreach (var payment in RelatedPayments)
            {
                payment.CurrentAccountId = AccountId;
            }

            var dailyList = DateListGroup<PaymentViewModel>
                .CreateGroups(RelatedPayments,
                              CultureInfo.CurrentUICulture,
                              s => s.Date.ToString("D", CultureInfo.InvariantCulture),
                              s => s.Date,
                              itemClickCommand: EditPaymentCommand, itemLongClickCommand: OpenContextMenuCommand);

            Source = new ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>>(
                DateListGroup<DateListGroup<PaymentViewModel>>
                    .CreateGroups(dailyList, CultureInfo.CurrentUICulture,
                                  s =>
                                  {
                                      var date = Convert.ToDateTime(s.Key);
                                      return date.ToString("MMMM",CultureInfo.InvariantCulture) +" " + date.Year;
                                  },
                                  s => Convert.ToDateTime(s.Key)));
        }

        private async Task EditPayment(PaymentViewModel payment)
        {
            await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                new ModifyPaymentParameter(payment.Id));
        }

        private async Task OpenContextMenu(PaymentViewModel payment)
        {
            var result = await modifyDialogService.ShowEditSelectionDialog();

            switch (result)
            {
                case ModifyOperation.Edit:
                    EditPaymentCommand.Execute(payment);
                    break;

                case ModifyOperation.Delete:
                    DeletePaymentCommand.Execute(payment);
                    break;
            }
        }

        private async Task DeletePayment(PaymentViewModel payment)
        {
            if (!await dialogService
                .ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) return;

            await paymentService.DeletePayment(await paymentService.GetById(payment.Id));

            settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
            backupManager.EnqueueBackupTask();
#pragma warning restore 4014
            await LoadCommand.ExecuteAsync();
        }
    }
}