using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Business.Manager;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
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
            accountRepoSetup.Setup(x => x.GetList(null)).Returns(new List<AccountViewModel>());
            accountRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()))
                .Callback(() => accountsLoaded = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(null)).Returns(new List<PaymentViewModel>());
            paymentRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<PaymentViewModel, bool>>>()))
                .Callback(() => paymentsLoaded = true);

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(null)).Returns(new List<CategoryViewModel>());
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<CategoryViewModel, bool>>>()))
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