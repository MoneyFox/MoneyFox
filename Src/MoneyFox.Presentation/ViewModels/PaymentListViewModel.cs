using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation.Groups;
using MoneyFox.Presentation.Messages;
using MoneyFox.Presentation.Parameters;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
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
        private readonly INavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

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
            INavigationService navigationService)
        {
            this.crudServices = crudServices;
            this.paymentService = paymentService;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.balanceCalculationService = balanceCalculationService;
            this.backupService = backupService;
            this.navigationService = navigationService;

            Messenger.Default.Register<PaymentListFilterChangedMessage>(this, LoadPayments);
        }

        /// <inheritdoc />
        public override void Prepare(PaymentListParameter parameter)
        {
            AccountId = parameter.AccountId;
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            Title = (await crudServices.ReadSingleAsync<AccountViewModel>(AccountId)).Name;

            BalanceViewModel = new PaymentListBalanceViewModel(crudServices, balanceCalculationService, AccountId);
            ViewActionViewModel = new PaymentListViewActionViewModel(crudServices,
                settingsFacade,
                dialogService,
                BalanceViewModel,
                AccountId,
                navigationService);
        }

        /// <inheritdoc />
        public override async void ViewAppearing()
        {
            dialogService.ShowLoadingDialog();
            await Task.Run(Load);
            dialogService.HideLoadingDialog();
        }

        #region Properties

        /// <summary>
        ///     Indicator if there are payments or not.
        /// </summary>
        public bool IsPaymentsEmpty => Source != null && !Source.Any();

        /// <summary>
        ///     Id for the current account.
        /// </summary>
        public int AccountId
        {
            get => accountId;
            set
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
        ///     Returns grouped related payments
        /// </summary>
        public ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source
        {
            get => source;
            private set
            {
                source = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsPaymentsEmpty));
            }
        }

        /// <summary>
        ///     Returns daily grouped related payments
        /// </summary>
        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList
        {
            get => dailyList;
            private set
            {
                dailyList = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsPaymentsEmpty));
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

        /// <summary>
        ///     Opens the Edit Dialog for the passed Payment
        /// </summary>
        public MvxAsyncCommand<PaymentViewModel> EditPaymentCommand => new MvxAsyncCommand<PaymentViewModel>(EditPayment);

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand => new MvxAsyncCommand<PaymentViewModel>(DeletePayment);

        private void Load()
        {
            LoadPayments(new PaymentListFilterChangedMessage(this));
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute(null);
        }

        private void LoadPayments(PaymentListFilterChangedMessage filterMessage)
        {
            var paymentQuery = crudServices.ReadManyNoTracked<PaymentViewModel>()
                .HasAccountId(AccountId);

            if (filterMessage.IsClearedFilterActive) paymentQuery = paymentQuery.AreCleared();
            if (filterMessage.IsRecurringFilterActive) paymentQuery = paymentQuery.AreRecurring();

            paymentQuery = paymentQuery.Where(x => x.Date >= filterMessage.TimeRangeStart);
            paymentQuery = paymentQuery.Where(x => x.Date <= filterMessage.TimeRangeEnd);

            var loadedPayments = new List<PaymentViewModel>(
                paymentQuery.OrderDescendingByDate());

            foreach (var payment in loadedPayments) payment.CurrentAccountId = AccountId;

            var dailyItems = DateListGroupCollection<PaymentViewModel>
                .CreateGroups(loadedPayments,
                    s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                    s => s.Date,
                    itemClickCommand: EditPaymentCommand);

            DailyList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);

            Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>(
                DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>
                    .CreateGroups(dailyItems,
                        s =>
                        {
                            var date = Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture);
                            return date.ToString("MMMM", CultureInfo.CurrentCulture) + " " + date.Year;
                        },
                        s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture)));
        }

        private async Task EditPayment(PaymentViewModel payment)
        {
            navigationService.NavigateTo(ViewModelLocator.EditPayment, payment.Id);
        }

        private async Task DeletePayment(PaymentViewModel payment)
        {
            await paymentService.DeletePayment(payment);

#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            Load();
        }
    }
}