namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class BudgetCategoryViewModel : ObservableObject
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
        set => SetProperty(field: ref name, newValue: value);
    }
}
