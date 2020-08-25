using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Uwp.Src;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

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
        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> payments = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();

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

        /// <summary>
        /// Returns grouped related payments
        /// </summary>
        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> GroupedPayments
        {
            get => payments;
            private set
            {
                payments = value;
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

            var loadedPayments = mapper.Map<List<PaymentViewModel>>(await mediator.Send(new GetPaymentsForAccountIdQuery(AccountId,
                                                                                                                         filterMessage.TimeRangeStart,
                                                                                                                         filterMessage.TimeRangeEnd)
                                                                                        {
                                                                                            IsClearedFilterActive = filterMessage.IsClearedFilterActive,
                                                                                            IsRecurringFilterActive = filterMessage.IsRecurringFilterActive
                                                                                        }));

            List<DateListGroupCollection<PaymentViewModel>> groupedPayments = DateListGroupCollection<PaymentViewModel>
                .CreateGroups(loadedPayments,
                                s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                                s => s.Date);

            GroupedPayments = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(groupedPayments);
        }
    }
}
