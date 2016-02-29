using GalaSoft.MvvmLight.Command;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
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
        {
        }

        /// <summary>
        ///     Selects the clicked category and sends it to the message hub.
        /// </summary>
        public RelayCommand<Category> DoneCommand => new RelayCommand<Category>(Done);

        /// <summary>
        ///     Closes this activity without selecting something.
        /// </summary>
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        private void Done(Category category)
        {
            MessageHub.Publish(new CategorySelectedMessage(this, category));
            Close(this);
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}