using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using Moq;

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
        public void ReloadData_SelectedNotNull_SelectedSetToNull()
        {
            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var paymentRepository = paymentRepoSetup.Object;

            paymentRepository.Selected = new Payment();

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.PaymentRepository).Returns(paymentRepoSetup.Object);


            new RepositoryManager(unitOfWork.Object,
                new PaymentManager(unitOfWork.Object,new Mock<IDialogService>().Object))
                .ReloadData();

            Assert.IsNull(paymentRepository.Selected);
        }

        [TestMethod]
        public void ReloadData_CollectionNull_CollectionInstantiated()
        {
            var accountsLoaded = false;
            var paymentsLoaded = false;
            var categoryLoaded = false;

            var accountRepoSetup = new Mock<IAccountRepository>();
            accountRepoSetup.SetupAllProperties();
            accountRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()))
                .Callback(() => accountsLoaded = true);

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();
            paymentRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Callback(() => paymentsLoaded = true);

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.SetupAllProperties();
            categoryRepoSetup.Setup(x => x.Load(It.IsAny<Expression<Func<Category, bool>>>()))
                .Callback(() => categoryLoaded = true);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.AccountRepository).Returns(accountRepoSetup .Object);
            unitOfWork.SetupGet(x => x.PaymentRepository).Returns(paymentRepoSetup.Object);

            new RepositoryManager(unitOfWork.Object,
                new PaymentManager(unitOfWork.Object,
                    new Mock<IDialogService>().Object)).ReloadData();

            Assert.IsTrue(accountsLoaded);
            Assert.IsTrue(paymentsLoaded);
            Assert.IsTrue(categoryLoaded);
        }
    }
}