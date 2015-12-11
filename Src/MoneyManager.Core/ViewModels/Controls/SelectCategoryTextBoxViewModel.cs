using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels.CategoryList;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels.Controls
{
    [ImplementPropertyChanged]
    public class SelectCategoryTextBoxViewModel : BaseViewModel
    {
        private readonly ITransactionRepository transactionRepository;

        //TODO: maybe refactor this to use just a button instead of a own control. or just to code behind since pretty plattform specific
        /// <summary>
        ///     Creates an SelectCategoryTextBoxViewModel instance
        /// </summary>
        public SelectCategoryTextBoxViewModel(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

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
            SelectedTransaction.Category = null;
        }

        private void Navigate()
        {
            ShowViewModel<SelectCategoryListViewModel>();
        }
    }
}