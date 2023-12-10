namespace MoneyFox.Ui.Views.Dashboard;

using Common.Navigation;

public partial class DashboardPage : IBindablePage
{
    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<DashboardViewModel>();
    }

    public DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.OnNavigatedAsync(null);
    }
}
