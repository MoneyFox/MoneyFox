using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels
{
    public abstract class AbstractCategoryListViewModel : BaseViewModel
    {
        protected readonly IRepository<Category> CategoryRepository;
        protected readonly IDialogService DialogService;

        private ObservableCollection<Category> categories = new ObservableCollection<Category>();

        private string searchText;

        /// <summary>
        ///     Baseclass for the categorylist usercontrol
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        protected AbstractCategoryListViewModel(IRepository<Category> categoryRepository,
            IDialogService dialogService)
        {
            CategoryRepository = categoryRepository;
            DialogService = dialogService;

            Categories = CategoryRepository.Data;
        }

        /// <summary>
        ///     Deletes the passed Category after show a confirmation dialog.
        /// </summary>
        public MvxCommand<Category> DeleteCategoryCommand => new MvxCommand<Category>(DeleteCategory);

        public ObservableCollection<Category> Categories { get; set; }

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

        /// <summary>
        ///     Performs a search with the text in the searchtext property
        /// </summary>
        public void Search()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Categories = new ObservableCollection<Category>
                    (CategoryRepository.Data.Where(
                        x => x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()))
                        .ToList());
            }
            else
            {
                Categories = new ObservableCollection<Category>(CategoryRepository.Data);
            }
        }

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