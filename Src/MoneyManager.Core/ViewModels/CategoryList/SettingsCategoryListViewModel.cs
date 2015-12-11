using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels.CategoryList
{
    public class SettingsCategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SettingsCategoryListViewModel(IRepository<Category> categoryRepository, IDialogService dialogService)
            : base(categoryRepository, dialogService)
        {
        }

        public Category SelectedCategory
        {
            get
            {
                return CategoryRepository.Selected ?? new Category();
            }
            set
            {
                if (value == null) return;

                CategoryRepository.Selected = value;
            }
        }
    }
}
