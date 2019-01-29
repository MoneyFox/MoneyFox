using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.ServiceLayer.Facades;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel" />
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private readonly ICategorySummaryDataProvider categorySummaryDataDataProvider;
        private ObservableCollection<CategoryOverviewItem> categorySummary;

        public StatisticCategorySummaryViewModel(ICategorySummaryDataProvider categorySummaryDataDataProvider,
                                                 IMvxMessenger messenger,
                                                 ISettingsFacade settingsFacade,
                                                 IMvxLogProvider logProvider,
                                                 IMvxNavigationService navigationService) : base(messenger, settingsFacade, logProvider, navigationService)
        {
            this.categorySummaryDataDataProvider = categorySummaryDataDataProvider;
            CategorySummary = new ObservableCollection<CategoryOverviewItem>();
        }

        public ObservableCollection<CategoryOverviewItem> CategorySummary
        {
            get => categorySummary;
            set
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
            CategorySummary = new ObservableCollection<CategoryOverviewItem>(await categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
        }
    }
}