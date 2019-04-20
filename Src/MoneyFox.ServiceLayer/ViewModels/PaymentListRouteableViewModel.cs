using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Groups;
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
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of the payment list view.
    /// </summary>
    public class PaymentListRouteableViewModel : RouteableViewModelBase
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
        private BalanceRouteableViewModel balanceRouteableViewModel;
        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> dailyList;

        private ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> source;
        private string title;
        private PaymentListViewActionViewModel viewActionViewModel;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public PaymentListRouteableViewModel(IScreen hostScreen, 
            ICrudServicesAsync crudServices = null, 
            IPaymentService paymentService= null,
            IDialogService dialogService=null,
            ISettingsFacade settingsFacade=null,
            IBalanceCalculationService balanceCalculationService=null,
            IBackupService backupService=null)
        {
            HostScreen = hostScreen;
            this.crudServices = crudServices ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.paymentService = paymentService ?? Locator.Current.GetService<IPaymentService>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            this.settingsFacade = settingsFacade ?? Locator.Current.GetService<ISettingsFacade>();
            this.balanceCalculationService = balanceCalculationService ?? Locator.Current.GetService<IBalanceCalculationService>();
            this.backupService = backupService ?? Locator.Current.GetService<IBackupService>();

            //token = messenger.Subscribe<PaymentListFilterChangedMessage>(LoadPayments);
        }

        public override string UrlPathSegment => "PaymentList";
        public override IScreen HostScreen { get; }


        /// <inheritdoc />
        //public override void Prepare(PaymentListParameter parameter)
        //{
        //    AccountId = parameter.AccountId;
        //}

        ///// <inheritdoc />
        //public override async Task Initialize()
        //{
        //    Title = (await crudServices.ReadSingleAsync<AccountViewModel>(AccountId)).Name;

        //    BalanceRouteableViewModel = new PaymentListBalanceRouteableViewModel(HostScreen, crudServices, balanceCalculationService, AccountId);
        //    RouteableViewActionRouteableViewModel = new PaymentListViewActionViewModel(crudServices,
        //        settingsFacade,
        //        dialogService,
        //        BalanceRouteableViewModel,
        //        messenger,
        //        AccountId,
        //        logProvider,
        //        navigationService);
        //}

        ///// <inheritdoc />
        //public override async void ViewAppearing()
        //{
        //    dialogService.ShowLoadingDialog();
        //    await Task.Run(async () => await Load());
        //    dialogService.HideLoadingDialog();
        //}

//        #region Properties

//        /// <summary>
//        ///     Indicator if there are payments or not.
//        /// </summary>
//        public bool IsPaymentsEmpty => Source != null && !Source.Any();

//        /// <summary>
//        ///     Id for the current account.
//        /// </summary>
//        public int AccountId
//        {
//            get => accountId;
//            private set
//            {
//                accountId = value;
//                RaisePropertyChanged();
//            }
//        }

//        /// <summary>
//        ///     View Model for the balance subview.
//        /// </summary>
//        public BalanceRouteableViewModel BalanceRouteableViewModel
//        {
//            get => balanceRouteableViewModel;
//            private set
//            {
//                balanceRouteableViewModel = value;
//                RaisePropertyChanged();
//            }
//        }

//        /// <summary>
//        ///     View Model for the global actions on the view.
//        /// </summary>
//        public IPaymentListViewActionViewModel RouteableViewActionRouteableViewModel
//        {
//            get => viewActionViewModel;
//            private set
//            {
//                if (viewActionViewModel == value) return;
//                viewActionViewModel = value;
//                RaisePropertyChanged();
//            }
//        }

//        /// <summary>
//        ///     Returns grouped related payments
//        /// </summary>
//        public ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source
//        {
//            get => source;
//            private set
//            {
//                source = value;
//                RaisePropertyChanged();
//                // ReSharper disable once ExplicitCallerInfoArgument
//                RaisePropertyChanged(nameof(IsPaymentsEmpty));
//            }
//        }

//        /// <summary>
//        ///     Returns daily grouped related payments
//        /// </summary>
//        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList
//        {
//            get => dailyList;
//            private set
//            {
//                dailyList = value;
//                RaisePropertyChanged();
//                // ReSharper disable once ExplicitCallerInfoArgument
//                RaisePropertyChanged(nameof(IsPaymentsEmpty));
//            }
//        }

//        /// <summary>
//        ///     Returns the name of the account title for the current page
//        /// </summary>
//        public string Title
//        {
//            get => title;
//            private set
//            {
//                if (title == value) return;
//                title = value;
//                RaisePropertyChanged();
//            }
//        }

//        #endregion

//        #region Commands

//        /// <summary>
//        ///     Opens the Edit Dialog for the passed Payment
//        /// </summary>
//        public MvxAsyncCommand<PaymentViewModel> EditPaymentCommand =>
//            new MvxAsyncCommand<PaymentViewModel>(EditPayment);

//        /// <summary>
//        ///     Deletes the passed PaymentViewModel.
//        /// </summary>
//        public MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand =>
//            new MvxAsyncCommand<PaymentViewModel>(DeletePayment);

//        #endregion

//        private async Task Load()
//        {
//            LoadPayments(new PaymentListFilterChangedMessage(this));
//            //Refresh balance control with the current account
//            await BalanceRouteableViewModel.UpdateBalanceCommand.ExecuteAsync();
//        }

//        private void LoadPayments(PaymentListFilterChangedMessage filterMessage)
//        {
//            var paymentQuery = crudServices.ReadManyNoTracked<PaymentViewModel>()
//                .HasAccountId(AccountId);

//            if (filterMessage.IsClearedFilterActive) paymentQuery = paymentQuery.AreCleared();
//            if (filterMessage.IsRecurringFilterActive) paymentQuery = paymentQuery.AreRecurring();

//            paymentQuery = paymentQuery.Where(x => x.Date >= filterMessage.TimeRangeStart);
//            paymentQuery = paymentQuery.Where(x => x.Date <= filterMessage.TimeRangeEnd);

//            var loadedPayments = new List<PaymentViewModel>(
//                paymentQuery.OrderDescendingByDate());

//            foreach (var payment in loadedPayments) payment.CurrentAccountId = AccountId;

//            var dailyItems = DateListGroupCollection<PaymentViewModel>
//                .CreateGroups(loadedPayments,
//                    s => s.Date.ToString("D", CultureInfo.CurrentCulture),
//                    s => s.Date,
//                    itemClickCommand: EditPaymentCommand);

//            DailyList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);

//            Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>(
//                DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>
//                    .CreateGroups(dailyItems,
//                        s =>
//                        {
//                            var date = Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture);
//                            return date.ToString("MMMM", CultureInfo.CurrentCulture) + " " + date.Year;
//                        },
//                        s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture)));
//        }

//        private async Task EditPayment(PaymentViewModel payment)
//        {
//            await navigationService.Navigate<EditPaymentViewModel, ModifyPaymentParameter>(
//                new ModifyPaymentParameter(payment.Id))
//                ;
//        }

//        private async Task DeletePayment(PaymentViewModel payment)
//        {
//            await paymentService.DeletePayment(payment);

//#pragma warning disable 4014
//            backupService.EnqueueBackupTask();
//#pragma warning restore 4014
//            await Load();
//        }
    }
}