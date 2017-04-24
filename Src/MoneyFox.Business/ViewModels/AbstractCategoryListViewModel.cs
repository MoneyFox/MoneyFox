using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Business.ViewModels
{
    public abstract class AbstractCategoryListViewModel : BaseViewModel
    {
        protected readonly ICategoryService CategoryService;
        protected readonly IModifyDialogService ModifyDialogService;
        protected readonly IDialogService DialogService;

        private string searchText;
        private ObservableCollection<CategoryViewModel> categories;
        private CategoryViewModel selectedCategory;
        private ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> source;

        /// <summary>
        ///     Baseclass for the categorylist usercontrol
        /// </summary>
        /// <param name="categoryService">An instance of <see cref="ICategoryService" />.</param>
        /// <param name="modifyDialogService">An instance of <see cref="IModifyDialogService"/> to display a context dialog.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        protected AbstractCategoryListViewModel(ICategoryService categoryService,
           IModifyDialogService modifyDialogService, IDialogService dialogService)
        {
            CategoryService = categoryService;
            ModifyDialogService = modifyDialogService;
            DialogService = dialogService;
        }

        /// <summary>
        ///     Handle the selection of a CategoryViewModel in the list
        /// </summary>
        protected abstract void ItemClick(CategoryViewModel category);

        #region Properties


        /// <summary>
        ///     Collection with all categories
        /// </summary>
        public ObservableCollection<CategoryViewModel> Categories
        {
            get { return categories; }
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
        public ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> Source
        {
            get { return source; }
            set
            {
                if (source == value) return;
                source = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     CategoryViewModel currently selected in the view.
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (selectedCategory == value) return;
                selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCategoriesEmpty => !Categories?.Any() ?? true;

        /// <summary>
        ///     Text to search for. Will perform the search when the text changes.
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                Search();
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public MvxCommand<CategoryViewModel> DeleteCategoryCommand => new MvxCommand<CategoryViewModel>(DeleteCategory);

        /// <summary>
        ///     Edit the currently selected CategoryViewModel
        /// </summary>
        public MvxCommand<CategoryViewModel> EditCategoryCommand => new MvxCommand<CategoryViewModel>(EditCategory);

        /// <summary>
        ///     Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public MvxCommand<CategoryViewModel> ItemClickCommand => new MvxCommand<CategoryViewModel>(ItemClick);

        /// <summary>
        ///     Opens a option dialog to select the modify operation
        /// </summary>
        public MvxCommand<CategoryViewModel> OpenContextMenuCommand => new MvxCommand<CategoryViewModel>(OpenContextMenu);

        /// <summary>
        ///     Create and save a new CategoryViewModel group
        /// </summary>
        public MvxCommand<CategoryViewModel> CreateNewCategoryCommand
            => new MvxCommand<CategoryViewModel>(CreateNewCategory);

        #endregion

        /// <summary>
        ///     Performs a search with the text in the searchtext property
        /// </summary>
        public async void Search()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var searchedCategories = await CategoryService.SearchByName(searchText);
                Categories = new ObservableCollection<CategoryViewModel>(searchedCategories.Select(x => new CategoryViewModel(x)));
            } else
            {
                var selectedCategories = await CategoryService.GetAllCategories();
                Categories =
                    new ObservableCollection<CategoryViewModel>(selectedCategories.Select(x => new CategoryViewModel(x)));
            }
            Source = CreateGroup();
        }

        private void Loaded()
        {
            SearchText = string.Empty;
            Search();
        }

        private void EditCategory(CategoryViewModel category)
        {
            //TODO:
            //ShowViewModel<ModifyCategoryViewModel>(new {isEdit = true, selectedCategoryId = category.Id});
        }

        private void CreateNewCategory(CategoryViewModel category)
        {
            //TODO:
            //ShowViewModel<ModifyCategoryViewModel>(new {isEdit = false, SelectedCategory = 0});
        }

        private ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CreateGroup() =>
            new ObservableCollection<AlphaGroupListGroup<CategoryViewModel>>(
                AlphaGroupListGroup<CategoryViewModel>.CreateGroups(Categories,
                    CultureInfo.CurrentUICulture,
                    s => string.IsNullOrEmpty(s.Name)
                        ? "-"
                        : s.Name[0].ToString().ToUpper(), itemClickCommand: ItemClickCommand,
                    itemLongClickCommand:OpenContextMenuCommand));

        private async void OpenContextMenu(CategoryViewModel category)
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

        private async void DeleteCategory(CategoryViewModel categoryToDelete)
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