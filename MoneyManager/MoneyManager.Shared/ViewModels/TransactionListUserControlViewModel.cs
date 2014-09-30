using System.Collections.Generic;
using System.Globalization;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using QKit.JumpList;

namespace MoneyManager.ViewModels
{
    public class TransactionListUserControlViewModel : ViewModelBase
    {
        private TransactionDataAccess TransactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public List<JumpListGroup<FinancialTransaction>> RelatedTransactions
        {
            get
            {
                var dateInfo = new DateTimeFormatInfo();
                return TransactionData.RelatedTransactions.ToGroups(x => x.Date, x => dateInfo.GetMonthName(x.Date.Month));
            }
        }
    }
}
