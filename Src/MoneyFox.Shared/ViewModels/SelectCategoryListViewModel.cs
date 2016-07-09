using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Messages;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="unitOfWork">An instance of <see cref="IUnitOfWork" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SelectCategoryListViewModel(IUnitOfWork unitOfWork,
            IDialogService dialogService) : base(unitOfWork, dialogService)
        {
        }

        /// <summary>
        ///     Closes this activity without selecting something.
        /// </summary>
        public MvxCommand CancelCommand => new MvxCommand(Cancel);


        /// <summary>
        ///     Post selected category to message hub
        /// </summary>
        protected override void Selected(Category category)
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