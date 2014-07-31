using MoneyTracker.Models;
using MoneyTracker.Src;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyTracker.ViewModels
{
    public class StatisticDAO
    {
        public ObservableCollection<StatisticItem> MonthlyOverview
        {
            get { return LoadMonthlyOverview(); }
        }

        private ObservableCollection<StatisticItem> LoadMonthlyOverview()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                var transactionList = dbConn.Table<FinancialTransaction>().ToList()
                    .Where(x => x.Date.Month == DateTime.Now.Month).ToList();

                var itemList = new ObservableCollection<StatisticItem>();

                var income = new StatisticItem
                {
                    Category = Utilities.GetTranslation("IncomeLabel"),
                    Value = transactionList.Where(x => x.Type == (int)TransactionType.Income).Sum(x => x.Amount)
                };

                var spent = new StatisticItem
                {
                    Category = Utilities.GetTranslation("SpentLabel"),
                    Value = -transactionList.Where(x => x.Type == (int)TransactionType.Spending).Sum(x => x.Amount)
                };

                var increased = new StatisticItem
                {
                    Category = Utilities.GetTranslation("IncreasedLabel"),
                    Value = income.Value - spent.Value
                };

                itemList.Add(income);
                itemList.Add(spent);
                itemList.Add(increased);

                return itemList;
            }
        }
    }
}