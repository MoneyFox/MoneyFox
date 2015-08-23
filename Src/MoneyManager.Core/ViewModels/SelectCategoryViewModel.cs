using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class SelectCategoryViewModel :  ViewModelBase
    {
        /// <summary>
        ///     Creates an SelectCategoryViewModel instance
        /// </summary>
        /// <param name="transactionRepository">An instance of <see cref="ITransactionRepository"/></param>
        /// <param name="navigationService">An instance of <see cref="INavigationService"/></param>
        public SelectCategoryViewModel(ITransactionRepository transactionRepository, INavigationService navigationService)
        {
            ResetCategoryCommand = new RelayCommand(() => transactionRepository.Selected.Category = null);
            GoToSelectCategorydialogCommand = new RelayCommand(() => navigationService.NavigateTo("SelectCategoryView"));
        }

        /// <summary>
        ///     Resets the category of the currently selected transaction
        /// </summary>
        public RelayCommand ResetCategoryCommand { get; set; }

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public RelayCommand GoToSelectCategorydialogCommand { get; set; }
    }
}
