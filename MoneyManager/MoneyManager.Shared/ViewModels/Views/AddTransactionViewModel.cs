using GalaSoft.MvvmLight;
using MoneyManager.Models;
using MoneyManager.Src;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace MoneyManager.ViewModels.Views
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel : ViewModelBase
    {
        private TransactionType transactionType;
        public TransactionType TransactionType
        {
            get { return transactionType; }
            set
            {
                transactionType = value;
                IsTargetAccountVisible = transactionType == TransactionType.Transfer;
                RaisePropertyChanged();
            }
        }

        public bool IsTargetAccountVisible { get; set; }

        public string TransactionTitle { get; set; }

        public ObservableCollection<Account> AllAccounts
        {
            get { return new ViewModelLocator().AccountViewModel.AllAccounts; }
        }

        public ObservableCollection<Category> AllCategories
        {
            get { return new ViewModelLocator().CategoryViewModel.AllCategories; }
        }

        public FinancialTransaction SelectedTransaction
        {
            get { return new ViewModelLocator().TransactionViewModel.SelectedTransaction; }
        }
    }
}