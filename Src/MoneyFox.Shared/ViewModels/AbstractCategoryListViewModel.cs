using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels
{
    public abstract class AbstractCategoryListViewModel : BaseViewModel, IDisposable
    {
        protected readonly IDialogService DialogService;
        protected readonly IUnitOfWork UnitOfWork;

        private string searchText;

        /// <summary>
        ///     Baseclass for the categorylist usercontrol
        /// </summary>
        /// <param name="unitOfWork">An instance of <see cref="IUnitOfWork" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        protected AbstractCategoryListViewModel(IUnitOfWork unitOfWork,
            IDialogService dialogService)
        {
            UnitOfWork = unitOfWork;
            DialogService = dialogService;

            Categories = UnitOfWork.CategoryRepository.Data;

            Source = CreateGroup();
        }

        /// <summary>
        ///     Deletes the passed Category after show a confirmation dialog.
        /// </summary>
        public MvxCommand<Category> DeleteCategoryCommand => new MvxCommand<Category>(DeleteCategory);

        /// <summary>
        ///     Collection with all categories
        /// </summary>
        public ObservableCollection<Category> Categories { get; set; }

        /// <summary>
        ///     Collection with categories alphanumeric grouped by
        /// </summary>
        public ObservableCollection<AlphaGroupListGroup<Category>> Source { get; set; }

        /// <summary>
        ///     Category currently selected in the view.
        /// </summary>
        public Category SelectedCategory { get; set; }

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

        public void Dispose()
        {
            UnitOfWork.Dispose();
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
                    (UnitOfWork.CategoryRepository.Data.Where(
                        x => x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()))
                        .OrderBy(x => x.Name));
            }
            else
            {
                Categories = new ObservableCollection<Category>(UnitOfWork.CategoryRepository.Data.OrderBy(x => x.Name));
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

                UnitOfWork.CategoryRepository.Delete(categoryToDelete);
            }
        }
    }
}