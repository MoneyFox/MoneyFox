using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Messenger;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels.CategoryList
{
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        private readonly IMvxMessenger messenger;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SelectCategoryListViewModel(IRepository<Category> categoryRepository,
            IDialogService dialogService)
            : base(categoryRepository, dialogService)
        {
            this.messenger = messenger;
        }

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

    public class CategorySelectedMessage:MvxMessage
    {
        public CategorySelectedMessage(object sender, Category selectedCategory) : base(sender)
        {
            SelectedCategory = selectedCategory;
        }

        public Category SelectedCategory { get; private set; }
    }
}