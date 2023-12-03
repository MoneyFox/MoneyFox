namespace MoneyFox.Ui.Views.Statistics.MonthlyAccountCashFlow;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticAccountMonthlyCashFlowPage: IBindablePage
{
    public StatisticAccountMonthlyCashFlowPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticAccountMonthlyCashFlowViewModel>();
    }

    private StatisticAccountMonthlyCashFlowViewModel ViewModel => (StatisticAccountMonthlyCashFlowViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.InitCommand.Execute(null);
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
