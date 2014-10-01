using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;

namespace MoneyManager.WindowsPhone.Test.ViewModels
{
    [TestClass]
    public class AccountDataAccessTest
    {
        private Account account;

        private AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        [TestInitialize]
        public async Task InitTests()
        {
            await DatabaseHelper.CreateDatabase();
            AccountDataAccess.Save(account);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.DeleteAll<Account>();
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
            AccountDataAccess.Save(account);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                var saved = dbConn.Table<Account>().Where(x => x.Name == account.Name).ToList().First();
                Assert.IsTrue(saved.Name == account.Name && saved.CurrentBalance == account.CurrentBalance
                    && saved.Iban == account.Iban && saved.Note == account.Note);
            }
        }

        [TestMethod]
        public void AddTransactionAmountTest()
        {
            AccountDataAccess.Save(account);

            var transaction = new FinancialTransaction
            {
                ChargedAccountId = account.Id,
                Amount = 20,
                Date = DateTime.Today,
                Note = "this is a note!!!"
            };
            AccountDataAccess.AddTransactionAmount(transaction);
            Assert.AreEqual(account.CurrentBalance, 40);
        }

        [TestMethod]
        public void LoadAccountListTest()
        {
            AccountDataAccess.Save(account);
            AccountDataAccess.Save(account);
            Assert.AreEqual(AccountDataAccess.AllAccounts.Count, 2);

            AccountDataAccess.AllAccounts = null;
            AccountDataAccess.LoadList();
            Assert.AreEqual(AccountDataAccess.AllAccounts.Count, 2);
        }

        [TestMethod]
        public void UpateAccountTest()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AccountDataAccess.Save(account);
                Assert.AreEqual(AccountDataAccess.AllAccounts.Count, 1);

                string newName = "This is a new Name";

                account = dbConn.Table<Account>().First();
                account.Name = newName;
                AccountDataAccess.Update(account);

                Assert.AreEqual(newName, dbConn.Table<Account>().First().Name);
            }
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            AccountDataAccess.Save(account);
            Assert.IsTrue(AccountDataAccess.AllAccounts.Contains(account));

            AccountDataAccess.Delete(account, true);
            Assert.IsFalse(AccountDataAccess.AllAccounts.Contains(account));
        }
    }
}