using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.Business.Src
{
    [ImplementPropertyChanged]
    public class StatisticLogic
    {
        private IEnumerable<FinancialTransaction> allTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().AllTransactions; }
        }

        public ObservableCollection<StatisticItem> MonthlyCashFlow
        {
            get { return LoadMonthlyCashFlow(); }
        }

        public ObservableCollection<StatisticItem> LoadMonthlyCashFlow()
        {
            var transactionList = allTransaction
                .Where(x => x.Date.Month == DateTime.Now.Month)
                .ToList();

            var itemList = new ObservableCollection<StatisticItem>();

            var income = new StatisticItem
            {
                Category = Translation.GetTranslation("IncomeLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Income).Sum(x => x.Amount)
            };

            var spent = new StatisticItem
            {
                Category = Translation.GetTranslation("SpentLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Spending).Sum(x => x.Amount)
            };

            var increased = new StatisticItem
            {
                Category = Translation.GetTranslation("IncreasedLabel"),
                Value = income.Value - spent.Value
            };

            itemList.Add(income);
            itemList.Add(spent);
            itemList.Add(increased);

            return itemList;
        }
    }
}