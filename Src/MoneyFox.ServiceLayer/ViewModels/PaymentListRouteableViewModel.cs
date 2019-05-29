using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using GenericServices;
using MoneyFox.Foundation.Groups;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of the payment list view.
    /// </summary>
    public class PaymentListViewModel : RouteableViewModelBase
    {
        private ObservableAsPropertyHelper<bool> hasNoPayments;

        private SourceList<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> paymentsSource;
        private SourceList<DateListGroupCollection<PaymentViewModel>> dailySourceList;


        private readonly ICrudServicesAsync crudServices;
        private readonly IPaymentService paymentService;
        private readonly IBackupService backupService;
        private readonly IDialogService dialogService;

        private readonly int accountId;
        private string title;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public PaymentListViewModel(int accountId,
            IScreen hostScreen, 
            ICrudServicesAsync crudServices = null, 
            IPaymentService paymentService= null,
            IDialogService dialogService=null,
            IBackupService backupService=null)
        {
            this.accountId = accountId;
            HostScreen = hostScreen;
            this.crudServices = crudServices ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.paymentService = paymentService ?? Locator.Current.GetService<IPaymentService>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            this.backupService = backupService ?? Locator.Current.GetService<IBackupService>();

            this.WhenActivated(async disposables =>
            {
                Title = (await this.crudServices.ReadSingleAsync<AccountViewModel>(accountId)).Name;

                LoadPayments(new PaymentListFilterChangedMessage(this));

                EditPaymentCommand = ReactiveCommand.Create<PaymentViewModel>(EditPayment).DisposeWith(disposables);
                DeletePaymentCommand = ReactiveCommand.CreateFromTask<PaymentViewModel>(DeletePayment).DisposeWith(disposables);

                hasNoPayments = paymentsSource.Connect()
                                              .QueryWhenChanged()
                                              .Select(x => !x.Any())
                                              .ToProperty(this, x => x.HasNoPayments);
            });
        }
        public override string UrlPathSegment => "PaymentList";
        public override IScreen HostScreen { get; }

        public string Title
        {
            get => title;
            private set => this.RaiseAndSetIfChanged(ref title, value);
        }

        public bool HasNoPayments => hasNoPayments.Value;

        public ReactiveCommand<PaymentViewModel, Unit> EditPaymentCommand { get; set; }
        public ReactiveCommand<PaymentViewModel, Unit> DeletePaymentCommand { get; set; }

        private void LoadPayments(PaymentListFilterChangedMessage filterMessage)
        {
            var paymentQuery = crudServices.ReadManyNoTracked<PaymentViewModel>()
                .HasAccountId(accountId);

            if (filterMessage.IsClearedFilterActive) paymentQuery = paymentQuery.AreCleared();
            if (filterMessage.IsRecurringFilterActive) paymentQuery = paymentQuery.AreRecurring();

            paymentQuery = paymentQuery.Where(x => x.Date >= filterMessage.TimeRangeStart);
            paymentQuery = paymentQuery.Where(x => x.Date <= filterMessage.TimeRangeEnd);

            var loadedPayments = new List<PaymentViewModel>(paymentQuery.OrderDescendingByDate());

            foreach (var payment in loadedPayments) payment.CurrentAccountId = accountId;

            dailySourceList.AddRange(DateListGroupCollection<PaymentViewModel>
                .CreateGroups(loadedPayments,
                    s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                    s => s.Date,
                    itemClickCommand: EditPaymentCommand));

            paymentsSource.AddRange(DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>
                .CreateGroups(dailySourceList.Items,
                    s =>
                    {
                        var date = Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture);
                        return date.ToString("MMMM", CultureInfo.CurrentCulture) + " " + date.Year;
                    },
                    s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture)));
        }

        private void EditPayment(PaymentViewModel paymentViewModel) =>
            HostScreen.Router.Navigate.Execute(new EditPaymentViewModel(paymentViewModel.Id, HostScreen));

        private async Task DeletePayment(PaymentViewModel payment)
        {
            await paymentService.DeletePayment(payment);

#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014

            LoadPayments(new PaymentListFilterChangedMessage(this));
        }

        ///// <inheritdoc />
            //public override async Task Initialize()
            //{

            //    BalanceViewModel = new PaymentListBalanceViewModel(HostScreen, crudServices, balanceCalculationService, AccountId);
            //    RouteableViewActionRouteableViewModel = new PaymentListViewActionViewModel(crudServices,
            //        settingsFacade,
            //        dialogService,
            //        BalanceViewModel,
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
            //        public BalanceViewModel BalanceViewModel
            //        {
            //            get => balanceViewModel;
            //            private set
            //            {
            //                balanceViewModel = value;
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

            //        #endregion
            
            //        private async Task Load()
            //        {
            //            LoadPayments(new PaymentListFilterChangedMessage(this));
            //            //Refresh balance control with the current account
            //            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();
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