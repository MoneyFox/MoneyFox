namespace MoneyFox.Ui.Views.Budget;

public partial class AddBudgetPage
{
    public AddBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddBudgetViewModel>();
    }
}
