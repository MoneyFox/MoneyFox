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
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

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

        private ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> source;
        private ObservableCollection<DateListGroup<PaymentViewModel>> dailyList;
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
        public bool IsPaymentsEmtpy => Source != null && !Source.Any();

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
        ///     Returns daily groupped related payments
        /// </summary>
        public ObservableCollection<DateListGroup<PaymentViewModel>> DailyList
        {
            get => dailyList;
            set
            {
                dailyList = value;
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
            await Load();
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

            var loadedPayments = new ObservableCollection<PaymentViewModel>(
                paymentQuery
                    .OrderByDescending(x => x.Data.Date)
                    .Select(x => new PaymentViewModel(x)));


            foreach (var payment in loadedPayments)
            {
                payment.CurrentAccountId = AccountId;
            }

            var dailyItems = DateListGroup<PaymentViewModel>
                .CreateGroups(loadedPayments,
                              CultureInfo.CurrentUICulture,
                              s => s.Date.ToString("D", CultureInfo.InvariantCulture),
                              s => s.Date,
                              itemClickCommand: EditPaymentCommand, itemLongClickCommand: OpenContextMenuCommand);

            DailyList = new ObservableCollection<DateListGroup<PaymentViewModel>>(dailyItems);

            Source = new ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>>(
                DateListGroup<DateListGroup<PaymentViewModel>>
                    .CreateGroups(dailyItems, CultureInfo.CurrentUICulture,
                                  s =>
                                  {
                                      var date = Convert.ToDateTime(s.Key);
                                      return date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + date.Year;
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
            await Load();
        }
    }
}