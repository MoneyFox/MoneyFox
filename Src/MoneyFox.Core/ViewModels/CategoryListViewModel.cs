using GalaSoft.MvvmLight.Views;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;
using PropertyChanged;
using IDialogService = MoneyFox.Core.Interfaces.IDialogService;

namespace MoneyFox.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(IRepository<Category> categoryRepository, IDialogService dialogService,
            INavigationService navigationService)
            : base(categoryRepository, dialogService, navigationService)
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