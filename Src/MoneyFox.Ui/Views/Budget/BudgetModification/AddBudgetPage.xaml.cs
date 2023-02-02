namespace MoneyFox.Ui.Views.Budget.BudgetModification;

public partial class AddBudgetPage
{
    public AddBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddBudgetViewModel>();
    }
}
