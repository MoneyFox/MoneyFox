using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Messages;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels.CategoryList
{
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SelectCategoryListViewModel(IRepository<Category> categoryRepository,
            IDialogService dialogService) : base(categoryRepository, dialogService)
        {}

        public Category SelectedCategory { get; set; }

        public MvxCommand DoneCommand => new MvxCommand(Done);

        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        private void Done()
        {
            MessageHub.Publish(new CategorySelectedMessage(this, SelectedCategory));
            Close(this);
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}