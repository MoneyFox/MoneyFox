namespace MoneyFox.Ui.Views.Budget.BudgetModification;

public partial class AddBudgetPage
{
    public AddBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddBudgetViewModel>();
    }

    private AddBudgetViewModel ViewModel => (AddBudgetViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
