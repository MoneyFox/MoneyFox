using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Ui.Shared.Commands;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Presentation.ViewModels
{
    public class PaymentForCategoryListViewModel : ViewModelBase
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public PaymentForCategoryListViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            PaymentList = new ObservableCollection<PaymentViewModel>();
        }

        public int CategoryId { get; set; }

        public DateTime TimeRangeFrom { get; set; }

        public DateTime TimeRangeTo { get; set; }


        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public ObservableCollection<PaymentViewModel> PaymentList { get; set; }

        private async Task Initialize()
        {
            logger.Info($"Loading payments for category with id {CategoryId}");

            var loadedPayments = await mediator.Send(new GetPaymentsForCategoryQuery(CategoryId, TimeRangeFrom, TimeRangeTo));
            loadedPayments.ForEach(x => PaymentList.Add(mapper.Map<PaymentViewModel>(x)));
        }
    }
}
