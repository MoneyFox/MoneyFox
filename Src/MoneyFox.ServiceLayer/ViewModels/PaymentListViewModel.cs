using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of the payment list view.
    /// </summary>
    public class PaymentListViewModel : MvxViewModel<PaymentListParameter>, IPaymentListViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IPaymentService paymentService;
        private readonly IBackupService backupService;
        private readonly IBalanceCalculationService balanceCalculationService;
        private readonly IDialogService dialogService;
        private readonly IMvxLogProvider logProvider;
        private readonly IMvxMessenger messenger;
        private readonly IMvxNavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;
        private int accountId;
        private IBalanceViewModel balanceViewModel;
        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> dailyList;

        private ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> source;
        private string title;
        private IPaymentListViewActionViewModel viewActionViewModel;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public PaymentListViewModel(ICrudServicesAsync crudServices, 
            IPaymentService paymentService,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBalanceCalculationService balanceCalculationService,
            IBackupService backupService,
            IMvxNavigationService navigationService,
            IMvxMessenger messenger,
            IMvxLogProvider logProvider)
        {
            this.crudServices = crudServices;
            this.paymentService = paymentService;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.balanceCalculationService = balanceCalculationService;
            this.backupService = backupService;
            this.navigationService = navigationService;
            this.messenger = messenger;
            this.logProvider = logProvider;

            token = messenger.Subscribe<PaymentListFilterChangedMessage>(LoadPayments);
        }

        /// <inheritdoc />
        public override void Prepare(PaymentListParameter parameter)
        {
            AccountId = parameter.AccountId;
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            Title = (await crudServices.ReadSingleAsync<AccountViewModel>(AccountId).ConfigureAwait(true)).Name;

            BalanceViewModel = new PaymentListBalanceViewModel(crudServices, balanceCalculationService, AccountId,
                logProvider, navigationService);
            ViewActionViewModel = new PaymentListViewActionViewModel(crudServices,
                settingsFacade,
                dialogService,
                BalanceViewModel,
                messenger,
                AccountId,
                logProvider,
                navigationService);
        }

        /// <inheritdoc />
        public override async void ViewAppearing()
        {
            dialogService.ShowLoadingDialog();
            await Task.Run(async () => await Load());
            dialogService.HideLoadingDialog();
        }

        private async Task Load()
        {
            LoadPayments(new PaymentListFilterChangedMessage(this));
            //Refresh balance control with the current account
            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync().ConfigureAwait(true);
        }

        private void LoadPayments(PaymentListFilterChangedMessage filterMessage)
        {
            var paymentQuery = crudServices.ReadManyNoTracked<PaymentViewModel>()
                .HasChargedAccountId(AccountId);

            if (filterMessage.IsClearedFilterActive) paymentQuery = paymentQuery.Where(x => x.IsCleared);
            if (filterMessage.IsRecurringFilterActive) paymentQuery = paymentQuery.Where(x => x.IsRecurring);

            paymentQuery = paymentQuery.Where(x => x.Date >= filterMessage.TimeRangeStart);
            paymentQuery = paymentQuery.Where(x => x.Date <= filterMessage.TimeRangeEnd);

            var loadedPayments = new List<PaymentViewModel>(
                paymentQuery.OrderByDescending(x => x.Date));

            foreach (var payment in loadedPayments) payment.CurrentAccountId = AccountId;

            var dailyItems = DateListGroupCollection<PaymentViewModel>
                .CreateGroups(loadedPayments,
                    s => s.Date.ToString("D", CultureInfo.CurrentUICulture),
                    s => s.Date,
                    itemClickCommand: EditPaymentCommand);

            DailyList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);

            Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>(
                DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>
                    .CreateGroups(dailyItems,
                        s =>
                        {
                            var date = Convert.ToDateTime(s.Key);
                            return date.ToString("MMMM", CultureInfo.CurrentUICulture) + " " + date.Year;
                        },
                        s => Convert.ToDateTime(s.Key)));
        }

        private async Task EditPayment(PaymentViewModel payment)
        {
            await navigationService.Navigate<EditPaymentViewModel, ModifyPaymentParameter>(
                new ModifyPaymentParameter(payment.Id))
                .ConfigureAwait(true);
        }

        private async Task DeletePayment(PaymentViewModel payment)
        {
            await paymentService.DeletePayment(payment).ConfigureAwait(true);

#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            await Load();
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
        ///     View Model for the balance subview.
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
            private set
            {
                if (viewActionViewModel == value) return;
                viewActionViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns groupped related payments
        /// </summary>
        public ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source
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
        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList
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
        public MvxAsyncCommand<PaymentViewModel> EditPaymentCommand =>
            new MvxAsyncCommand<PaymentViewModel>(EditPayment);

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand =>
            new MvxAsyncCommand<PaymentViewModel>(DeletePayment);

        #endregion
    }
}