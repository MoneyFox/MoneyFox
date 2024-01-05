namespace MoneyFox.Ui.Views.Statistics.MonthlyAccountCashFlow;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticAccountMonthlyCashFlowPage : IBindablePage
{
    public StatisticAccountMonthlyCashFlowPage()
    {
        InitializeComponent();
    }

    private StatisticAccountMonthlyCashFlowViewModel ViewModel => (StatisticAccountMonthlyCashFlowViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Application.Current!.MainPage!.ShowPopup(popup);
    }
}
