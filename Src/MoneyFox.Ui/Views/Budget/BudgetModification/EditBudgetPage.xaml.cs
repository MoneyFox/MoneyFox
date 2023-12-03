namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using Common.Navigation;

public partial class EditBudgetPage : IBindablePage
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
