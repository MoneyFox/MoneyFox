using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using System.Collections.ObjectModel;
using PropertyChanged;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class TransactionListUserControlViewModel : ViewModelBase
    {
        private Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
        }

        public ObservableCollection<FinancialTransaction> RelatedTransactions { get; set; }

        public RelayCommand LoadRelatedTransactionsCommand { get; private set; }

        public TransactionListUserControlViewModel()
        {
            LoadRelatedTransactionsCommand = new RelayCommand(LoadRelatedTransactions);
        }

        private void LoadRelatedTransactions()
        {
            RelatedTransactions = ServiceLocator.Current.GetInstance<TransactionDataAccess>().GetRelatedTransactions(SelectedAccount.Id);
        }
    }
}