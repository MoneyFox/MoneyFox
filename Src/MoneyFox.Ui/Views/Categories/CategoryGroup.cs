namespace MoneyFox.Ui.Views.Categories;

public class CategoryGroup : List<CategoryListItemViewModel>
{
    public CategoryGroup(string title, IEnumerable<CategoryListItemViewModel> categoryItems) : base(categoryItems)
    {
        Title = title;
    }

    public string Title { get; set; }
}
