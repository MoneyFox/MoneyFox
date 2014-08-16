using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using PropertyChanged;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionUserControlViewModel : ViewModelBase
    {
        public FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        public bool IsEdit { get; set; }

        public TransactionType TransactionType { get; set; }

        public AddTransactionUserControlViewModel()
        {
            SelectedTransaction = new FinancialTransaction();
        }
    }
}
