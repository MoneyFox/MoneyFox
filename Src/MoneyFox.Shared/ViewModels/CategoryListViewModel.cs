using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(IRepository<Category> categoryRepository, IDialogService dialogService)
            : base(categoryRepository, dialogService)
        {
        }

        public Category SelectedCategory
        {
            get { return CategoryRepository.Selected ?? new Category(); }
            set
            {
                if (value == null)
                {
                    return;
                }

                CategoryRepository.Selected = value;
            }
        }
    }
}