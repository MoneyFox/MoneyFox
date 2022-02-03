using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics;

namespace MoneyFox.Win.Pages.Statistics
{
    public partial class StatisticCashFlowView : BaseView
    {
        public StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)DataContext;

        public override string Header => Strings.CashFlowStatisticTitle;

        public StatisticCashFlowView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCashFlowVm;
        }
    }
}