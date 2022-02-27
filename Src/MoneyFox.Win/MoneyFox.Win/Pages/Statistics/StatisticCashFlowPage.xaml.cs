namespace MoneyFox.Win.Pages.Statistics;

using Core.Resources;
using ViewModels.Statistics;

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