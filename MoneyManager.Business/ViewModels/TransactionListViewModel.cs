#region

using System.Collections.Generic;
using System.Globalization;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using QKit.JumpList;

#endregion

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class TransactionListViewModel : ViewModelBase
    {
        private TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private AccountDataAccess accountData
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public string Title
        {
            get { return accountData.SelectedAccount.Name; }
        }

        public List<JumpListGroup<FinancialTransaction>> RelatedTransactions { set; get; }

        public void SetRelatedTransactions(int accountId)
        {
            var related = transactionData.GetRelatedTransactions(accountId);

            var dateInfo = new DateTimeFormatInfo();
            RelatedTransactions = related.ToGroups(x => x.Date, x => dateInfo.GetMonthName(x.Date.Month) + " " + x.Date.Year);

            foreach (var list in RelatedTransactions)
            {
                list.Reverse();
            }
        }
    }
}