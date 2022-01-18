#nullable enable
using MoneyFox.Core.Resources;
using MoneyFox.Uwp.ViewModels.Statistics;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticAccountMonthlyCashflowView
    {
        public StatisticAccountMonthlyCashflowViewModel ViewModel =>
            (StatisticAccountMonthlyCashflowViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public StatisticAccountMonthlyCashflowView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticAccountMonthlyCashflowVm;
        }
    }
}