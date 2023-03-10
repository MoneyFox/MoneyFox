namespace MoneyFox.Ui.Views.Categories;

using System.Collections.Generic;

public class CategoryGroup : List<CategoryListItemViewModel>
{
    public CategoryGroup(string title, List<CategoryListItemViewModel> categoryItems) : base(categoryItems)
    {

        Title = title;
    }

    public string Title { get; set; }
}
