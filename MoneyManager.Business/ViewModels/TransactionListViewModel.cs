#region

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using QKit.JumpList;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class TransactionListViewModel : ViewModelBase {
        private TransactionDataAccess transactionData {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private AccountDataAccess accountData {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public string Title {
            get { return accountData.SelectedAccount.Name; }
        }

        public List<JumpListGroup<FinancialTransaction>> RelatedTransactions { set; get; }

        public void SetRelatedTransactions(int accountId) {
            IEnumerable<FinancialTransaction> related = transactionData.GetRelatedTransactions(accountId);

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