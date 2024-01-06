namespace MoneyFox.Ui.Views.Dashboard;

public partial class DashboardView
{
    public DashboardView()
    {
        InitializeComponent();
    }

    public DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;
}
