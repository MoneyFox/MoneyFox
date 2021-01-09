using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.ViewModels.Statistics;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCashFlowView
    {
        public StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)DataContext;

        public override string Header => Strings.CashFlowStatisticTitle;

        public StatisticCashFlowView()
        {
            InitializeComponent();
        }
    }
}
