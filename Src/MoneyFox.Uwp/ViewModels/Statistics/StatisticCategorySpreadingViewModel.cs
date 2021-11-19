using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Statistics
{
    /// <summary>
    ///     Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
        private ObservableCollection<ISeries> series = new ObservableCollection<ISeries>();
        private readonly ISettingsFacade settingsFacade;
        private PaymentType selectedPaymentType;

        public StatisticCategorySpreadingViewModel(IMediator mediator, ISettingsFacade settingsFacade) : base(mediator)
        {
            this.settingsFacade = settingsFacade;
        }

        public List<PaymentType> PaymentTypes => new List<PaymentType> {PaymentType.Expense, PaymentType.Income};

        public PaymentType SelectedPaymentType
        {
            get => selectedPaymentType;
            set
            {
                if(selectedPaymentType == value)
                {
                    return;
                }

                selectedPaymentType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Statistic items to display.
        /// </summary>
        public ObservableCollection<ISeries> Series
        {
            get => series;
            private set
            {
                if(series == value)
                {
                    return;
                }

                series = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Amount of categories to show. All Payments not fitting in here will go to the other category
        /// </summary>
        public int NumberOfCategoriesToShow
        {
            get => settingsFacade.CategorySpreadingNumber;
            set
            {
                if(settingsFacade.CategorySpreadingNumber == value)
                {
                    return;
                }

                settingsFacade.CategorySpreadingNumber = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

        /// <summary>
        ///     Set a custom CategorySpreadingModel with the set Start and End date
        /// </summary>
        protected override async Task LoadAsync()
        {
            IEnumerable<StatisticEntry> statisticEntries = await Mediator.Send(
                new GetCategorySpreadingQuery(
                    StartDate,
                    EndDate,
                    SelectedPaymentType,
                    NumberOfCategoriesToShow));

            var pieSeries = statisticEntries.Select(x =>
                new PieSeries<decimal>
                {
                    Name = x.Label,
                    TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                    DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                    Values = new List<decimal> {x.Value},
                    InnerRadius = 150
                });
            Series = new ObservableCollection<ISeries>(pieSeries);
        }
    }
}