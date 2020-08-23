using MoneyFox.Ui.Shared.ViewModels.Categories;

namespace MoneyFox.Uwp.ViewModels
{
    /// <summary>
    /// Represents the SelectCategoryListView
    /// </summary>
    public interface ISelectCategoryListViewModel
    {
        CategoryViewModel SelectedCategory { get; }
    }
}
