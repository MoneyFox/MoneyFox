namespace MoneyFox.Ui.Views.Budget;

using Common.Navigation;

public partial class BudgetListPage : IBindablePage
{
    public BudgetListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<BudgetListViewModel>();
    }
}
