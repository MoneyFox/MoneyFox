using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Application.Statistics.Queries.GetCategorySummary;
using MoneyFox.Uwp.Groups;
using MoneyFox.Uwp.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Statistics;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel"/>
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<CategoryOverviewViewModel> categorySummary =
            new ObservableCollection<CategoryOverviewViewModel>();

        private readonly IMapper mapper;

        public StatisticCategorySummaryViewModel(IMediator mediator,
            IMapper mapper)
            : base(mediator)
        {
            this.mapper = mapper;
        }

        private IncomeExpenseBalanceViewModel incomeExpenseBalance = new IncomeExpenseBalanceViewModel();

        public IncomeExpenseBalanceViewModel IncomeExpenseBalance
        {
            get => incomeExpenseBalance;
            set
            {
                if(incomeExpenseBalance == value)
                {
                    return;
                }

                incomeExpenseBalance = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryOverviewViewModel> CategorySummary
        {
            get => categorySummary;
            private set
            {
                if(categorySummary == value)
                {
                    return;
                }

                categorySummary = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasData));
            }
        }

        /// <inheritdoc/>
        public bool HasData => CategorySummary.Any();

        public RelayCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand =>
            new RelayCommand<CategoryOverviewViewModel>(async c => await SummaryEntrySelectedAsync(c));

        private CategoryOverviewViewModel? selectedOverviewItem;

        public CategoryOverviewViewModel? SelectedOverviewItem
        {
            get => selectedOverviewItem;
            set
            {
                selectedOverviewItem = value;
                OnPropertyChanged();
            }
        }

        private async Task SummaryEntrySelectedAsync(CategoryOverviewViewModel summaryItem)
        {
            logger.Info($"Loading payments for category with id {summaryItem.CategoryId}");

            var loadedPayments = mapper.Map<List<PaymentViewModel>>(
                await Mediator.Send(new GetPaymentsForCategoryQuery(summaryItem.CategoryId, StartDate, EndDate)));

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>
                .CreateGroups(loadedPayments,
                    s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                    s => s.Date);

            summaryItem.Source.Clear();

            DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>.CreateGroups(dailyItems,
                    s =>
                    {
                        var date = Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture);
                        return $"{date.ToString("MMMM", CultureInfo.CurrentCulture)} {date.Year}";
                    }, s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture))
                .ForEach(summaryItem.Source.Add);

            SelectedOverviewItem = summaryItem;
        }

        /// <summary>
        /// Overrides the load method to load the category summary data.
        /// </summary>
        protected override async Task LoadAsync()
        {
            CategorySummaryModel categorySummaryModel =
                await Mediator.Send(new GetCategorySummaryQuery {EndDate = EndDate, StartDate = StartDate});

            CategorySummary.Clear();
            categorySummaryModel.CategoryOverviewItems
                .Select(x => new CategoryOverviewViewModel
                {
                    CategoryId = x.CategoryId,
                    Value = x.Value,
                    Average = x.Average,
                    Label = x.Label,
                    Percentage = x.Percentage
                })
                .ToList()
                .ForEach(CategorySummary.Add);

            IncomeExpenseBalance.TotalEarned = categorySummaryModel.TotalEarned;
            IncomeExpenseBalance.TotalSpent = categorySummaryModel.TotalSpent;
        }
    }
}