using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels {
    [ImplementPropertyChanged]
    public class CategoryListViewModel : AbstractCategoryListViewModel {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="ICategoryRepository" /></param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(ICategoryRepository categoryRepository, IDialogService dialogService)
            : base(categoryRepository, dialogService) {
        }

        public Category SelectedCategory {
            get { return CategoryRepository.Selected ?? new Category(); }
            set {
                if (value == null) {
                    return;
                }

                CategoryRepository.Selected = value;
            }
        }
    }
}