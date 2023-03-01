namespace MoneyFox.Ui.Views.Budget.BudgetModification;

public sealed class BudgetCategoryViewModel : ObservableViewModelBase
{
    private string name = string.Empty;

    public BudgetCategoryViewModel(int categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }

    public int CategoryId { get; }

    public string Name
    {
        get => name;
        set => SetProperty(property: ref name, value: value);
    }
}
