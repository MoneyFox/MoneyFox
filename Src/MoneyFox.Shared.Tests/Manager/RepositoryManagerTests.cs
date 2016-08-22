using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using Moq;

namespace MoneyFox.Shared.Tests.Manager
{
    [TestClass]
    public class RepositoryManagerTests
    {
        [TestMethod]
        public void Constructor_NullValues_NoException()
        {
            new RepositoryManager(null, null, null, null).ShouldNotBeNull();
        }

        [TestMethod]
        public void ReloadData_CollectionNull_CollectionInstantiated()
        {
            var accountsLoaded = false;
            var paymentsLoaded = false;
            var categoryLoaded = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.Setup(x => x.GetList(null)).Returns(new List<Account>());
            accountRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()))
                .Callback(() => accountsLoaded = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(null)).Returns(new List<Payment>());
            paymentRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Callback(() => paymentsLoaded = true);

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(null)).Returns(new List<Category>());
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()))
                .Callback(() => categoryLoaded = true);

            new RepositoryManager(new PaymentManager(paymentRepoSetup.Object, accountRepoSetup.Object,
                    new Mock<IRecurringPaymentRepository>().Object, 
                    new Mock<IDialogService>().Object),
                    accountRepoSetup.Object, paymentRepoSetup.Object, categoryRepoSetup.Object)
                    .ReloadData();

            Assert.IsTrue(accountsLoaded);
            Assert.IsTrue(paymentsLoaded);
            Assert.IsTrue(categoryLoaded);
        }
    }
}