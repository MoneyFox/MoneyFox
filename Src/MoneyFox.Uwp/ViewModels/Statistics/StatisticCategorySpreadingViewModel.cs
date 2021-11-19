using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Uno.Extensions;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Statistics
{
    /// <summary>
    ///     Representation of the category Spreading View
    /// </summary>
    [UsedImplicitly]
    public class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
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
            set => SetProperty(ref selectedPaymentType, value);
        }

        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

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

        public AsyncRelayCommand LoadDataCommand => new AsyncRelayCommand(async () => await LoadAsync());

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
            Series.Clear();
            Series.AddRange(pieSeries);
        }
    }
}