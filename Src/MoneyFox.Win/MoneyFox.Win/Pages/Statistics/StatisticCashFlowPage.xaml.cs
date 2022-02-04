using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics;

namespace MoneyFox.Win.Pages.Statistics
{
    public partial class StatisticCashFlowPage : BasePage
    {
        public StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)DataContext;

        public override string Header => Strings.CashFlowStatisticTitle;

        public StatisticCashFlowPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCashFlowVm;
        }
    }
}