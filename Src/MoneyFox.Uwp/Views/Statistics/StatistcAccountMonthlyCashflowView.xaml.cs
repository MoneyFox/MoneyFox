using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Statistics;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatistcAccountMonthlyCashflowView
    {
        public StatisticAccountMonthlyCashflowViewModel ViewModel => (StatisticAccountMonthlyCashflowViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public StatistcAccountMonthlyCashflowView()
        {
            InitializeComponent();
        }
    }
}
