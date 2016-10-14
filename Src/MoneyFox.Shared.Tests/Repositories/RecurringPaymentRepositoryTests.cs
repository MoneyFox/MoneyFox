using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
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
            var dataAccessMock = new Mock<IDataAccess<RecurringPayment>>();
            var testPayment = new RecurringPayment {Id = 100, Amount = 78};

            dataAccessMock.Setup(x => x.LoadList(null))
                .Returns(new List<RecurringPayment> {testPayment});

            Assert.AreEqual(testPayment, new RecurringPaymentRepository(dataAccessMock.Object).FindById(100));
        }
    }
}