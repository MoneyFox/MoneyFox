#region

using System.Collections.Generic;
using System.Globalization;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using PropertyChanged;
using QKit.JumpList;

#endregion

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class TransactionListViewModel : ViewModelBase
    {
        private TransactionDataAccess TransactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public List<JumpListGroup<FinancialTransaction>> RelatedTransactions { set; get; }

        public void SetRelatedTransactions(int accountId)
        {
            var related = TransactionData.GetRelatedTransactions(accountId);

            var dateInfo = new DateTimeFormatInfo();
            RelatedTransactions = related.ToGroups(x => x.Date, x => dateInfo.GetMonthName(x.Date.Month));

            foreach (var list in RelatedTransactions)
            {
                list.Reverse();
            }
        }
    }
}