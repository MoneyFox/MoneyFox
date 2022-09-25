namespace MoneyFox.Ui.Views.Dashboard;

using MoneyFox.Ui.ViewModels.Dashboard;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<DashboardViewModel>();
    }

    private DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }
}
