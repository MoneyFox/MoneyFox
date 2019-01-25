using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.Foundation.Models;
using MoneyFox.ServiceLayer.Facades;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel" />
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private readonly ICategorySummaryDataProvider categorySummaryDataDataProvider;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="T:MoneyFox.ServiceLayer.ViewModels.Statistic.StatisticCategorySummaryViewModel" /> class.
        /// </summary>
        public StatisticCategorySummaryViewModel(ICategorySummaryDataProvider categorySummaryDataDataProvider,
                                                 IMvxMessenger messenger,
                                                 ISettingsFacade settingsFacade,
                                                 IMvxLogProvider logProvider,
                                                 IMvxNavigationService navigationService) : base(messenger, settingsFacade, logProvider, navigationService)
        {
            this.categorySummaryDataDataProvider = categorySummaryDataDataProvider;
            CategorySummary = new MvxObservableCollection<StatisticItem>();
        }

        /// <inheritdoc />
        public MvxObservableCollection<StatisticItem> CategorySummary { get; set; }

        /// <inheritdoc />
        public bool HasData => CategorySummary.Any();

        /// <summary>
        ///     Overrides the load method to load the category summary data.
        /// </summary>
        protected override async Task Load()
        {
            await Task.Run(async () => await LoadData());
            await RaisePropertyChanged(nameof(HasData));
        }

        private async Task LoadData()
        {
            var items = (await categorySummaryDataDataProvider.GetValues(StartDate, EndDate)).ToList();

            CategorySummary.Clear();
            CategorySummary.AddRange(items);
        }
    }
}