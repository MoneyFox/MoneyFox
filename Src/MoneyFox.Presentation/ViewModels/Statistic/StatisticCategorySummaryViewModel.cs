using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.Presentation.Facades;
using MoneyFox.ServiceLayer.Facades;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel" />
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private readonly ICategorySummaryDataProvider categorySummaryDataDataProvider;
        private ObservableCollection<CategoryOverviewViewModel> categorySummary;

        public StatisticCategorySummaryViewModel(ICategorySummaryDataProvider categorySummaryDataDataProvider,
                                                 ISettingsFacade settingsFacade)
            : base(settingsFacade)
        {
            this.categorySummaryDataDataProvider = categorySummaryDataDataProvider;
            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>();
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
            var summaryItems = await categorySummaryDataDataProvider.GetValues(StartDate, EndDate);

            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>(
                summaryItems.Select(x => new CategoryOverviewViewModel
                {
                    Value = x.Value,
                    Average = x.Average,
                    Label = x.Label,
                    Percentage = x.Percentage
                }));
        }
    }
}