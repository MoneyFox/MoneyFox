namespace MoneyFox.Ui.Views.Budget;

using Common.Navigation;

public partial class BudgetListPage : IBindablePage
{
    public BudgetListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<BudgetListViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if WINDOWS        
        ViewModel.OnNavigatedAsync(null).GetAwaiter().GetResult();
#endif
    }

    public BudgetListViewModel ViewModel => (BudgetListViewModel)BindingContext;
}
