#nullable enable
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistics;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCashFlowView
    {
        public override string Header => Strings.CategoriesTitle;

        public StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)DataContext;

        public StatisticCashFlowView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCashFlowVm;
        }
    }
}