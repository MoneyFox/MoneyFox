namespace MoneyFox.Ui.Views.Dashboard;

using ViewModels.Dashboard;
using ViewModels.Payments;

public partial class DashboardPage : ContentPage
{
    const string ClosedPanelState = "ClosedPanel";
    const string OpenPanelState = "OpenPanel";

    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<DashboardViewModel>();

        VisualStateManager.GoToState(this, ClosedPanelState);
    }

    private DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }

    private void ClickClosePanel(object sender, EventArgs e)
    {
        VisualStateManager.GoToState(this, ClosedPanelState);
    }

    private void OpenAddPaymentPanel(object? sender, EventArgs e)
    {
        AddPaymentView.BindingContext = App.GetViewModel<AddPaymentViewModel>();
        VisualStateManager.GoToState(this, OpenPanelState);
    }
}
