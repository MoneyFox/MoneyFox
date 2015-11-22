using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels
{
    public class SelectCategoryTextBoxViewModel : BaseViewModel
    {
        private readonly ModifyTransactionViewModel modifyTransactionViewModel;

        //TODO: maybe refactor this to use just a button instead of a own control.
        /// <summary>
        ///     Creates an SelectCategoryTextBoxViewModel instance
        /// </summary>
        /// <param name="modifyTransactionViewModel">An instance of <see cref="ModifyTransactionViewModel" /></param>
        public SelectCategoryTextBoxViewModel(ModifyTransactionViewModel modifyTransactionViewModel)
        {
            this.modifyTransactionViewModel = modifyTransactionViewModel;

            if (SelectedTransaction == null)
            {
                SelectedTransaction = new FinancialTransaction();
            }
        }

        public FinancialTransaction SelectedTransaction
        {
            get { return modifyTransactionViewModel.SelectedTransaction; }
            set { modifyTransactionViewModel.SelectedTransaction = value; }
        }

        /// <summary>
        ///     Resets the category of the currently selected transaction
        /// </summary>
        public IMvxCommand ResetCategoryCommand => new MvxCommand(ResetSelection);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public IMvxCommand GoToSelectCategorydialogCommand => new MvxCommand(Navigate);

        private void ResetSelection()
        {
            SelectedTransaction.Category = null;
        }

        private void Navigate()
        {
            ShowViewModel<CategoryListViewModel>();
        }
    }
}