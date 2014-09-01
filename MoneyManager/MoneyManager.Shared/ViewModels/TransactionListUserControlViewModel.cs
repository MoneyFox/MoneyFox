using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using System.Collections.ObjectModel;

namespace MoneyManager.ViewModels
{
    public class TransactionListUserControlViewModel : ViewModelBase
    {
        private Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
        }

        public ObservableCollection<FinancialTransaction> RelatedTransactions
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().RelatedTransactions; }
        }

        public RelayCommand LoadRelatedTransactionsCommand { get; private set; }

        public TransactionListUserControlViewModel()
        {
            LoadRelatedTransactionsCommand = new RelayCommand(LoadRelatedTransactions);
        }

        private void LoadRelatedTransactions()
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().GetRelatedTransactions(SelectedAccount.Id);
        }

        public void Dispose()
        {
            this.Cleanup();
        }
    }
}