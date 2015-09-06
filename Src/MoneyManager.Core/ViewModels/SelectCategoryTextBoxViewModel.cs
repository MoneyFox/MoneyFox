using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class SelectCategoryTextBoxViewModel : BaseViewModel
    {
        private readonly ITransactionRepository transactionRepository;

        /// <summary>
        ///     Creates an SelectCategoryTextBoxViewModel instance
        /// </summary>
        /// <param name="transactionRepository">An instance of <see cref="ITransactionRepository" /></param>
        public SelectCategoryTextBoxViewModel(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;

            if (transactionRepository.Selected == null)
            {
                transactionRepository.Selected = new FinancialTransaction();
            }
        }

        public Category SelectedCategory { get; set; }

        public FinancialTransaction SelectedTransaction
        {
            get { return transactionRepository.Selected; }
            set { transactionRepository.Selected = value; }
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
            ShowViewModel<CategoryListViewModel>();
        }
    }
}