using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics;

namespace MoneyFox.Win.Pages.Statistics
{
    public sealed partial class StatisticAccountMonthlyCashFlowPage
    {
        public StatisticAccountMonthlyCashflowViewModel ViewModel =>
            (StatisticAccountMonthlyCashflowViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public StatisticAccountMonthlyCashFlowPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticAccountMonthlyCashflowVm;
        }
    }
}