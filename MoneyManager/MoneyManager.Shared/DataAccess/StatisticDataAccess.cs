using MoneyManager.Models;
using MoneyManager.Src;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess
{
    public class StatisticDataAccess
    {
        public ObservableCollection<StatisticItem> MonthlyOverview
        {
            get
            {
                return LoadMonthlyOverview();
            }
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
                    Value = spent.Value + income.Value
                };

                itemList.Add(income);
                itemList.Add(spent);
                itemList.Add(increased);

                return itemList;
            }
        }
    }
}