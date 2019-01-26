using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Groups;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Defines the interface for a category list.
    /// </summary>
    public interface ICategoryListViewModel : IBaseViewModel
    {
        /// <summary>
        ///     List of categories.
        /// </summary>
        ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryList { get; }

        /// <summary>
        ///     Command for the item click.
        /// </summary>
        MvxAsyncCommand<CategoryViewModel> ItemClickCommand { get; }

        /// <summary>
        ///     Search command
        /// </summary>
        MvxAsyncCommand<string> SearchCommand { get; }

        /// <summary>
        ///     Indicates if the category list is empty.
        /// </summary>
        bool IsCategoriesEmpty { get; }
    }

    public class CategoryListViewModel : AbstractCategoryListViewModel, ICategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        public CategoryListViewModel(ICrudServicesAsync curdServicesAsync,
                                     IDialogService dialogService,
                                     IMvxLogProvider logProvider,
                                     IMvxNavigationService navigationService) : base(curdServicesAsync, dialogService, logProvider, navigationService)
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