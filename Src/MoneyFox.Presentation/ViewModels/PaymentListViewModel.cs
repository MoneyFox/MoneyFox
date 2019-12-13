using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Backup;
using MoneyFox.Application.Facades;
using MoneyFox.Application.Messages;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Groups;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels.Interfaces;
using Xamarin.Forms;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    /// <summary>
    ///     Representation of the payment list view.
    /// </summary>
    public class PaymentListViewModel : BaseViewModel, IPaymentListViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
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
        public PaymentListViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IPaymentService paymentService,
                                    IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    IBalanceCalculationService balanceCalculationService,
                                    IBackupService backupService,
                                    INavigationService navigationService,
                                    IMessenger messenger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.paymentService = paymentService;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.balanceCalculationService = balanceCalculationService;
            this.backupService = backupService;
            this.navigationService = navigationService;

            MessengerInstance = messenger;

            MessengerInstance.Register<PaymentListFilterChangedMessage>(this, async message => { await LoadPayments(message); });
            MessengerInstance.Register<RemovePaymentMessage>(this, message => { RemovePayment(message); });
            MessengerInstance.Register<BackupRestoredMessage>(this, async message => await LoadData());
        }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

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
        public Command<PaymentViewModel> EditPaymentCommand => new Command<PaymentViewModel>(EditPayment);

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public Command<PaymentViewModel> DeletePaymentCommand => new Command<PaymentViewModel>(DeletePayment);

        private async Task Initialize()
        {
            Title = await mediator.Send(new GetAccountNameByIdQuery(accountId));

            BalanceViewModel = new PaymentListBalanceViewModel(mediator, mapper, balanceCalculationService, AccountId);
            ViewActionViewModel = new PaymentListViewActionViewModel(AccountId,
                                                                     mediator,
                                                                     settingsFacade,
                                                                     dialogService,
                                                                     BalanceViewModel,
                                                                     navigationService);

            await LoadPaymentList();
        }

        private async Task LoadPaymentList()
        {
            await dialogService.ShowLoadingDialogAsync();

            await LoadData();

            await dialogService.HideLoadingDialogAsync();
        }

        private async Task LoadData() {
            await LoadPayments(new PaymentListFilterChangedMessage());
            //Refresh balance control with the current account
            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();
        }
        private void RemovePayment(RemovePaymentMessage message)
        {
            foreach (var dateGroup in DailyList)
            {
                dateGroup.RemoveAll(y => y.Id == message.PaymentId);
            }

            foreach (var monthList in Source)
            {
                foreach (var dailyGroup in monthList)
                {
                    dailyGroup.RemoveAll(y => y.Id == message.PaymentId);
                }
            }
        }

        private async Task LoadPayments(PaymentListFilterChangedMessage filterMessage)
        {
            var loadedPayments = mapper.Map<List<PaymentViewModel>>(
                await mediator.Send(new GetPaymentsForAccountIdQuery(AccountId, filterMessage.TimeRangeStart, filterMessage.TimeRangeEnd)
                {
                    IsClearedFilterActive = filterMessage.IsClearedFilterActive,
                    IsRecurringFilterActive = filterMessage.IsRecurringFilterActive
                }));

            foreach (PaymentViewModel payment in loadedPayments)
            {
                payment.CurrentAccountId = AccountId;
            }

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>
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
                                      DateTime date = Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture);

                                      return date.ToString("MMMM", CultureInfo.CurrentCulture) + " " + date.Year;
                                  },
                                  s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture)));
        }

        private void EditPayment(PaymentViewModel payment)
        {
            navigationService.NavigateTo(ViewModelLocator.EditPayment, payment.Id);
        }

        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "Acts as EventHandler")]
        private async void DeletePayment(PaymentViewModel payment)
        {
            await paymentService.DeletePayment(payment);

#pragma warning disable 4014
            backupService.UploadBackupAsync();
#pragma warning restore 4014
            await LoadPaymentList();
        }
    }
}
