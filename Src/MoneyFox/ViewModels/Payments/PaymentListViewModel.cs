using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Extensions;
using MoneyFox.Groups;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.Views.Dialogs;
using MoneyFox.Views.Payments;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public class PaymentListViewModel : ObservableRecipient
    {
        private AccountViewModel selectedAccount = new AccountViewModel();

        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> payments =
            new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();

        private bool isRunning;

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public PaymentListViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        protected override void OnActivated()
        {
            Messenger.Register<PaymentListViewModel, ReloadMessage>(
                this,
                (r, m) => OnAppearingAsync(SelectedAccount.Id));
            Messenger.Register<PaymentListViewModel, PaymentListFilterChangedMessage>(
                this,
                (r, m) => LoadPaymentsByMessageAsync(m));
        }

        protected override void OnDeactivated()
        {
            Messenger.Unregister<ReloadMessage>(this);
            Messenger.Unregister<PaymentListFilterChangedMessage>(this);
        }

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> Payments
        {
            get => payments;
            private set
            {
                payments = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     List with the different recurrence types.
        ///     This has to have the same order as the enum
        /// </summary>
        public static List<PaymentRecurrence> RecurrenceList => new List<PaymentRecurrence>
        {
            PaymentRecurrence.Daily,
            PaymentRecurrence.DailyWithoutWeekend,
            PaymentRecurrence.Weekly,
            PaymentRecurrence.Biweekly,
            PaymentRecurrence.Monthly,
            PaymentRecurrence.Bimonthly,
            PaymentRecurrence.Quarterly,
            PaymentRecurrence.Biannually,
            PaymentRecurrence.Yearly
        };

        public async Task OnAppearingAsync(int accountId)
        {
            SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
            await LoadPaymentsByMessageAsync(new PaymentListFilterChangedMessage());
        }

        public async Task LoadPaymentsByMessageAsync(PaymentListFilterChangedMessage message)
        {
            try
            {
                if(isRunning)
                {
                    return;
                }

                isRunning = true;

                var paymentVms = mapper.Map<List<PaymentViewModel>>(
                    await mediator.Send(
                        new GetPaymentsForAccountIdQuery(
                            SelectedAccount.Id,
                            message.TimeRangeStart,
                            message.TimeRangeEnd,
                            message.IsClearedFilterActive,
                            message.IsRecurringFilterActive)));

                paymentVms.ForEach(x => x.CurrentAccountId = SelectedAccount.Id);

                List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>
                    .CreateGroups(
                        paymentVms,
                        s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                        s => s.Date);

                dailyItems.ForEach(CalculateSubBalances);

                Payments = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);
            }
            finally
            {
                isRunning = false;
            }
        }

        private void CalculateSubBalances(DateListGroupCollection<PaymentViewModel> group) =>
            group.Subtitle = string.Format(
                Strings.ExpenseAndIncomeTemplate,
                group.Where(
                         x => x.Type == PaymentType.Expense
                              || (x.Type == PaymentType.Transfer
                                  && x.ChargedAccount.Id == SelectedAccount.Id))
                     .Sum(x => x.Amount),
                group.Where(
                         x => x.Type == PaymentType.Income
                              || (x.Type == PaymentType.Transfer
                                  && x.TargetAccount != null
                                  && x.TargetAccount.Id == SelectedAccount.Id))
                     .Sum(x => x.Amount));

        public RelayCommand ShowFilterDialogCommand =>
            new RelayCommand(async () => await new FilterPopup().ShowAsync());

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(
            async () =>
                await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));

        public RelayCommand<PaymentViewModel> GoToEditPaymentCommand
            => new RelayCommand<PaymentViewModel>(
                async paymentViewModel
                    => await Shell.Current.Navigation.PushModalAsync(
                        new NavigationPage(new EditPaymentPage(paymentViewModel.Id))
                        {
                            BarBackgroundColor = Color.Transparent
                        }));
    }
}