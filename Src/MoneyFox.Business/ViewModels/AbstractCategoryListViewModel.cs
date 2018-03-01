using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.Parameters;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MoneyFox.Foundation;
using MvvmCross.Core.Navigation;

namespace MoneyFox.Business.ViewModels
{
    public abstract class AbstractCategoryListViewModel : MvxViewModel
    {
        protected readonly ICategoryService CategoryService;
        protected readonly IModifyDialogService ModifyDialogService;
        protected readonly IDialogService DialogService;
        protected readonly IMvxNavigationService NavigationService;
        
        private ObservableCollection<CategoryViewModel> categories;
        private ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> source;

        /// <summary>
        ///     Baseclass for the categorylist usercontrol
        /// </summary>
        /// <param name="categoryService">An instance of <see cref="ICategoryService" />.</param>
        /// <param name="modifyDialogService">An instance of <see cref="IModifyDialogService"/> to display a context dialog.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        /// <param name="navigationService">An instance of <see cref="IMvxNavigationService" /></param>
        protected AbstractCategoryListViewModel(ICategoryService categoryService,
           IModifyDialogService modifyDialogService, IDialogService dialogService, 
           IMvxNavigationService navigationService)
        {
            CategoryService = categoryService;
            ModifyDialogService = modifyDialogService;
            DialogService = dialogService;
            this.NavigationService = navigationService;
        }

        /// <summary>
        ///     Handle the selection of a CategoryViewModel in the list
        /// </summary>
        protected abstract Task ItemClick(CategoryViewModel category);

        #region Properties


        /// <summary>
        ///     Collection with all categories
        /// </summary>
        public ObservableCollection<CategoryViewModel> Categories
        {
            get => categories;
            set
            {
                if (categories == value) return;
                categories = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsCategoriesEmpty));
            }
        }

        /// <summary>
        ///     Collection with categories alphanumeric grouped by
        /// </summary>
        public ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryList
        {
            get => source;
            set
            {
                if (source == value) return;
                source = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCategoriesEmpty => !Categories?.Any() ?? true;

        #endregion

        #region Commands

        /// <summary>
        ///     Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public MvxAsyncCommand<CategoryViewModel> DeleteCategoryCommand => new MvxAsyncCommand<CategoryViewModel>(DeleteCategory);

        /// <summary>
        ///     Edit the currently selected CategoryViewModel
        /// </summary>
        public MvxAsyncCommand<CategoryViewModel> EditCategoryCommand => new MvxAsyncCommand<CategoryViewModel>(EditCategory);

        /// <summary>
        ///     Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public MvxAsyncCommand<CategoryViewModel>ItemClickCommand  => new MvxAsyncCommand<CategoryViewModel>(ItemClick);

        /// <summary>
        ///     Opens a option dialog to select the modify operation
        /// </summary>
        public MvxAsyncCommand<CategoryViewModel> OpenContextMenuCommand => new MvxAsyncCommand<CategoryViewModel>(OpenContextMenu);

        /// <summary>
        ///     Executes a search for the passed term and updates the displayed list.
        /// </summary>
        public MvxAsyncCommand<string> SearchCommand => new MvxAsyncCommand<string>(Search);

        /// <summary>
        ///     Create and save a new CategoryViewModel group
        /// </summary>
        public MvxAsyncCommand<CategoryViewModel> CreateNewCategoryCommand
            => new MvxAsyncCommand<CategoryViewModel>(CreateNewCategory);

        #endregion

        public override async Task Initialize()
        {
            await Loaded();
        }

        /// <summary>
        ///     Performs a search with the text in the searchtext property
        /// </summary>
        public async Task Search(string searchText = "")
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                var searchedCategories = await CategoryService.SearchByName(searchText);
                Categories = new ObservableCollection<CategoryViewModel>(searchedCategories.Select(x => new CategoryViewModel(x)));
            } else
            {
                var selectedCategories = await CategoryService.GetAllCategories();
                Categories =
                    new ObservableCollection<CategoryViewModel>(selectedCategories.Select(x => new CategoryViewModel(x)));
            }
            CategoryList = CreateGroup();
        }

        private async Task Loaded()
        {
            await Search();
        }

        private async Task EditCategory(CategoryViewModel category)
        {
            await NavigationService.Navigate<ModifyCategoryViewModel, ModifyCategoryParameter>(new ModifyCategoryParameter(category.Id));
        }

        private async Task CreateNewCategory(CategoryViewModel category)
        {
            await NavigationService.Navigate<ModifyCategoryViewModel, ModifyCategoryParameter>(new ModifyCategoryParameter());
        }

        private ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CreateGroup() =>
            new ObservableCollection<AlphaGroupListGroup<CategoryViewModel>>(
                AlphaGroupListGroup<CategoryViewModel>.CreateGroups(Categories,
                    CultureInfo.CurrentUICulture,
                    s => string.IsNullOrEmpty(s.Name)
                        ? "-"
                        : s.Name[0].ToString().ToUpper(), itemClickCommand: ItemClickCommand,
                    itemLongClickCommand:OpenContextMenuCommand));

        private async Task OpenContextMenu(CategoryViewModel category)
        {
            var result = await ModifyDialogService.ShowEditSelectionDialog();

            switch (result)
            {
                case ModifyOperation.Edit:
                    EditCategoryCommand.Execute(category);
                    break;

                case ModifyOperation.Delete:
                    DeleteCategoryCommand.Execute(category);
                    break;
            }
        }

        private async Task DeleteCategory(CategoryViewModel categoryToDelete)
        {
            if (await DialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                if (Categories.Contains(categoryToDelete))
                {
                    Categories.Remove(categoryToDelete);
                }

                await CategoryService.DeleteCategory(categoryToDelete.Category);
                Search();
            }
        }
    }
}