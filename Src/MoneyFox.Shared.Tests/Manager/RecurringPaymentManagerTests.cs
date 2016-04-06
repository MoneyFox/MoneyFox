using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;

namespace MoneyManager.Core.Tests.Manager
{
    [TestClass]
    public class RecurringPaymentManagerTests
    {
        [TestMethod]
        public void CheckRecurringPayments_None_NewEntryForRecurring()
        {
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Amount = 99,
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    Date = DateTime.Now.AddDays(-3),
                    RecurringPaymentId = 3,
                    RecurringPayment = new RecurringPayment
                    {
                        Id = 3,
                        Recurrence = (int) PaymentRecurrence.Daily,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 95
                    },
                    IsCleared = true,
                    IsRecurring = true
                },
                new Payment
                {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-3),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment
                    {
                        Id = 4,
                        Recurrence = (int) PaymentRecurrence.Weekly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<Payment>()))
                .Callback((Payment payment) => resultList.Add(payment));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            resultList.Any().ShouldBeTrue();
            resultList.First().Amount.ShouldBe(95);
            resultList.First().ChargedAccountId.ShouldBe(2);
            resultList.First().RecurringPaymentId.ShouldBe(3);
        }
    }
}