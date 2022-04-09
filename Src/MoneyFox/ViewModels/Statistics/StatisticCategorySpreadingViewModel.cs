namespace MoneyFox.ViewModels.Statistics
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Core.Aggregates.Payments;
    using Core.Queries.Statistics;
    using MediatR;
    using Microcharts;
    using SkiaSharp;
    using Views.Statistics;
    using Xamarin.Essentials;

    /// <summary>
    ///     Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
        private DonutChart chart = new DonutChart();
        private PaymentType selectedPaymentType;

        public StatisticCategorySpreadingViewModel(IMediator mediator) : base(mediator) { }

        public List<PaymentType> PaymentTypes => new List<PaymentType> { PaymentType.Expense, PaymentType.Income };

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

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public DonutChart Chart
        {
            get => chart;

            set
            {
                if (chart == value)
                {
                    return;
                }

                chart = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

        /// <summary>
        ///     Set a custom CategorySpreadingModel with the set Start and End date
        /// </summary>
        protected override async Task LoadAsync()
        {
            var statisticItems = new ObservableCollection<StatisticEntry>(
                await Mediator.Send(new GetCategorySpreadingQuery(startDate: StartDate, endDate: EndDate, paymentType: SelectedPaymentType)));

            var microChartItems = statisticItems.Select(
                    x => new ChartEntry((float)x.Value)
                    {
                        Label = x.Label,
                        ValueLabel = x.ValueLabel,
                        Color = SKColor.Parse(x.Color),
                        ValueLabelColor = SKColor.Parse(x.Color)
                    })
                .ToList();

            Chart = new DonutChart
            {
                Entries = microChartItems,
                BackgroundColor = new SKColor(ChartOptions.BackgroundColor.ToUInt()),
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = SKTypeface.FromFamilyName(ChartOptions.TypeFace)
            };
        }
    }

}
