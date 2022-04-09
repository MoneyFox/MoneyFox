namespace MoneyFox.Win.Pages.Statistics;

using Core.Resources;
using ViewModels.Statistics;

public sealed partial class StatisticAccountMonthlyCashFlowPage
{
    public StatisticAccountMonthlyCashFlowPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.StatisticAccountMonthlyCashflowVm;
    }

    public StatisticAccountMonthlyCashflowViewModel ViewModel => (StatisticAccountMonthlyCashflowViewModel)DataContext;

    public override string Header => Strings.MonthlyCashflowTitle;
}
