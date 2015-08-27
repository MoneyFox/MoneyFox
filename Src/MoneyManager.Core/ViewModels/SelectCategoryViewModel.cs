using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class SelectCategoryViewModel :  BaseViewModel
    {
        /// <summary>
        ///     Creates an SelectCategoryViewModel instance
        /// </summary>
        /// <param name="transactionRepository">An instance of <see cref="ITransactionRepository"/></param>
        public SelectCategoryViewModel(ITransactionRepository transactionRepository)
        {
            ResetCategoryCommand = new MvxCommand(() => transactionRepository.Selected.Category = null);
            GoToSelectCategorydialogCommand = new MvxCommand(Navigate);
        }

        private void Navigate()
        {
            ShowViewModel<SelectCategoryViewModel>();
        }

        /// <summary>
        ///     Resets the category of the currently selected transaction
        /// </summary>
        public MvxCommand ResetCategoryCommand { get; set; }

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public MvxCommand GoToSelectCategorydialogCommand { get; set; }
    }
}
