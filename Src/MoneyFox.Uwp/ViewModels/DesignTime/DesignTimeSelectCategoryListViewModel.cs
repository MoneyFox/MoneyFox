using MoneyFox.Ui.Shared.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.Categories;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSelectCategoryListViewModel : ISelectCategoryListViewModel
    {
        public CategoryViewModel SelectedCategory { get; } = new CategoryViewModel();
    }
}
