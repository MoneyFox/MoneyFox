namespace MoneyFox.Ui.Views.Dashboard;

using ViewModels.Dashboard;
using ViewModels.Payments;

public partial class DashboardPage : ContentPage
{
    private const string CLOSED_PANEL_STATE = "ClosedPanel";
    private const string OPEN_PANEL_STATE = "OpenPanel";

    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<DashboardViewModel>();

        VisualStateManager.GoToState(this, CLOSED_PANEL_STATE);
    }

    private DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }

    private void ClickClosePanel(object sender, EventArgs e)
    {
        VisualStateManager.GoToState(this, CLOSED_PANEL_STATE);
    }

    private async void OpenAddPaymentPanel(object? sender, EventArgs e)
    {
        var viewModel = App.GetViewModel<AddPaymentViewModel>();
        await viewModel.InitializeAsync();
        AddPaymentView.BindingContext = viewModel;
        VisualStateManager.GoToState(this, OPEN_PANEL_STATE);
    }
}
