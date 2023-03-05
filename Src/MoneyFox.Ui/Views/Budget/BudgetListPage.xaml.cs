namespace MoneyFox.Ui.Views.Budget;

public partial class BudgetListPage
{
    public BudgetListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<BudgetListViewModel>();
    }

    public BudgetListViewModel ViewModel => (BudgetListViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.InitializeCommand.ExecuteAsync(null).GetAwaiter().GetResult();
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
