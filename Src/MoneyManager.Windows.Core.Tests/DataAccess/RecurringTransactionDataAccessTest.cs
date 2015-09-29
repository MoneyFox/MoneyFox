using System;
using System.Linq;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    public class RecurringTransactionDataAccessTest
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void RecurringTransactionDataAccess_CrudRecurringTransaction()
        {
            var recurringTransactionDataAccess =
                new RecurringTransactionDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()));

            const double firstAmount = 100.70;
            const double secondAmount = 80.45;

            var transaction = new RecurringTransaction
            {
                ChargedAccountId = 7,
                Amount = firstAmount,
                StartDate = DateTime.Today,
                EndDate = DateTime.Now.AddDays(7),
                Note = "this is a note!!!"
            };

            recurringTransactionDataAccess.SaveItem(transaction);


            var list = recurringTransactionDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(firstAmount, list.First().Amount);

            transaction.Amount = secondAmount;

            recurringTransactionDataAccess.SaveItem(transaction);

            recurringTransactionDataAccess.LoadList();
            list = recurringTransactionDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(secondAmount, list.First().Amount);

            recurringTransactionDataAccess.DeleteItem(transaction);

            recurringTransactionDataAccess.LoadList();
            list = recurringTransactionDataAccess.LoadList();
            Assert.False(list.Any());
        }
    }
}