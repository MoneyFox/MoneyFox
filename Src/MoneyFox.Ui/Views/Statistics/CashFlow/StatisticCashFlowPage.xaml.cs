namespace MoneyFox.Ui.Views.Statistics.CashFlow;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCashFlowPage : IBindablePage
{
    public StatisticCashFlowPage()
    {
        InitializeComponent();
    }

    private StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Application.Current!.MainPage!.ShowPopup(popup);
    }
}
