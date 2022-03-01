namespace MoneyFox.Win.ViewModels.DesignTime;

using Categories;

public class DesignTimeSelectCategoryListViewModel : ISelectCategoryListViewModel
{
    public CategoryViewModel SelectedCategory { get; } = new();
}