#nullable enable
namespace MoneyFox.Uwp.ViewModels.Categories
{
    /// <summary>
    /// Represents the SelectCategoryListView
    /// </summary>
    public interface ISelectCategoryListViewModel
    {
        CategoryViewModel? SelectedCategory { get; }
    }
}