using MoneyFox.Presentation.ViewModels.Statistic;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticCashFlowPage
    {
        private StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel) BindingContext;

        public StatisticCashFlowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCashFlowViewModel;

            ViewModel.LoadedCommand.Execute(null);
        }
    }
}
