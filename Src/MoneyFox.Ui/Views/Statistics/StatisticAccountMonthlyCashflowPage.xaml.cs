namespace MoneyFox.Ui.Views.Statistics;

using CommunityToolkit.Maui.Views;
using Popups;

public partial class StatisticAccountMonthlyCashFlowPage
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
    }

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
