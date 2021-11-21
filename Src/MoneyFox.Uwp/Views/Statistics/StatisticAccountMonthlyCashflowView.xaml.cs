#nullable enable
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistics;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticAccountMonthlyCashflowView
    {
        public StatisticAccountMonthlyCashFlowViewModel ViewModel =>
            (StatisticAccountMonthlyCashFlowViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public StatisticAccountMonthlyCashflowView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticAccountMonthlyCashFlowVm;
        }
    }
}