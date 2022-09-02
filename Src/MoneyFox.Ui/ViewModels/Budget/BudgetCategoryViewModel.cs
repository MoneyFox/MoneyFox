namespace MoneyFox.ViewModels.Budget;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class BudgetCategoryViewModel : ObservableObject
{
    private string name;

    public BudgetCategoryViewModel(int categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }

    public int CategoryId { get; }

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }
}
