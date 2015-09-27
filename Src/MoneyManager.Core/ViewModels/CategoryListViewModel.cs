using System.Collections.ObjectModel;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class CategoryListViewModel : BaseViewModel
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IDialogService dialogService;
        private readonly ITransactionRepository transactionRepository;

        private string searchText;

        /// <summary>
        ///     Creates an CategoryListViewModel
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="transactionRepository">An instance of <see cref="ITransactionRepository" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(IRepository<Category> categoryRepository,
            ITransactionRepository transactionRepository,
            IDialogService dialogService)
        {
            this.categoryRepository = categoryRepository;
            this.transactionRepository = transactionRepository;
            this.dialogService = dialogService;

            Categories = categoryRepository.Data;
        }

        /// <summary>
        ///     Deletes the passed Category after show a confirmation dialog.
        /// </summary>
        public MvxCommand<Category> DeleteCategoryCommand => new MvxCommand<Category>(DeleteCategory);

        public MvxCommand DoneCommand => new MvxCommand(Done);
        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     Indicates wether the view is shown from the settings to adjust something
        ///     or from the transaction modificatio nto select a category.
        /// </summary>
        public bool IsSettingCall { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        /// <summary>
        ///     The currently selected category. If IsSettingsCall is set it will
        ///     return and set the selected item in the CategoryRepository, otherwise
        ///     the category of the selected transaction.
        /// </summary>
        public Category SelectedCategory
        {
            get
            {
                var selected = IsSettingCall
                    ? categoryRepository.Selected
                    : transactionRepository.Selected?.Category;

                return selected ?? new Category();
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (IsSettingCall)
                {
                    categoryRepository.Selected = value;
                }
                else
                {
                    transactionRepository.Selected.Category = value;
                }
            }
        }

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
            if (SearchText != string.Empty)
            {
                Categories = new ObservableCollection<Category>
                    (categoryRepository.Data.Where(
                        x => x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()))
                        .ToList());
            }
            else
            {
                Categories = categoryRepository.Data;
            }
        }

        private async void DeleteCategory(Category categoryToDelete)
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                categoryRepository.Delete(categoryToDelete);
            }
        }

        private void Done()
        {
            transactionRepository.Selected.Category = SelectedCategory;
            Categories = categoryRepository.Data;
            Close(this);
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}