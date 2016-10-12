using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Messages;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public SelectCategoryListViewModel(ICategoryRepository categoryRepository,
            IDialogService dialogService) : base(categoryRepository, dialogService)
        {
        }

        /// <summary>
        ///     Closes this activity without selecting something.
        /// </summary>
        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

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