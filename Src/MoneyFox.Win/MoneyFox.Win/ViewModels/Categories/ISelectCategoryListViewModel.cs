using MoneyFox;

namespace MoneyFox.Win.ViewModels.Categories
{
    /// <summary>
    ///     Represents the SelectCategoryListView
    /// </summary>
    public interface ISelectCategoryListViewModel
    {
        CategoryViewModel? SelectedCategory { get; }
    }
}