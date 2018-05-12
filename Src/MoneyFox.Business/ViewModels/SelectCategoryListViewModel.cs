using System.Threading.Tasks;
using MoneyFox.Business.Messages;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public interface ISelectCategoryListViewModel
    {
        CategoryViewModel SelectedCategory { get; }
    }

    /// <summary>
    ///     Represents the SelectCategoryListView
    /// </summary>
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel, ISelectCategoryListViewModel
    {
        private readonly IMvxMessenger messenger;
        private CategoryViewModel selectedCategory;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
        /// </summary>
        /// <param name="categoryService">An instance of <see cref="ICategoryService" />.</param>
        /// <param name="modifyDialogService">An instance of <see cref="IModifyDialogService" /></param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        /// <param name="messenger">An instance of <see cref="IMvxMessenger" /></param>
        /// <param name="navigationService">An instance of <see cref="IMvxNavigationService" /></param>
        public SelectCategoryListViewModel(ICategoryService categoryService,
                                           IModifyDialogService modifyDialogService,
                                           IDialogService dialogService,
                                           IMvxMessenger messenger,
                                           IMvxNavigationService navigationService)
            : base(categoryService, modifyDialogService, dialogService, navigationService)
        {
            this.messenger = messenger;
        }
        
        /// <summary>
        ///     CategoryViewModel currently selected in the view.
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory == value) return;
                selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Closes this activity without selecting something.
        /// </summary>
        public MvxAsyncCommand CancelCommand => new MvxAsyncCommand(Cancel);

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override async Task ItemClick(CategoryViewModel category)
        {
            messenger.Publish(new CategorySelectedMessage(this, category));
            await NavigationService.Close(this);
        }

        private async Task Cancel()
        {
            await NavigationService.Close(this);
        }
    }
}