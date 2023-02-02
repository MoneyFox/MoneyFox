namespace MoneyFox.Ui.Views.Budget.ModifyBudget;

public partial class AddBudgetPage
{
    public AddBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddBudgetViewModel>();
    }
}
