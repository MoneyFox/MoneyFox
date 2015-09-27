using System;
using System.Linq;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;
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
                new TransactionDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()));

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
                new TransactionDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()));

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