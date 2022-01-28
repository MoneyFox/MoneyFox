using MoneyFox.Win.ViewModels.Categories;

namespace MoneyFox.Win.ViewModels.DesignTime
{
    public class DesignTimeSelectCategoryListViewModel : ISelectCategoryListViewModel
    {
        public CategoryViewModel SelectedCategory { get; } = new CategoryViewModel();
    }
}