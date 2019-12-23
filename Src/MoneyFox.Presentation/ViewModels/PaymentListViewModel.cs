using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Groups;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Presentation.ViewModels
{
    /// <summary>
    /// Representation of the payment list view.
    /// </summary>
    public class PaymentListViewModel : ViewModelBase, IPaymentListViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IBalanceCalculationService balanceCalculationService;
        private readonly Application.Common.Interfaces.IDialogService dialogService;
        private readonly INavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        private int accountId;
        private IBalanceViewModel balanceViewModel;
        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> dailyList;

        private ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> source;
        private string title;
        private IPaymentListViewActionViewModel viewActionViewModel;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PaymentListViewModel(IMediator mediator,
                                    IMapper mapper,
                                    Application.Common.Interfaces.IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    IBalanceCalculationService balanceCalculationService,
                                    INavigationService navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.balanceCalculationService = balanceCalculationService;
            this.navigationService = navigationService;

            MessengerInstance.Register<PaymentListFilterChangedMessage>(this, async message => await LoadPayments(message));
            MessengerInstance.Register<RemovePaymentMessage>(this, async message => await LoadData());
            MessengerInstance.Register<BackupRestoredMessage>(this, async message => await LoadData());
        }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);


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
                if (viewActionViewModel == value)
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
        /// Returns daily grouped related payments
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
        /// Returns the name of the account title for the current page
        /// </summary>
        public string Title
        {
            get => title;
            private set
            {
                if (title == value)
                    return;
                title = value;
                RaisePropertyChanged();
            }
        }


        private async Task Initialize()
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

            await LoadPaymentList();
        }

        private async Task LoadPaymentList()
        {
            await dialogService.ShowLoadingDialogAsync();

            await LoadData();

            await dialogService.HideLoadingDialogAsync();
        }

        private async Task LoadData()
        {
            await LoadPayments(new PaymentListFilterChangedMessage());
            //Refresh balance control with the current account
            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();
        }

        private async Task LoadPayments(PaymentListFilterChangedMessage filterMessage)
        {
            var loadedPayments = mapper.Map<List<PaymentViewModel>>(await mediator.Send(new GetPaymentsForAccountIdQuery(AccountId,
                                                                                                                         filterMessage.TimeRangeStart,
                                                                                                                         filterMessage.TimeRangeEnd)
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
                              s => s.Date);

            DailyList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);

            Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>(DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>
                    .CreateGroups(dailyItems,
                                  s =>
                                  {
                                      var date = Convert.ToDateTime(s.Key,
                                                                    CultureInfo.CurrentCulture);

                                      return $"{(date.ToString("MMMM", CultureInfo.CurrentCulture))} {date.Year}";
                                  },
                                  s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture)));
        }
    }
}
