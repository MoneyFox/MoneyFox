using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Messages;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels {
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel {
        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SelectCategoryListViewModel(ICategoryRepository categoryRepository,
            IDialogService dialogService) : base(categoryRepository, dialogService) {
        }

        /// <summary>
        ///     Selects the clicked category and sends it to the message hub.
        /// </summary>
        public MvxCommand<Category> DoneCommand => new MvxCommand<Category>(Done);

        /// <summary>
        ///     Closes this activity without selecting something.
        /// </summary>
        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        private void Done(Category category) {
            MessageHub.Publish(new CategorySelectedMessage(this, category));
            Close(this);
        }

        private void Cancel() {
            Close(this);
        }
    }
}