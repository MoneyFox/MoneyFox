using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels.Controls;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels.CategoryList
{
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        private SelectCategoryTextBoxViewModel selectCategoryTextBoxViewModel;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SelectCategoryListViewModel(IRepository<Category> categoryRepository,
            SelectCategoryTextBoxViewModel selectCategoryTextBoxViewModel, 
            IDialogService dialogService)
            : base(categoryRepository, dialogService)
        {
            this.selectCategoryTextBoxViewModel = selectCategoryTextBoxViewModel;
        }

        public Category SelectedCategory { get; set; }

        public MvxCommand DoneCommand => new MvxCommand(Done);

        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        private void Done()
        {
            selectCategoryTextBoxViewModel.ResetCategoryCommand.Execute();
            selectCategoryTextBoxViewModel.SelectedTransaction.Category = SelectedCategory;
            Close(this);
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}