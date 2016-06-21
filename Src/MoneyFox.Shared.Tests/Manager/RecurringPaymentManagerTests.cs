using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using Moq;

namespace MoneyFox.Shared.Tests.Manager {
    [TestClass]
    public class RecurringPaymentManagerTests {
        [TestMethod]
        public void CheckRecurringPayments_NewEntryForDailyRecurring() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 1,
                    Amount = 99,
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    Date = DateTime.Now.AddDays(-3),
                    RecurringPaymentId = 3,
                    RecurringPayment = new RecurringPayment {
                        Id = 3,
                        Recurrence = (int) PaymentRecurrence.Daily,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 95
                    },
                    IsCleared = true,
                    IsRecurring = true
                },
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-3),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
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

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(1);
            resultList.First().Amount.ShouldBe(95);
            resultList.First().ChargedAccountId.ShouldBe(2);
            resultList.First().RecurringPaymentId.ShouldBe(3);
            resultList.First().RecurringPayment.ShouldNotBeNull();
            resultList.First().RecurringPayment.Recurrence.ShouldBe((int) PaymentRecurrence.Daily);
        }

        [TestMethod]
        public void CheckRecurringPayments_NewEntryForWeeklyRecurring() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-7),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
                        Id = 4,
                        Recurrence = (int) PaymentRecurrence.Weekly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsCleared = true,
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<Payment>()))
                .Callback((Payment payment) => resultList.Add(payment));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(1);

            resultList[0].Amount.ShouldBe(105);
            resultList[0].ChargedAccountId.ShouldBe(2);
            resultList[0].RecurringPaymentId.ShouldBe(4);
            resultList[0].RecurringPayment.ShouldNotBeNull();
            resultList[0].RecurringPayment.Recurrence.ShouldBe((int) PaymentRecurrence.Weekly);
        }

        [TestMethod]
        public void CheckRecurringPayments_NewEntryForBiweeklyRecurring() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-14),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
                        Id = 4,
                        Recurrence = (int) PaymentRecurrence.Biweekly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsCleared = true,
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<Payment>()))
                .Callback((Payment payment) => resultList.Add(payment));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(1);

            resultList[0].Amount.ShouldBe(105);
            resultList[0].ChargedAccountId.ShouldBe(2);
            resultList[0].RecurringPaymentId.ShouldBe(4);
            resultList[0].RecurringPayment.ShouldNotBeNull();
            resultList[0].RecurringPayment.Recurrence.ShouldBe((int) PaymentRecurrence.Biweekly);
        }

        [TestMethod]
        public void CheckRecurringPayments_NewEntryForMonthlyRecurring() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddMonths(-1),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
                        Id = 4,
                        Recurrence = (int) PaymentRecurrence.Monthly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsCleared = true,
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<Payment>()))
                .Callback((Payment payment) => resultList.Add(payment));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(1);

            resultList[0].Amount.ShouldBe(105);
            resultList[0].ChargedAccountId.ShouldBe(2);
            resultList[0].RecurringPaymentId.ShouldBe(4);
            resultList[0].RecurringPayment.ShouldNotBeNull();
            resultList[0].RecurringPayment.Recurrence.ShouldBe((int) PaymentRecurrence.Monthly);
        }

        [TestMethod]
        public void CheckRecurringPayments_NewEntryForYearlyRecurring() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddYears(-1),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
                        Id = 4,
                        Recurrence = (int) PaymentRecurrence.Yearly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsCleared = true,
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<Payment>()))
                .Callback((Payment payment) => resultList.Add(payment));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(1);

            resultList[0].Amount.ShouldBe(105);
            resultList[0].ChargedAccountId.ShouldBe(2);
            resultList[0].RecurringPaymentId.ShouldBe(4);
            resultList[0].RecurringPayment.ShouldNotBeNull();
            resultList[0].RecurringPayment.Recurrence.ShouldBe((int) PaymentRecurrence.Yearly);
        }

        [TestMethod]
        public void CheckRecurringPayments_NewEntryForDailyAndWeeklyRecurring() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 1,
                    Amount = 99,
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    Date = DateTime.Now.AddDays(-1),
                    RecurringPaymentId = 3,
                    RecurringPayment = new RecurringPayment {
                        Id = 3,
                        Recurrence = (int) PaymentRecurrence.Daily,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 95
                    },
                    IsCleared = true,
                    IsRecurring = true
                },
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-7),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
                        Id = 4,
                        Recurrence = (int) PaymentRecurrence.Weekly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsCleared = true,
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<Payment>()))
                .Callback((Payment payment) => resultList.Add(payment));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(2);

            resultList[0].Amount.ShouldBe(95);
            resultList[0].ChargedAccountId.ShouldBe(2);
            resultList[0].RecurringPaymentId.ShouldBe(3);
            resultList[0].RecurringPayment.ShouldNotBeNull();
            resultList[0].RecurringPayment.Recurrence.ShouldBe((int) PaymentRecurrence.Daily);

            resultList[1].Amount.ShouldBe(105);
            resultList[1].ChargedAccountId.ShouldBe(2);
            resultList[1].RecurringPaymentId.ShouldBe(4);
            resultList[1].RecurringPayment.ShouldNotBeNull();
            resultList[1].RecurringPayment.Recurrence.ShouldBe((int) PaymentRecurrence.Weekly);
        }

        [TestMethod]
        public void CheckRecurringPayments_IgnorePaymentsNotReady() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-3),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
                        Id = 4,
                        Recurrence = (int) PaymentRecurrence.Weekly,
                        ChargedAccountId = 2,
                        ChargedAccount = new Account {Id = 2},
                        Amount = 105
                    },
                    IsCleared = true,
                    IsRecurring = true
                }
            };

            repoSetup.Setup(x => x.Save(It.IsAny<Payment>()))
                .Callback((Payment payment) => resultList.Add(payment));

            repoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>(testList));

            repoSetup.Setup(x => x.LoadRecurringList(null)).Returns(testList);

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(0);
        }

        [TestMethod]
        public void CheckRecurringPayments_IgnoreNotClearedPaymentsNotReady() {
            //Setup
            var repoSetup = new Mock<IPaymentRepository>();
            var resultList = new List<Payment>();

            var testList = new List<Payment> {
                new Payment {
                    Id = 2,
                    Amount = 105,
                    Date = DateTime.Now.AddDays(-8),
                    ChargedAccountId = 2,
                    ChargedAccount = new Account {Id = 2},
                    RecurringPaymentId = 4,
                    RecurringPayment = new RecurringPayment {
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

            //Execution
            new RecurringPaymentManager(repoSetup.Object, new Mock<IAccountRepository>().Object).CheckRecurringPayments();

            //Assertion
            resultList.Count.ShouldBe(0);
        }
    }
}