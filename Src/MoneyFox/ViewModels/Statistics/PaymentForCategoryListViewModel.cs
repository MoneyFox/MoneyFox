using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.ViewModels.Payments;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Statistics
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

            PaymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>();
        }

        public int CategoryId { get; set; }

        public DateTime TimeRangeFrom { get; set; }

        public DateTime TimeRangeTo { get; set; }


        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public bool IsPaymentsEmpty => PaymentList != null && !PaymentList.Any();

        private ObservableCollection<DateListGroupCollection<PaymentViewModel>> paymentList;
        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> PaymentList
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

            var loadedPayments = mapper.Map<List<PaymentViewModel>>(await mediator.Send(new GetPaymentsForCategoryQuery(CategoryId, TimeRangeFrom, TimeRangeTo)));

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(loadedPayments, s => s.Date.ToString("D", CultureInfo.CurrentCulture), s => s.Date);

            PaymentList = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>(dailyItems);
        }
    }
}
