using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Uwp.Src;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Data;
using MoneyFox.Ui.Shared.Groups;
using System.Globalization;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Resources;
using GalaSoft.MvvmLight.Messaging;

namespace MoneyFox.Uwp.ViewModels
{
    /// <summary>
    /// Representation of the payment list view.
    /// </summary>
    public class PaymentListViewModel : ViewModelBase, IPaymentListViewModel
    {
        private const int DEFAULT_MONTH_BACK = -2;

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IBalanceCalculationService balanceCalculationService;
        private readonly IDialogService dialogService;
        private readonly NavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        private int accountId;
        private IBalanceViewModel balanceViewModel = null!;

        private string title = "";
        private IPaymentListViewActionViewModel viewActionViewModel;

        /// <summary>
        /// Default constructor
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

            MessengerInstance.Register<PaymentListFilterChangedMessage>(this, async message => await LoadPaymentsAsync(message));
            MessengerInstance.Register<ReloadMessage>(this, async m => await LoadDataAsync());
        }

        public RelayCommand InitializeCommand => new RelayCommand(async () => await InitializeAsync());

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadDataAsync());

        /// <summary>
        /// Deletes the passed PaymentViewModel.
        /// </summary>
        public RelayCommand<PaymentViewModel> DeletePaymentCommand => new RelayCommand<PaymentViewModel>(async (vm) => await DeletePayment(vm));

        /// <summary>
        /// Id for the current account.
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
        /// View Model for the balance subview.
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
        /// View Model for the global actions on the view.
        /// </summary>
        public IPaymentListViewActionViewModel ViewActionViewModel
        {
            get => viewActionViewModel;
            private set
            {
                if(viewActionViewModel == value)
                    return;
                viewActionViewModel = value;
                RaisePropertyChanged();
            }
        }

        private CollectionViewSource groupedPayments;

        /// <summary>
        /// Returns grouped related payments
        /// </summary>
        public CollectionViewSource GroupedPayments
        {
            get => groupedPayments;
            private set
            {
                groupedPayments = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Returns the name of the account title for the current page
        /// </summary>
        public string Title
        {
            get => title;
            private set
            {
                if(title == value)
                    return;
                title = value;
                RaisePropertyChanged();
            }
        }


        private async Task InitializeAsync()
        {
            Title = await mediator.Send(new GetAccountNameByIdQuery(accountId));

            BalanceViewModel = new PaymentListBalanceViewModel(mediator,
                                                               mapper,
                                                               balanceCalculationService,
                                                               AccountId);
            ViewActionViewModel = new PaymentListViewActionViewModel(AccountId,
                                                                     mediator,
                                                                     settingsFacade,
                                                                     dialogService,
                                                                     BalanceViewModel,
                                                                     navigationService);

            await LoadPaymentListAsync();
        }

        private async Task LoadPaymentListAsync()
        {
            await dialogService.ShowLoadingDialogAsync();

            await LoadDataAsync();

            await dialogService.HideLoadingDialogAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadPaymentsAsync(new PaymentListFilterChangedMessage { TimeRangeStart = DateTime.Now.AddYears(DEFAULT_MONTH_BACK) });
            //Refresh balance control with the current account
            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();
        }

        private async Task LoadPaymentsAsync(PaymentListFilterChangedMessage filterMessage)
        {
            var payments = mapper.Map<List<PaymentViewModel>>(await mediator.Send(new GetPaymentsForAccountIdQuery(AccountId,
                                                                                                               filterMessage.TimeRangeStart,
                                                                                                               filterMessage.TimeRangeEnd)
                                                                              {
                                                                                  IsClearedFilterActive = filterMessage.IsClearedFilterActive,
                                                                                  IsRecurringFilterActive = filterMessage.IsRecurringFilterActive
                                                                              }));

            payments.ForEach(x => x.CurrentAccountId = AccountId);

            List<DateListGroupCollection<PaymentViewModel>> group = DateListGroupCollection<PaymentViewModel>
                .CreateGroups(payments,
                                s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                                s => s.Date);

            var source = new CollectionViewSource();
            source.IsSourceGrouped = true;
            source.Source = group;

            GroupedPayments = source;
        }

        private async Task DeletePayment(PaymentViewModel payment)
        {
            if(!await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle,
                                                            Strings.DeletePaymentConfirmationMessage,
                                                            Strings.YesLabel,
                                                            Strings.NoLabel))
                return;

            var command = new DeletePaymentByIdCommand(payment.Id);

            if(payment.IsRecurring)
            {
                command.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(Strings.DeleteRecurringPaymentTitle,
                                                                                             Strings.DeleteRecurringPaymentMessage);
            }

            await mediator.Send(command);
            Messenger.Default.Send(new ReloadMessage());
        }
    }
}
