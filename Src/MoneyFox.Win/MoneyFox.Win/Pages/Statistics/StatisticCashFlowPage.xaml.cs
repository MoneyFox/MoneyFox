namespace MoneyFox.Win.Pages.Statistics;

using Core.Resources;
using ViewModels.Statistics;

public partial class StatisticCashFlowPage : BasePage
{
    public StatisticCashFlowPage()
    {
        InitializeComponent();
        DataContext = App.GetViewModel<StatisticCashFlowViewModel>();
    }

    internal StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)DataContext;

    public override string Header => Strings.CashFlowStatisticTitle;
}
