using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Common;
using MoneyFox.Ui.Shared.Groups;
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
    public class PaymentForCategoryListViewModel : ViewModelBase
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        private PaymentsForCategoryMessage? receivedMessage;

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public PaymentForCategoryListViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            PaymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();
            MessengerInstance.Register<PaymentsForCategoryMessage>(this, async m =>
            {
                receivedMessage = m;
                await InitializeAsync();
            });
        }

        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> paymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();
        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> PaymentList
        {
            get => paymentList;
            private set
            {
                paymentList = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<PaymentViewModel> GoToEditPaymentCommand
            => new RelayCommand<PaymentViewModel>(async (paymentViewModel)
                => await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditPaymentPage(paymentViewModel.Id)) { BarBackgroundColor = Color.Transparent, BarTextColor = ResourceHelper.GetCurrentTextColor() }));

        private async Task InitializeAsync()
        {
            if(receivedMessage == null)
            {
                logger.Error("No message received");
                return;
            }

            logger.Info($"Loading payments for category with id {receivedMessage.CategoryId}");

            var loadedPayments = mapper.Map<List<PaymentViewModel>>(await mediator.Send(
                new GetPaymentsForCategoryQuery(receivedMessage.CategoryId, receivedMessage.StartDate, receivedMessage.EndDate)));

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(loadedPayments, s => s.Date.ToString("D", CultureInfo.CurrentCulture), s => s.Date);

            PaymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);
        }
    }
}
