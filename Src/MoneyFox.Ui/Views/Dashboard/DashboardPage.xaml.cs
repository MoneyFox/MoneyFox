namespace MoneyFox.Ui.Views.Dashboard;

using Common.Navigation;

public partial class DashboardPage : IBindablePage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    public DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;
}
