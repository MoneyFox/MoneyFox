using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Statistics.Queries.GetCategorySummary;
using MoneyFox.Presentation.Facades;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel" />
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private ObservableCollection<CategoryOverviewViewModel> categorySummary;

        public StatisticCategorySummaryViewModel(IMediator mediator,
                                                 ISettingsFacade settingsFacade)
            : base(mediator, settingsFacade)
        {
            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>();
            IncomeExpenseBalance = new IncomeExpenseBalanceViewModel();
        }

        private IncomeExpenseBalanceViewModel incomeExpenseBalance;

        public IncomeExpenseBalanceViewModel IncomeExpenseBalance
        {
            get => incomeExpenseBalance;
            set
            {
                if (incomeExpenseBalance == value) return;
                incomeExpenseBalance = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CategoryOverviewViewModel> CategorySummary
        {
            get => categorySummary;
            private set
            {
                categorySummary = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasData));
            }
        }

        /// <inheritdoc />
        public bool HasData => CategorySummary.Any();

        /// <summary>
        ///     Overrides the load method to load the category summary data.
        /// </summary>
        protected override async Task Load()
        {
            CategorySummaryModel categorySummaryModel = await Mediator.Send(new GetCategorySummaryQuery {EndDate = EndDate, StartDate = StartDate});

            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>(
                categorySummaryModel.CategoryOverviewItems.Select(x => new CategoryOverviewViewModel
                {
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
