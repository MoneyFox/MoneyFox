using System;
using System.Linq;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    public class TransactionDataAccessTest
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void TransactionDataAccess_CrudTransaction()
        {
            var transactionDataAccess =
                new TransactionDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()));

            const double firstAmount = 76.30;
            const double secondAmount = 22.90;

            var transaction = new FinancialTransaction
            {
                ChargedAccount = new Account {Id = 4},
                Amount = firstAmount,
                Date = DateTime.Today,
                Note = "this is a note!!!",
                IsCleared = false
            };

            transactionDataAccess.SaveItem(transaction);

            var list = transactionDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(firstAmount, list.First().Amount);

            transaction.Amount = secondAmount;

            transactionDataAccess.SaveItem(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(secondAmount, list.First().Amount);

            transactionDataAccess.DeleteItem(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();
            Assert.False(list.Any());
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void TransactionDataAccess_CrudTransactionWithoutAccount()
        {
            var transactionDataAccess =
                new TransactionDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()));

            const double firstAmount = 76.30;
            const double secondAmount = 22.90;

            var transaction = new FinancialTransaction
            {
                Amount = firstAmount,
                Date = DateTime.Today,
                Note = "this is a note!!!",
                IsCleared = false
            };

            transactionDataAccess.SaveItem(transaction);

            var list = transactionDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(firstAmount, list.First().Amount);

            transaction.Amount = secondAmount;

            transactionDataAccess.SaveItem(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(secondAmount, list.First().Amount);

            transactionDataAccess.DeleteItem(transaction);

            transactionDataAccess.LoadList();
            list = transactionDataAccess.LoadList();
            Assert.False(list.Any());
        }
    }
}