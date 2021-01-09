using MoneyFox.Ui.Shared.ViewModels.Categories;

#nullable enable
namespace MoneyFox.Uwp.ViewModels
{
    /// <summary>
    /// Represents the SelectCategoryListView
    /// </summary>
    public interface ISelectCategoryListViewModel
    {
        CategoryViewModel? SelectedCategory { get; }
    }
}
