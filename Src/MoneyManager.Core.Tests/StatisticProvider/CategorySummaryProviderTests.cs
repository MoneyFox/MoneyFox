using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;
using Assert = Xunit.Assert;

namespace MoneyManager.Core.Tests.StatisticProvider
{
    [TestClass]
    public class CategorySummaryProviderTests
    {
        [TestMethod]
        public void GetValues_NullDependency_NullReferenceException()
        {
            Assert.Throws<NullReferenceException>(
                () => new CategorySummaryDataProvider(null, null).GetValues(DateTime.Today, DateTime.Today));
        }

        [TestMethod]
        public void GetValues_InitializedData_IgnoreTransfers()
        {
            //Setup
            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Ausgehen"}
            });

            var paymentRepository = paymentRepoSetup.Object;
            paymentRepository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.Data.First(),
                    CategoryId = 1
                },
                new Payment
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.Data.First(),
                    CategoryId = 1
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Transfer,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.Data.First(),
                    CategoryId = 1
                }
            });

            //Excution
            var result =
                new CategorySummaryDataProvider(paymentRepository, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(-30);
        }

        [TestMethod]
        public void GetValues_InitializedData_CalculateIncome()
        {
            //Setup
            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"},
                new Category {Id = 2, Name = "Ausgehen"},
                new Category {Id = 3, Name = "Foo"}
            });

            var paymentRepository = paymentRepoSetup.Object;
            paymentRepository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new Payment
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.Data[1],
                    CategoryId = 2
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 66,
                    Category = categoryRepo.Data[2],
                    CategoryId = 3
                }
            });

            //Excution
            var result =
                new CategorySummaryDataProvider(paymentRepository, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(3);
            result[0].Value.ShouldBe(-40);
            result[1].Value.ShouldBe(-30);
            result[2].Value.ShouldBe(66);
        }

        [TestMethod]
        public void GetValues_InitializedData_HandleDateCorrectly()
        {
            //Setup
            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"},
                new Category {Id = 2, Name = "Ausgehen"},
                new Category {Id = 3, Name = "Bier"}
            });

            var paymentRepository = paymentRepoSetup.Object;
            paymentRepository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(-5),
                    Amount = 60,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new Payment
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.Data[1],
                    CategoryId = 2
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(5),
                    Amount = 40,
                    Category = categoryRepo.Data[2],
                    CategoryId = 3
                }
            });

            //Excution
            var result =
                new CategorySummaryDataProvider(paymentRepository, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(-90);
        }
    }
}