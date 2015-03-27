using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;
using QKit.JumpList;

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class TransactionListViewModel : ViewModelBase {
        private ITransactionRepository transactionRepository {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>(); }
        }

        private IAccountRepository AccountRepository {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>(); }
        }

        public string Title {
            get { return AccountRepository.Selected.Name; }
        }

        public List<JumpListGroup<FinancialTransaction>> RelatedTransactions { set; get; }

        public void SetRelatedTransactions(int accountId) {
            IEnumerable<FinancialTransaction> related = transactionRepository.GetRelatedTransactions(accountId);

            var dateInfo = new DateTimeFormatInfo();
            RelatedTransactions = related.ToGroups(x => x.Date,
                x => dateInfo.GetMonthName(x.Date.Month) + " " + x.Date.Year);

            RelatedTransactions =
                RelatedTransactions.OrderByDescending(x => ((FinancialTransaction) x.First()).Date).ToList();

            foreach (var list in RelatedTransactions) {
                list.Reverse();
            }
        }
    }
}