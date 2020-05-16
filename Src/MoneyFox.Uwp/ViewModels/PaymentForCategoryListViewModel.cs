using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels
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

            Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>();
        }

        public int CategoryId { get; set; }

        public DateTime TimeRangeFrom { get; set; }

        public DateTime TimeRangeTo { get; set; }


        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public bool IsPaymentsEmpty => Source != null && !Source.Any();

        private ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> source;
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

        private async Task Initialize()
        {
            logger.Info($"Loading payments for category with id {CategoryId}");

            var loadedPayments = mapper.Map<List<PaymentViewModel>>(await mediator.Send(new GetPaymentsForCategoryQuery(CategoryId, TimeRangeFrom, TimeRangeTo)));

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>
               .CreateGroups(loadedPayments,
                             s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                             s => s.Date);

            Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>(
                DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>.CreateGroups(dailyItems,
                                                                                                s =>
                                                                                                {
                                                                                                    var date = Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture);
                                                                                                    return $"{date.ToString("MMMM", CultureInfo.CurrentCulture)} {date.Year}";
                                                                                                }, s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture)));
        }
    }
}
