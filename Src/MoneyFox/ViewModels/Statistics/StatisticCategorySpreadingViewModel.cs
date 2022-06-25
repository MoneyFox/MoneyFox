namespace MoneyFox.ViewModels.Statistics
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Core.ApplicationCore.Queries.Statistics;
    using Core.Common.Extensions;
    using LiveChartsCore;
    using LiveChartsCore.SkiaSharpView;
    using MediatR;

    internal sealed class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
        private PaymentType selectedPaymentType;

        public StatisticCategorySpreadingViewModel(IMediator mediator) : base(mediator) { }

        public List<PaymentType> PaymentTypes => new List<PaymentType> { PaymentType.Expense, PaymentType.Income };

        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

        public PaymentType SelectedPaymentType
        {
            get => selectedPaymentType;

            set
            {
                if (selectedPaymentType == value)
                {
                    return;
                }

                selectedPaymentType = value;
                OnPropertyChanged();
                LoadDataCommand.Execute(null);
            }
        }

        public AsyncRelayCommand LoadDataCommand => new AsyncRelayCommand(LoadAsync);

        protected override async Task LoadAsync()
        {
            var statisticEntries = await Mediator.Send(new GetCategorySpreadingQuery(startDate: StartDate, endDate: EndDate, paymentType: SelectedPaymentType));
            var pieSeries = statisticEntries.Select(
                x => new PieSeries<decimal>
                {
                    Name = x.Label,
                    TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                    DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                    Values = new List<decimal> { x.Value },
                    InnerRadius = 150
                });

            Series.Clear();
            Series.AddRange(pieSeries);
        }
    }

}
