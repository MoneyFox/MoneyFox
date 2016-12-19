using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using Moq;

namespace MoneyFox.Shared.Tests.StatisticProvider
{
    [TestClass]
    public class CashFlowProviderTests
    {
        [TestMethod]
        public void Constructor_Null_NotNullObject()
        {
            new CashFlowDataProvider(null).ShouldNotBeNull();
        }

        [TestMethod]
        public void GetValues_SetupData_ListWithoutTransfer()
        {
            //Setup
            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>())).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 50
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = PaymentType.Transfer,
                    Date = DateTime.Today,
                    Amount = 40
                }
            });

            //Excution
            var result = new CashFlowDataProvider(paymentRepoSetup.Object).GetValues(DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(3));

            //Assertion
            result.Income.Value.ShouldBe(60);
            result.Expense.Value.ShouldBe(50);
            result.Revenue.Value.ShouldBe(10);
        }

        [TestMethod]
        public void GetValues_SetupData_CalculatedCorrectTimeRange()
        {
            var paymentList = new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 60
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(5),
                    Amount = 50
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(-5),
                    Amount = 40
                }
            };

            //Setup
            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>()))
                .Returns((Expression<Func<PaymentViewModel, bool>> filter) => paymentList.Where(filter.Compile()).ToList());

            //Excution
            var result = new CashFlowDataProvider(paymentRepoSetup.Object).GetValues(DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(3));

            //Assertion
            result.Income.Value.ShouldBe(0);
            result.Expense.Value.ShouldBe(60);
            result.Revenue.Value.ShouldBe(-60);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetValues_NullDependency_NullReferenceException()
        {
            new CashFlowDataProvider(null).GetValues(DateTime.Today, DateTime.Today);
        }
    }
}