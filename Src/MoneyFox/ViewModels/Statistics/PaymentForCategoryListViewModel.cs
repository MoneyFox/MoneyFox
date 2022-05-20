namespace MoneyFox.ViewModels.Statistics
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Threading.Tasks;
    using AutoMapper;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.Queries;
    using Groups;
    using MediatR;
    using Payments;
    using Views.Payments;

    public class PaymentForCategoryListViewModel : ObservableRecipient
    {
        private readonly IMapper mapper;

        private readonly IMediator mediator;

        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> paymentList
            = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();

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
                async paymentViewModel => await Shell.Current.Navigation.PushModalAsync(
                    new NavigationPage(new EditPaymentPage(paymentViewModel.Id)) { BarBackgroundColor = Colors.Transparent }));

        protected override void OnActivated()
        {
            Messenger.Register<PaymentForCategoryListViewModel, PaymentsForCategoryMessage>(recipient: this, handler: (r, m) => r.InitializeAsync(m));
        }

        protected override void OnDeactivated()
        {
            Messenger.Unregister<PaymentsForCategoryMessage>(this);
        }

        private async Task InitializeAsync(PaymentsForCategoryMessage receivedMessage)
        {
            var loadedPayments = mapper.Map<List<PaymentViewModel>>(
                await mediator.Send(
                    new GetPaymentsForCategoryQuery(
                        categoryId: receivedMessage.CategoryId,
                        dateRangeFrom: receivedMessage.StartDate,
                        dateRangeTo: receivedMessage.EndDate)));

            var dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(
                items: loadedPayments,
                getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
                getSortKey: s => s.Date);

            PaymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);
        }
    }

}
