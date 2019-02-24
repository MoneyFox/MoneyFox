using System.Threading.Tasks;
using GenericServices;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Messages;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Represents the SelectCategoryListView
    /// </summary>
    public interface ISelectCategoryListViewModel
    {
        CategoryViewModel SelectedCategory { get; }
    }

    /// <inheritdoc cref="ISelectCategoryListViewModel"/>
    public class 
        SelectCategoryListViewModel : AbstractCategoryListViewModel, ISelectCategoryListViewModel
    {
        private readonly IMvxMessenger messenger;
        private CategoryViewModel selectedCategory;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
        /// </summary>
        public SelectCategoryListViewModel(ICrudServicesAsync crudServicesAsync,
                                           IDialogService dialogService,
                                           IMvxMessenger messenger,
                                           IMvxLogProvider logProvider,
                                           IMvxNavigationService navigationService) : base(crudServicesAsync, dialogService, logProvider, navigationService)
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
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override async Task ItemClick(CategoryViewModel category)
        {
            messenger.Publish(new CategorySelectedMessage(this, category));
            await NavigationService.Close(this).ConfigureAwait(true);
        }
    }
}