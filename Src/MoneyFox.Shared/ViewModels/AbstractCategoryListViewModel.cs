using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels {
    public abstract class AbstractCategoryListViewModel : BaseViewModel {
        protected readonly ICategoryRepository CategoryRepository;
        protected readonly IDialogService DialogService;

        private string searchText;

        /// <summary>
        ///     Baseclass for the categorylist usercontrol
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="ICategoryRepository" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        protected AbstractCategoryListViewModel(ICategoryRepository categoryRepository,
            IDialogService dialogService) {
            CategoryRepository = categoryRepository;
            DialogService = dialogService;

            Categories = CategoryRepository.Data;

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

        public bool IsCategoriesEmpty => !Categories.Any();

        /// <summary>
        ///     Text to search for. Will perform the search when the text changes.
        /// </summary>
        public string SearchText {
            get { return searchText; }
            set {
                searchText = value;
                Search();
            }
        }

        /// <summary>
        ///     Performs a search with the text in the searchtext property
        /// </summary>
        public void Search() {
            if (!string.IsNullOrEmpty(SearchText)) {
                Categories = new ObservableCollection<Category>
                    (CategoryRepository.Data.Where(
                        x => x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()))
                        .OrderBy(x => x.Name));
            }
            else {
                Categories = new ObservableCollection<Category>(CategoryRepository.Data.OrderBy(x => x.Name));
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

        private async void DeleteCategory(Category categoryToDelete) {
            if (await DialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage)) {
                if (Categories.Contains(categoryToDelete)) {
                    Categories.Remove(categoryToDelete);
                }

                CategoryRepository.Delete(categoryToDelete);
            }
        }
    }
}