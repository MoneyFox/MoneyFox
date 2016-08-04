using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{Category}" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(IRepository<Category> categoryRepository, IDialogService dialogService)
            : base(categoryRepository, dialogService)
        {
        }

        protected override void Selected(Category category)
        {
            //Do nothing later will redirect to category spending details
        }
    }
}