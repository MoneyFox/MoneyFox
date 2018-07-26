using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Models;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace MoneyFox.Business.ViewModels.Statistic
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel" />
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
    {
        private readonly IDialogService dialogService;
        private readonly CategorySummaryDataProvider categorySummaryDataDataProvider;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="T:MoneyFox.Business.ViewModels.Statistic.StatisticCategorySummaryViewModel" /> class.
        /// </summary>
        /// <param name="categorySummaryDataDataProvider">Category summary data data provider.</param>
        /// <param name="messenger">Messenger.</param>
        /// <param name="settingsManager">Instance of a ISettingsManager</param>
        public StatisticCategorySummaryViewModel(CategorySummaryDataProvider categorySummaryDataDataProvider,
                                                 IMvxMessenger messenger,
                                                 ISettingsManager settingsManager, 
                                                 IDialogService dialogService) : base(messenger, settingsManager)
        {
            this.categorySummaryDataDataProvider = categorySummaryDataDataProvider;
            this.dialogService = dialogService;
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
            dialogService.ShowLoadingDialog();
            await Task.Run(async () => await LoadData());
            RaisePropertyChanged(nameof(HasData));
            dialogService.HideLoadingDialog();
        }

        private async Task LoadData()
        {
            var items = (await categorySummaryDataDataProvider.GetValues(StartDate, EndDate)).ToList();

            CategorySummary.Clear();
            CategorySummary.AddRange(items);
        }
    }
}