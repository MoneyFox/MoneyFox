namespace MoneyFox.Ui.Views.Statistics.CashFlow;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCashFlowPage : IBindablePage
{
    public StatisticCashFlowPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticCashFlowViewModel>();
    }

    private StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.LoadedCommand.Execute(null);
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
