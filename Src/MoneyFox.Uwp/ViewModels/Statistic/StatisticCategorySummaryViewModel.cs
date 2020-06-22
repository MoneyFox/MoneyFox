using AutoMapper;
using MediatR;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Application.Statistics.Queries.GetCategorySummary;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel"/>
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<CategoryOverviewViewModel> categorySummary;

        private readonly IMapper mapper;

        public StatisticCategorySummaryViewModel(IMediator mediator,
                                                 ISettingsFacade settingsFacade,
                                                 IMapper mapper) : base(mediator, settingsFacade)
        {
            this.mapper = mapper;

            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>();
            IncomeExpenseBalance = new IncomeExpenseBalanceViewModel();
        }

        private IncomeExpenseBalanceViewModel incomeExpenseBalance;

        public IncomeExpenseBalanceViewModel IncomeExpenseBalance
        {
            get => incomeExpenseBalance;
            set
            {
                if(incomeExpenseBalance == value)
                    return;
                incomeExpenseBalance = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CategoryOverviewViewModel> CategorySummary
        {
            get => categorySummary;
            private set
            {
                if(categorySummary == value) return;
                categorySummary = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasData));
            }
        }

        /// <inheritdoc/>
        public bool HasData => CategorySummary.Any();

        public AsyncCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand => new AsyncCommand<CategoryOverviewViewModel>(SummaryEntrySelected);

        private async Task SummaryEntrySelected(CategoryOverviewViewModel summaryItem)
        {
            logger.Info($"Loading payments for category with id {summaryItem.CategoryId}");

            var loadedPayments = mapper.Map<List<PaymentViewModel>>(await Mediator.Send(new GetPaymentsForCategoryQuery(summaryItem.CategoryId, StartDate, EndDate)));

            List<DateListGroupCollection<PaymentViewModel>> dailyItems = DateListGroupCollection<PaymentViewModel>
               .CreateGroups(loadedPayments,
                             s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                             s => s.Date);

            summaryItem.Source = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>(
                DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>.CreateGroups(dailyItems,
                                                                                                s =>
                                                                                                {
                                                                                                    var date = Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture);
                                                                                                    return $"{date.ToString("MMMM", CultureInfo.CurrentCulture)} {date.Year}";
                                                                                                }, s => Convert.ToDateTime(s.Key, CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Overrides the load method to load the category summary data.
        /// </summary>
        protected override async Task LoadAsync()
        {
            CategorySummaryModel categorySummaryModel = await Mediator.Send(new GetCategorySummaryQuery { EndDate = EndDate, StartDate = StartDate });

            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>(categorySummaryModel
                                                                                     .CategoryOverviewItems
                                                                                     .Select(x => new CategoryOverviewViewModel
                                                                                     {
                                                                                         CategoryId = x.CategoryId,
                                                                                         Value = x.Value,
                                                                                         Average = x.Average,
                                                                                         Label = x.Label,
                                                                                         Percentage = x.Percentage
                                                                                     }));

            IncomeExpenseBalance = new IncomeExpenseBalanceViewModel
            {
                TotalEarned = categorySummaryModel.TotalEarned,
                TotalSpent = categorySummaryModel.TotalSpent
            };
        }
    }
}
