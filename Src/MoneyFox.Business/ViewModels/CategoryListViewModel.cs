using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    public interface ICategoryListViewModel : IBaseViewModel
    {
        ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryList { get; }
        MvxAsyncCommand<CategoryViewModel> ItemClickCommand { get; }
        MvxAsyncCommand<string> SearchCommand { get; }
        bool IsCategoriesEmpty { get; }
    }

    /// <summary>
    ///     Reprensentation of the CategoryListView.
    /// </summary>
    public class CategoryListViewModel : AbstractCategoryListViewModel, ICategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryService">An instance of <see cref="ICategoryService" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        /// <param name="navigationService">An instance of <see cref="IMvxNavigationService" /></param>
        public CategoryListViewModel(ICategoryService categoryService, IDialogService dialogService, IMvxNavigationService navigationService)
            : base(categoryService, dialogService, navigationService)
        {
        }

        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override async Task ItemClick(CategoryViewModel category)
        {
            await EditCategoryCommand.ExecuteAsync(category);
        }
    }
}