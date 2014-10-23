using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Models;
using QKit.JumpList;
using System.Collections.Generic;
using System.Globalization;

namespace MoneyManager.ViewModels
{
    internal class TransactionListUserControlViewModel : ViewModelBase
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

            foreach (var lists in RelatedTransactions)
            {
                lists.Reverse();
            }
        }
    }
}