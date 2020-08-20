using MoneyFox.Presentation.ViewModels.Statistic;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticCategorySummaryPage
    {
        private StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel) BindingContext;

        public StatisticCategorySummaryPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySummaryViewModel;
            ViewModel.LoadedCommand.Execute(null);
        }
    }
}
