using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.WindowsPhone.Test.ViewModels
{
    [TestClass]
    public class AccountDataAccessTest
    {
        private Account account;

        private AccountDataAccess accountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        [TestInitialize]
        public async Task InitTests()
        {
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.DeleteAll<Account>();
                if (accountDataAccess.AllAccounts != null)
                {
                    accountDataAccess.AllAccounts.Clear();
                }
            }

            account = new Account
            {
                CurrentBalance = 20,
                Iban = "CHF20 0000 00000 000000",
                Name = "Foo",
                Note = "this is a note"
            };
        }

        [TestMethod]
        public void SaveAccount()
        {
            accountDataAccess.Save(account);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                var saved = dbConn.Table<Account>().Where(x => x.Name == account.Name).ToList().First();
                Assert.IsTrue(saved.Name == account.Name && saved.CurrentBalance == account.CurrentBalance
                    && saved.Iban == account.Iban && saved.Note == account.Note);
            }
        }

        [TestMethod]
        public void AddTransactionAmountIncomeTest()
        {
            accountDataAccess.Save(account);

            var transaction = new FinancialTransaction
            {
                ChargedAccountId = account.Id,
                Amount = 20,
                Date = DateTime.Today,
                Type = (int)TransactionType.Income,
                Note = "this is a note!!!"
            };
            accountDataAccess.AddTransactionAmount(transaction);
            Assert.AreEqual(40, transaction.ChargedAccount.CurrentBalance);
        }

        [TestMethod]
        public void AddTransactionAmountSpendingTest()
        {
            accountDataAccess.Save(account);

            var transaction = new FinancialTransaction
            {
                ChargedAccountId = account.Id,
                Amount = 10,
                Date = DateTime.Today,
                Type = (int)TransactionType.Spending,
                Note = "this is a note!!!"
            };
            accountDataAccess.AddTransactionAmount(transaction);
            Assert.AreEqual(10, transaction.ChargedAccount.CurrentBalance);
        }

        [TestMethod]
        public void AddTransactionAmountTransferTest()
        {
            accountDataAccess.Save(account);

            var transaction = new FinancialTransaction
            {
                ChargedAccountId = account.Id,
                TargetAccountId = account.Id,
                Amount = 10,
                Date = DateTime.Today,
                Type = (int)TransactionType.Transfer,
                Note = "this is a note!!!"
            };
            accountDataAccess.AddTransactionAmount(transaction);
            Assert.AreEqual(20, transaction.ChargedAccount.CurrentBalance);
        }

        [TestMethod]
        public void LoadAccountListTest()
        {
            accountDataAccess.Save(account);
            accountDataAccess.Save(account);
            Assert.AreEqual(accountDataAccess.AllAccounts.Count, 2);

            accountDataAccess.AllAccounts = null;
            accountDataAccess.LoadList();
            Assert.AreEqual(accountDataAccess.AllAccounts.Count, 2);
        }

        [TestMethod]
        public void UpateAccountTest()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                accountDataAccess.Save(account);
                Assert.AreEqual(1, accountDataAccess.AllAccounts.Count);

                string newName = "This is a new Name";

                account = dbConn.Table<Account>().First();
                account.Name = newName;
                accountDataAccess.Update(account);

                Assert.AreEqual(newName, dbConn.Table<Account>().First().Name);
            }
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            accountDataAccess.Save(account);
            Assert.IsTrue(accountDataAccess.AllAccounts.Contains(account));

            accountDataAccess.Delete(account, true);
            Assert.IsFalse(accountDataAccess.AllAccounts.Contains(account));
        }
    }
}