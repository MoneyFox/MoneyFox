using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Localization;
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
        /// <param name="modifyDialogService">An instance of <see cref="IModifyDialogService" /></param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        /// <param name="navigationService">An instance of <see cref="IMvxNavigationService" /></param>
        public CategoryListViewModel(ICategoryService categoryService, IModifyDialogService modifyDialogService, IDialogService dialogService, IMvxNavigationService navigationService)
            : base(categoryService, modifyDialogService, dialogService, navigationService)
        {
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);
        
        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override async Task ItemClick(CategoryViewModel category)
        {
            await EditCategoryCommand.ExecuteAsync(category);
        }
    }
}