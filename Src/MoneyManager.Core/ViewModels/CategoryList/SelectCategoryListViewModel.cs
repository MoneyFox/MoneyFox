using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels.CategoryList
{
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        private readonly ITransactionRepository transactionRepository;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SelectCategoryListViewModel(IRepository<Category> categoryRepository,
            ITransactionRepository transactionRepository, 
            IDialogService dialogService)
            : base(categoryRepository, dialogService)
        {
            this.transactionRepository = transactionRepository;
        }

        public Category SelectedCategory { get; set; }

        public MvxCommand DoneCommand => new MvxCommand(Done);

        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        private void Done()
        {
            transactionRepository.Selected.Category = SelectedCategory;
            Close(this);
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}