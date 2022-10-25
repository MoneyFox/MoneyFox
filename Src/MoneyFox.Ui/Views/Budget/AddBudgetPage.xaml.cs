namespace MoneyFox.Ui.Views.Budget;

using Core.Resources;

public partial class AddBudgetPage
{
    public AddBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddBudgetViewModel>();
    }
}
