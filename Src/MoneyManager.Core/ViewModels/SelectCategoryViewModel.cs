using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class SelectCategoryViewModel : BaseViewModel
    {
        private readonly ITransactionRepository transactionRepository;

        /// <summary>
        ///     Creates an SelectCategoryViewModel instance
        /// </summary>
        /// <param name="transactionRepository">An instance of <see cref="ITransactionRepository" /></param>
        public SelectCategoryViewModel(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;

            if (transactionRepository.Selected == null)
            {
                transactionRepository.Selected = new FinancialTransaction();
            }
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
            transactionRepository.Selected.Category = null;
        }

        private void Navigate()
        {
            ShowViewModel<SelectCategoryViewModel>();
        }
    }
}