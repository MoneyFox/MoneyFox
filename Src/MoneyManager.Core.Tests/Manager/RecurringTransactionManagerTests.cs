using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Manager
{
    public class RecurringTransactionManagerTests
    {
        [Fact]
        public void CheckRecurringTransactions()
        {
            var repoSetup = new Mock<ITransactionRepository>();
            var resultList = new List<FinancialTransaction>();

            var testList = new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = 1,
                    Amount = 99,
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    Date = DateTime.Now.AddDays(-3),
                    ReccuringTransactionId = 3,
                    RecurringTransaction = new RecurringTransaction
                    {
                        Id = 3,
                        Recurrence = (int) TransactionRecurrence.Daily,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 95
                    },
                    IsRecurring = true
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-3),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    ReccuringTransactionId = 4,
                    RecurringTransaction = new RecurringTransaction
                    {
                        Id = 4,
                        Recurrence = (int) TransactionRecurrence.Weekly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<FinancialTransaction>()))
                .Callback((FinancialTransaction transaction) => resultList.Add(transaction));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<FinancialTransaction>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            new RecurringTransactionManager(repoSetup.Object).CheckRecurringTransactions();

            resultList.Any().ShouldBeTrue();
            resultList.First().Amount.ShouldBe(95);
            resultList.First().ChargedAccountId.ShouldBe(2);
            resultList.First().ReccuringTransactionId.ShouldBe(3);
        }
    }
}