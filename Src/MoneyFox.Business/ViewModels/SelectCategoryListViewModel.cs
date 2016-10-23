using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        private readonly IMvxMessenger messenger;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" />.</param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        /// <param name="messenger">An instance of <see cref="IMvxMessenger" /></param>
        public SelectCategoryListViewModel(ICategoryRepository categoryRepository,
            IDialogService dialogService, IMvxMessenger messenger) 
            : base(categoryRepository, dialogService)
        {
            this.messenger = messenger;
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
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override void Selected(CategoryViewModel category)
        {
            messenger.Publish(new CategorySelectedMessage(this, category));
            Close(this);
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}