using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class accountViewModelTest
    {
        private Account account;

        private accountViewModel accountViewModel
        {
            get { return new ViewModelLocator().accountViewModel; }
        }

        [TestInitialize]
        public async Task InitTests()
        {
            accountViewModel.Save(account);
            await DatabaseHelper.CreateDatabase();

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
            accountViewModel.Save(account);

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
            accountViewModel.Save(account);

            var transaction = new FinancialTransaction
            {
                ChargedAccountId = account.Id,
                Amount = 20,
                Date = DateTime.Today,
                Note = "this is a note!!!"
            };
            accountViewModel.AddTransactionAmount(transaction);
            Assert.AreEqual(account.CurrentBalance, 40);
        }

        [TestMethod]
        public void LoadAccountListTest()
        {
            accountViewModel.Save(account);
            accountViewModel.Save(account);
            Assert.AreEqual(accountViewModel.AllAccounts.Count, 2);

            accountViewModel.AllAccounts = null;
            accountViewModel.LoadList();
            Assert.AreEqual(accountViewModel.AllAccounts.Count, 2);
        }

        [TestMethod]
        public void UpateAccountTest()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                accountViewModel.Save(account);
                Assert.AreEqual(accountViewModel.AllAccounts.Count, 1);

                string newName = "This is a new Name";

                account = dbConn.Table<Account>().First();
                account.Name = newName;
                accountViewModel.Update(account);

                Assert.AreEqual(newName, dbConn.Table<Account>().First().Name);
            }
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            accountViewModel.Save(account);
            Assert.IsTrue(accountViewModel.AllAccounts.Contains(account));

            accountViewModel.Delete(account);
            Assert.IsFalse(accountViewModel.AllAccounts.Contains(account));
        }
    }
}