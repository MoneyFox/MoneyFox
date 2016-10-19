using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels
{
    public abstract class AbstractCategoryListViewModel : BaseViewModel
    {
        protected readonly ICategoryRepository CategoryRepository;
        protected readonly IDialogService DialogService;

        private string searchText;
        private ObservableCollection<CategoryViewModel> categories;
        private CategoryViewModel selectedCategory;
        private ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> source;

        /// <summary>
        ///     Baseclass for the categorylist usercontrol
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{CategoryViewModel}" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        protected AbstractCategoryListViewModel(ICategoryRepository categoryRepository,
            IDialogService dialogService)
        {
            DialogService = dialogService;
            CategoryRepository = categoryRepository;

            Categories = new ObservableCollection<CategoryViewModel>(CategoryRepository.GetList());

            Source = CreateGroup();
        }

        /// <summary>
        ///     Collection with all categories
        /// </summary>
        public ObservableCollection<CategoryViewModel> Categories
        {
            get { return categories; }
            set
            {
                if(categories == value) return;
                categories = value;
                RaisePropertyChanged();
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
                if(source == value) return;
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
                if(selectedCategory == value) return;
                selectedCategory = value;
                RaisePropertyChanged();
            }
        }
        
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
        public MvxCommand<CategoryViewModel> SelectCommand => new MvxCommand<CategoryViewModel>(Selected);

        /// <summary>
        ///     Create and save a new CategoryViewModel group
        /// </summary>
        public MvxCommand<CategoryViewModel> CreateNewCategoryCommand => new MvxCommand<CategoryViewModel>(CreateNewCategory);

        public bool IsCategoriesEmpty => !Categories.Any();

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
            }
        }

        private void EditCategory(CategoryViewModel category)
        {
            ShowViewModel<ModifyCategoryViewModel>(new {isEdit = true, selectedCategoryId = category.Id});
        }

        private void CreateNewCategory(CategoryViewModel category)
        {
            ShowViewModel<ModifyCategoryViewModel>(new {isEdit = false, SelectedCategory = 0});
        }

        /// <summary>
        ///     Handle the selection of a CategoryViewModel in the list
        /// </summary>
        protected abstract void Selected(CategoryViewModel category);

        /// <summary>
        ///     Performs a search with the text in the searchtext property
        /// </summary>
        public void Search()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Categories = new ObservableCollection<CategoryViewModel>
                (CategoryRepository.GetList(
                        x => (x.Name != null) && x.Name.ToLower().Contains(searchText.ToLower()))
                    .OrderBy(x => x.Name));
            }
            else
            {
                Categories = new ObservableCollection<CategoryViewModel>(CategoryRepository.GetList().OrderBy(x => x.Name));
            }
            Source = CreateGroup();
        }

        private ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CreateGroup() =>
            new ObservableCollection<AlphaGroupListGroup<CategoryViewModel>>(
                AlphaGroupListGroup<CategoryViewModel>.CreateGroups(Categories,
                    CultureInfo.CurrentUICulture,
                    s => string.IsNullOrEmpty(s.Name)
                        ? "-"
                        : s.Name[0].ToString().ToUpper()));

        private async void DeleteCategory(CategoryViewModel categoryToDelete)
        {
            if (await DialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                if (Categories.Contains(categoryToDelete))
                {
                    Categories.Remove(categoryToDelete);
                }

                CategoryRepository.Delete(categoryToDelete);
            }
        }
    }
}