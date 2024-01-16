namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

using Common.Navigation;

public partial class BudgetOverviewPage : IBindablePage
{
    public BudgetOverviewPage()
    {
        InitializeComponent();
    }

    public BudgetOverviewViewModel ViewModel => (BudgetOverviewViewModel)BindingContext;
}
