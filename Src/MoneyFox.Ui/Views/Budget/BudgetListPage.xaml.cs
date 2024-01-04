namespace MoneyFox.Ui.Views.Budget;

using Common.Navigation;

public partial class BudgetListPage : IBindablePage
{
    public BudgetListPage()
    {
        InitializeComponent();
    }

    public BudgetListViewModel ViewModel => (BudgetListViewModel)BindingContext;
}
