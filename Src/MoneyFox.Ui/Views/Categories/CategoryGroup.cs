namespace MoneyFox.Ui.Views.Categories;

public class CategoryGroup : List<CategoryListItemViewModel>
{
    public CategoryGroup(string title, List<CategoryListItemViewModel> categoryItems) : base(categoryItems)
    {
        Title = title;
    }

    public string Title { get; set; }
}
