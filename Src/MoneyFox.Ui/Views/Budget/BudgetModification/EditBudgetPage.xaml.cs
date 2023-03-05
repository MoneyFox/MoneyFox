namespace MoneyFox.Ui.Views.Budget.BudgetModification;

public partial class EditBudgetPage
{
    public EditBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditBudgetViewModel>();
    }

    private EditBudgetViewModel ViewModel => (EditBudgetViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
