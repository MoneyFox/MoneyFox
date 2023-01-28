namespace MoneyFox.Ui.Views.Dashboard;

public partial class DashboardPage
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
