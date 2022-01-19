using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.Queries.Payments.GetPaymentsForCategory;
using MoneyFox.Groups;
using MoneyFox.ViewModels.Payments;
using MoneyFox.Views.Payments;
using NLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Statistics
{
    public class PaymentForCategoryListViewModel : ObservableRecipient
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper mapper;

        private readonly IMediator mediator;

        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> paymentList =
            new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();

        public PaymentForCategoryListViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            PaymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();

            IsActive = true;
        }

        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> PaymentList
        {
            get => paymentList;
            private set
            {
                paymentList = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<PaymentViewModel> GoToEditPaymentCommand
            => new RelayCommand<PaymentViewModel>(
                async paymentViewModel
                    => await Shell.Current.Navigation.PushModalAsync(
                        new NavigationPage(new EditPaymentPage(paymentViewModel.Id))
                        {
                            BarBackgroundColor = Color.Transparent
                        }));

        protected override void OnActivated()
            => Messenger.Register<PaymentForCategoryListViewModel, PaymentsForCategoryMessage>(
                this,
                (r, m) => r.InitializeAsync(m));

        protected override void OnDeactivated() => Messenger.Unregister<PaymentsForCategoryMessage>(this);

        private async Task InitializeAsync(PaymentsForCategoryMessage receivedMessage)
        {
            logger.Info($"Loading payments for category with id {receivedMessage.CategoryId}");

            var loadedPayments = mapper.Map<List<PaymentViewModel>>(
                await mediator.Send(
                    new GetPaymentsForCategoryQuery(
                        receivedMessage.CategoryId,
                        receivedMessage.StartDate,
                        receivedMessage.EndDate)));

            List<DateListGroupCollection<PaymentViewModel>> dailyItems
                = DateListGroupCollection<PaymentViewModel>.CreateGroups(
                    loadedPayments,
                    s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                    s => s.Date);

            PaymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);
        }
    }
}