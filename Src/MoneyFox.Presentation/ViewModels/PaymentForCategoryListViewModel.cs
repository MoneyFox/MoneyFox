using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Ui.Shared.Commands;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        public bool IsPaymentsEmpty => PaymentList != null && !PaymentList.Any();

        private ObservableCollection<PaymentViewModel> paymentList;
        public ObservableCollection<PaymentViewModel> PaymentList
        {
            get => paymentList;
            private set
            {
                paymentList = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsPaymentsEmpty));
            }
        }

        private async Task Initialize()
        {
            logger.Info($"Loading payments for category with id {CategoryId}");

            var loadedPayments = await mediator.Send(new GetPaymentsForCategoryQuery(CategoryId, TimeRangeFrom, TimeRangeTo));
            loadedPayments.ForEach(x => PaymentList.Add(mapper.Map<PaymentViewModel>(x)));
        }
    }
}
