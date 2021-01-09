using MoneyFox.Ui.Shared.ViewModels.Categories;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSelectCategoryListViewModel : ISelectCategoryListViewModel
    {
        public CategoryViewModel SelectedCategory { get; } = new CategoryViewModel();
    }
}
