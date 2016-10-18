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
        private ObservableCollection<Category> categories;
        private Category selectedCategory;
        private ObservableCollection<AlphaGroupListGroup<Category>> source;

        /// <summary>
        ///     Baseclass for the categorylist usercontrol
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{Category}" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        protected AbstractCategoryListViewModel(ICategoryRepository categoryRepository,
            IDialogService dialogService)
        {
            DialogService = dialogService;
            CategoryRepository = categoryRepository;

            Categories = new ObservableCollection<Category>(CategoryRepository.GetList());

            Source = CreateGroup();
        }

        /// <summary>
        ///     Collection with all categories
        /// </summary>
        public ObservableCollection<Category> Categories
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
        public ObservableCollection<AlphaGroupListGroup<Category>> Source
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
        ///     Category currently selected in the view.
        /// </summary>
        public Category SelectedCategory
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
        ///     Deletes the passed Category after show a confirmation dialog.
        /// </summary>
        public MvxCommand<Category> DeleteCategoryCommand => new MvxCommand<Category>(DeleteCategory);

        /// <summary>
        ///     Edit the currently selected category
        /// </summary>
        public MvxCommand<Category> EditCategoryCommand => new MvxCommand<Category>(EditCategory);

        /// <summary>
        ///     Selects the clicked category and sends it to the message hub.
        /// </summary>
        public MvxCommand<Category> SelectCommand => new MvxCommand<Category>(Selected);

        /// <summary>
        ///     Create and save a new category group
        /// </summary>
        public MvxCommand<Category> CreateNewCategoryCommand => new MvxCommand<Category>(CreateNewCategory);

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

        private void EditCategory(Category category)
        {
            ShowViewModel<ModifyCategoryViewModel>(new {isEdit = true, selectedCategoryId = category.Id});
        }

        private void CreateNewCategory(Category category)
        {
            ShowViewModel<ModifyCategoryViewModel>(new {isEdit = false, SelectedCategory = 0});
        }

        /// <summary>
        ///     Handle the selection of a category in the list
        /// </summary>
        protected abstract void Selected(Category category);

        /// <summary>
        ///     Performs a search with the text in the searchtext property
        /// </summary>
        public void Search()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Categories = new ObservableCollection<Category>
                (CategoryRepository.GetList(
                        x => (x.Name != null) && x.Name.ToLower().Contains(searchText.ToLower()))
                    .OrderBy(x => x.Name));
            }
            else
            {
                Categories = new ObservableCollection<Category>(CategoryRepository.GetList().OrderBy(x => x.Name));
            }
            Source = CreateGroup();
        }

        private ObservableCollection<AlphaGroupListGroup<Category>> CreateGroup() =>
            new ObservableCollection<AlphaGroupListGroup<Category>>(
                AlphaGroupListGroup<Category>.CreateGroups(Categories,
                    CultureInfo.CurrentUICulture,
                    s => string.IsNullOrEmpty(s.Name)
                        ? "-"
                        : s.Name[0].ToString().ToUpper()));

        private async void DeleteCategory(Category categoryToDelete)
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