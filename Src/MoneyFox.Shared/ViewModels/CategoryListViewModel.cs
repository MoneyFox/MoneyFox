using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="ICategoryRepository" /></param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(ICategoryRepository categoryRepository, IDialogService dialogService)
            : base(categoryRepository, dialogService)
        {
        }

        /// <summary>
        ///     Category currently selected in the view.
        /// </summary>
        public Category SelectedCategory { get; set; }

        /// <summary>
        /// Edit the currently selected category
        /// </summary>
        public MvxCommand<Category> EditCategoryCommand => new MvxCommand<Category>(EditCategory);

        /// <summary>
        /// Create and save a new category group
        /// </summary>
        public MvxCommand<Category> CreateNewCategoryCommand => new MvxCommand<Category>(CreateNewCategory);

        private void EditCategory(Category category)
        {
            ShowViewModel<ModifyCategoryViewModel>(new { isEdit = true, selectedCategoryId = category.Id });
        }

        private void CreateNewCategory(Category category)
        {
            ShowViewModel<ModifyCategoryViewModel>(new { isEdit = false, SelectedCategory = 0 });
        }
    }
}