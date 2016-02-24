using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;

namespace MoneyManager.Core.Tests.StatisticProvider
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
            paymentRepoSetup.SetupAllProperties();

            var paymentRepository = paymentRepoSetup.Object;
            paymentRepository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60
                },
                new Payment
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 50
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Transfer,
                    Date = DateTime.Today,
                    Amount = 40
                }
            });

            //Excution
            var result = new CashFlowDataProvider(paymentRepository).GetValues(DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(3));

            //Assertion
            result.Income.Value.ShouldBe(60);
            result.Spending.Value.ShouldBe(50);
            result.Revenue.Value.ShouldBe(10);
        }

        [TestMethod]
        public void GetValues_SetupData_CalculatedCorrectTimeRange()
        {
            //Setup
            var paymentRepositorySetup = new Mock<IPaymentRepository>();
            paymentRepositorySetup.SetupAllProperties();

            var paymentRepository = paymentRepositorySetup.Object;
            paymentRepository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 60
                },
                new Payment
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(5),
                    Amount = 50
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(-5),
                    Amount = 40
                }
            });

            //Excution
            var result = new CashFlowDataProvider(paymentRepository).GetValues(DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(3));

            //Assertion
            result.Income.Value.ShouldBe(0);
            result.Spending.Value.ShouldBe(60);
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