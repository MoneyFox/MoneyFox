using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Uwp.Src;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Domain;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;

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
        private ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> source
            = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>();

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

        public AsyncCommand InitializeCommand => new AsyncCommand(InitializeAsync);

        public AsyncCommand LoadDataCommand => new AsyncCommand(LoadDataAsync);


        /// <summary>
        /// Indicator if there are payments or not.
        /// </summary>
        public bool IsPaymentsEmpty => Source != null && !Source.Any();

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

            foreach(PaymentViewModel payment in loadedPayments)
            {
                payment.CurrentAccountId = AccountId;
            }

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>
               .CreateGroups(loadedPayments,
                             s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                             s => s.Date);

            foreach(var dailyGroup in dailyItems)
            {
                var monthlyIncome = dailyGroup.Where(payment => payment.Type == PaymentType.Income
                                                                || (payment.Type == PaymentType.Transfer && payment.TargetAccount.Id == AccountId))
                                              .Sum(x => x.Amount);

                var monthlyExpenses = dailyGroup.Where(payment => payment.Type == PaymentType.Expense
                                                                  || (payment.Type == PaymentType.Transfer && payment.ChargedAccount.Id == AccountId))
                                                .Sum(x => x.Amount);

                dailyGroup.Subtitle = $"+{monthlyIncome.ToString("C", CultureHelper.CurrentCulture)} / -{monthlyExpenses.ToString("C", CultureHelper.CurrentCulture)}";
            }

            Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>(
                DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>.CreateGroups(dailyItems,
                                                                                                s =>
                                                                                                {
                                                                                                    var date = Convert.ToDateTime(s.Key,CultureInfo.CurrentCulture);
                                                                                                    return $"{date.ToString("MMMM", CultureInfo.CurrentCulture)} {date.Year}";
                                                                                                },s => Convert.ToDateTime(s.Key,CultureInfo.CurrentCulture)));
        }
    }
}
