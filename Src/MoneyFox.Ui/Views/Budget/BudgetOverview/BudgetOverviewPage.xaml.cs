namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

using MoneyFox.Ui.Common.Navigation;

public partial class BudgetOverviewPage : IBindablePage
{
    public BudgetOverviewPage()
    {
        InitializeComponent();
    }

    public BudgetOverviewViewModel ViewModel => (BudgetOverviewViewModel)BindingContext;
}
