using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.Groups;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Payments
{
    /// <summary>
    ///     Representation of the payment list view.
    /// </summary>
    public class PaymentListViewModel : ViewModelBase
    {
        private const int DEFAULT_MONTH_BACK = -2;
        private readonly IBalanceCalculationService balanceCalculationService;
        private readonly IDialogService dialogService;
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();
        private readonly IMapper mapper;

        private readonly IMediator mediator;
        private readonly NavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        private int accountId;
        private IBalanceViewModel balanceViewModel = null!;

        private PaymentListFilterChangedMessage filterMessage =
            new PaymentListFilterChangedMessage {TimeRangeStart = DateTime.Now.AddYears(DEFAULT_MONTH_BACK)};

        private CollectionViewSource? groupedPayments;
        private bool isBusy = true;

        private string title = "";
        private IPaymentListViewActionViewModel? viewActionViewModel;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public PaymentListViewModel(IMediator mediator,
            IMapper mapper,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBalanceCalculationService balanceCalculationService,
            NavigationService navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.balanceCalculationService = balanceCalculationService;
            this.navigationService = navigationService;
        }

        public RelayCommand InitializeCommand => new RelayCommand(async () => await InitializeAsync());

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadDataAsync());

        public RelayCommand<PaymentViewModel> EditPaymentCommand
            => new RelayCommand<PaymentViewModel>(vm => navigationService.Navigate<EditPaymentViewModel>(vm));

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public RelayCommand<PaymentViewModel> DeletePaymentCommand
            => new RelayCommand<PaymentViewModel>(async vm => await DeletePaymentAsync(vm));

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
        public IPaymentListViewActionViewModel? ViewActionViewModel
        {
            get => viewActionViewModel;
            private set
            {
                if(viewActionViewModel == value)
                {
                    return;
                }

                viewActionViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns grouped related payments
        /// </summary>
        public CollectionViewSource? GroupedPayments
        {
            get => groupedPayments;
            private set
            {
                groupedPayments = value;
                RaisePropertyChanged();
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
                if(title == value)
                {
                    return;
                }

                title = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicates if the view is loading.
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            private set
            {
                if(isBusy == value)
                {
                    return;
                }

                isBusy = value;
                RaisePropertyChanged();
            }
        }

        public void Subscribe()
        {
            MessengerInstance.Register<PaymentListFilterChangedMessage>(
                this,
                async message =>
                {
                    filterMessage = message;
                    await LoadDataAsync();
                });
            MessengerInstance.Register<ReloadMessage>(this, async m => await LoadDataAsync());
        }

        public void Unsubscribe() => MessengerInstance.Unregister(this);

        private async Task InitializeAsync()
        {
            Title = await mediator.Send(new GetAccountNameByIdQuery(accountId));

            BalanceViewModel = new PaymentListBalanceViewModel(
                mediator,
                mapper,
                balanceCalculationService,
                AccountId);
            ViewActionViewModel = new PaymentListViewActionViewModel(
                AccountId,
                mediator,
                settingsFacade,
                dialogService,
                BalanceViewModel,
                navigationService);

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if(AccountId == 0)
            {
                return;
            }

            try
            {
                await dialogService.ShowLoadingDialogAsync();
                await LoadPaymentsAsync();

                //Refresh balance control with the current account
                await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();
            }
            finally
            {
                await dialogService.HideLoadingDialogAsync();
            }
        }

        private async Task LoadPaymentsAsync()
        {
            var payments = mapper.Map<List<PaymentViewModel>>(
                await mediator.Send(
                    new GetPaymentsForAccountIdQuery(
                        AccountId,
                        filterMessage.TimeRangeStart,
                        filterMessage.TimeRangeEnd)
                    {
                        IsClearedFilterActive = filterMessage.IsClearedFilterActive,
                        IsRecurringFilterActive = filterMessage.IsRecurringFilterActive
                    }));

            payments.ForEach(x => x.CurrentAccountId = AccountId);

            var source = new CollectionViewSource {IsSourceGrouped = filterMessage.IsGrouped};

            if(filterMessage.IsGrouped)
            {
                var group = DateListGroupCollection<PaymentViewModel>
                    .CreateGroups(
                        payments,
                        s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                        s => s.Date);
                source.Source = group;
            }
            else
            {
                source.Source = payments;
            }

            GroupedPayments = source;
        }

        private async Task DeletePaymentAsync(PaymentViewModel payment)
        {
            if(!await dialogService.ShowConfirmMessageAsync(
                   Strings.DeleteTitle,
                   Strings.DeletePaymentConfirmationMessage,
                   Strings.YesLabel,
                   Strings.NoLabel))
            {
                return;
            }

            try
            {
                var command = new DeletePaymentByIdCommand(payment.Id);

                if(payment.IsRecurring)
                {
                    command.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(
                        Strings.DeleteRecurringPaymentTitle,
                        Strings.DeleteRecurringPaymentMessage);
                }

                await mediator.Send(command);
                Messenger.Default.Send(new ReloadMessage());
            }
            catch(Exception ex)
            {
                logManager.Error(ex, "Error during deleting payment.");
                await dialogService.ShowMessageAsync(Strings.GeneralErrorTitle, Strings.UnknownErrorMessage);
            }
        }
    }
}