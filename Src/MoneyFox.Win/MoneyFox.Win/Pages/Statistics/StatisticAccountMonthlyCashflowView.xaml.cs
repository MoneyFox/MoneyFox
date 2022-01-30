using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics;

namespace MoneyFox.Win.Pages.Statistics
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