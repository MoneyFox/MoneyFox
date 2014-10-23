using MoneyManager.DataAccess.Model;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class StatisticDataAccess
    {
        public ObservableCollection<StatisticItem> MonthlyOverview
        {
            get { return LoadMonthlyOverview(); }
        }

        private ObservableCollection<StatisticItem> LoadMonthlyOverview()
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                List<FinancialTransaction> transactionList = dbConn.Table<FinancialTransaction>().ToList()
                    .Where(x => x.Date.Month == DateTime.Now.Month).ToList();

                var itemList = new ObservableCollection<StatisticItem>();

                //TODO: refactor
                //var income = new StatisticItem
                //{
                //    Category = Translation.GetTranslation("IncomeLabel"),
                //    Value = transactionList.Where(x => x.Type == (int)TransactionType.Income).Sum(x => x.Amount)
                //};

                //var spent = new StatisticItem
                //{
                //    Category = Translation.GetTranslation("SpentLabel"),
                //    Value = transactionList.Where(x => x.Type == (int)TransactionType.Spending).Sum(x => x.Amount)
                //};

                //var increased = new StatisticItem
                //{
                //    Category = Translation.GetTranslation("IncreasedLabel"),
                //    Value = income.Value - spent.Value
                //};

                //itemList.Add(income);
                //itemList.Add(spent);
                //itemList.Add(increased);

                return itemList;
            }
        }
    }
}