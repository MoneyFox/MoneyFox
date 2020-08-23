using MoneyFox.ViewModels.Statistics;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticCategorySpreadingPage
    {
        private StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel) BindingContext;

        public StatisticCategorySpreadingPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySpreadingViewModel;

            ViewModel.LoadedCommand.Execute(null);
        }
    }
}
