using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Shared.Repositories;
using Moq;

namespace MoneyFox.Shared.Tests.Repositories
{
    [TestClass]
    public class RecurringPaymentRepositoryTests
    {
        [TestMethod]
        public void FindById_ReturnsPayment()
        {
            var dataAccessMock = new Mock<IDataAccess<RecurringPaymentViewModel>>();
            var testPayment = new RecurringPaymentViewModel {Id = 100, Amount = 78};

            dataAccessMock.Setup(x => x.LoadList(null))
                .Returns(new List<RecurringPaymentViewModel> {testPayment});

            Assert.AreEqual(testPayment, new RecurringPaymentRepository(dataAccessMock.Object).FindById(100));
        }
    }
}