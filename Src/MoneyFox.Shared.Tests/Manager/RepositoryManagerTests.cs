using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using Moq;
using System.Collections.ObjectModel;

namespace MoneyFox.Shared.Tests.Manager
{
    [TestClass]
    public class RepositoryManagerTests
    {
        [TestMethod]
        public void Constructor_NullValues_NoException()
        {
            new RepositoryManager(null, null).ShouldNotBeNull();
        }

        [TestMethod]
        public void ReloadData_CollectionNull_CollectionInstantiated()
        {
            var accountsLoaded = false;
            var paymentsLoaded = false;
            var categoryLoaded = false;

            var accountRepoSetup = new Mock<IRepository<Account>>();
            accountRepoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());
            accountRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()))
                .Callback(() => accountsLoaded = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Payment>());
            paymentRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Callback(() => paymentsLoaded = true);

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()))
                .Callback(() => categoryLoaded = true);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.AccountRepository).Returns(accountRepoSetup .Object);
            unitOfWork.SetupGet(x => x.PaymentRepository).Returns(paymentRepoSetup.Object);
            unitOfWork.SetupGet(x => x.CategoryRepository).Returns(categoryRepoSetup.Object);

            new RepositoryManager(unitOfWork.Object,
                new PaymentManager(unitOfWork.Object,
                    new Mock<IDialogService>().Object)).ReloadData();

            Assert.IsTrue(accountsLoaded);
            Assert.IsTrue(paymentsLoaded);
            Assert.IsTrue(categoryLoaded);
        }
    }
}