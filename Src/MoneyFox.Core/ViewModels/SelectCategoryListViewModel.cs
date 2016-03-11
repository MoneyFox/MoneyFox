using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces;
using PropertyChanged;
using IDialogService = MoneyManager.Foundation.Interfaces.IDialogService;

namespace MoneyFox.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a category selection.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" /> of type category.</param>
        /// <param name="dialogService">An instance of <see cref="MoneyManager.Foundation.Interfaces.IDialogService" /></param>
        /// <param name="navigationService">An instance of <see cref="GalaSoft.MvvmLight.Views.INavigationService" /></param>
        public SelectCategoryListViewModel(IRepository<Category> categoryRepository,
            IDialogService dialogService,
            INavigationService navigationService) : base(categoryRepository, dialogService, navigationService)
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
            MessengerInstance.Send(category);
            NavigationService.GoBack();
        }

        private void Cancel()
        {
            NavigationService.GoBack();
        }
    }
}