using AutoMapper;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Application.Resources;
using MoneyFox.Common;
using MoneyFox.Domain;
using MoneyFox.Extensions;
using MoneyFox.Groups;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Categories;
using MoneyFox.Views.Payments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public class PaymentListViewModel : BaseViewModel
    {
        private AccountViewModel selectedAccount = new AccountViewModel();
        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> payments = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();
        private PaymentListFilterChangedMessage lastMessage = new PaymentListFilterChangedMessage();

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public PaymentListViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            MessengerInstance.Register<ReloadMessage>(this, async (m) => await OnAppearingAsync(SelectedAccount.Id));
            MessengerInstance.Register<PaymentListFilterChangedMessage>(this, async message =>
            {
                lastMessage = message;
                await LoadPaymentsByMessageAsync();
            });
        }

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> Payments
        {
            get => payments;
            set
            {
                payments = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     List with the different recurrence types.
        ///     This has to have the same order as the enum
        /// </summary>
        public List<PaymentRecurrence> RecurrenceList => new List<PaymentRecurrence>
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
            await LoadPaymentsByMessageAsync();
        }

        private async Task LoadPaymentsByMessageAsync()
        {
            var paymentVms = mapper.Map<List<PaymentViewModel>>(
                await mediator.Send(new GetPaymentsForAccountIdQuery(SelectedAccount.Id, lastMessage.TimeRangeStart, lastMessage.TimeRangeEnd)));

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>
               .CreateGroups(paymentVms,
                             s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                             s => s.Date);

            dailyItems.ForEach(CalculateSubBalances);

            Payments = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);
        }

        private void CalculateSubBalances(DateListGroupCollection<PaymentViewModel> group)
        {
            group.Subtitle = string.Format(Strings.IncomeAndExpenseTemplate,
                group.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount),
                group.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount));
        }

        public RelayCommand ShowFilterDialogCommand => new RelayCommand(async () => await new FilterPopup().ShowAsync());

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));

        public RelayCommand<PaymentViewModel> GoToEditPaymentCommand
            => new RelayCommand<PaymentViewModel>(async (paymentViewModel)
                => await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditPaymentPage(paymentViewModel.Id)) { BarBackgroundColor = Color.Transparent, BarTextColor = ResourceHelper.GetCurrentTextColor() }));
    }
}
