namespace MoneyFox.Ui.Views.Budget;

using Common.Navigation;

public partial class BudgetListPage : IBindablePage
{
    public BudgetListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<BudgetListViewModel>();
    }

    public BudgetListViewModel ViewModel => (BudgetListViewModel)BindingContext;

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if WINDOWS
        ViewModel.OnNavigatedAsync(null).GetAwaiter().GetResult();
#endif
    }
}
