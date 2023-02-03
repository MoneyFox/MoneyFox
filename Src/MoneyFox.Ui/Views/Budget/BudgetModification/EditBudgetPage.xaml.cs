namespace MoneyFox.Ui.Views.Budget.BudgetModification;

[QueryProperty(name: "BudgetId", queryId: "budgetId")]
public partial class EditBudgetPage
{
    public EditBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditBudgetViewModel>();
    }

    private EditBudgetViewModel ViewModel => (EditBudgetViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeCommand.ExecuteAsync(budgetId);
    }

#pragma warning disable S2376 // Write-only properties should not be used
    private int budgetId;
    public string BudgetId
    {
        set => budgetId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used
}
