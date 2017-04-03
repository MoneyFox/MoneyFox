using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Tests;
using Moq;
using Xunit;

namespace MoneyFox.Shared.Tests.StatisticProvider
{
    public class CategorySummaryProviderTests
    {
        [Fact]
        public void GetValues_NullDependency_NullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => new CategorySummaryDataProvider(null, null).GetValues(DateTime.Today, DateTime.Today));
        }

        [Fact]
        public void GetValues_InitializedData_IgnoreTransfers()
        {
            //Setup

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(null)).Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel {Id = 1, Name = "Ausgehen"}
            });

            var categoryRepo = categoryRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(null)).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.GetList().First(),
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.GetList().First(),
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = PaymentType.Transfer,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.GetList().First(),
                    CategoryId = 1
                }
            });

            //Excution
            var result =
                new CategorySummaryDataProvider(paymentRepoSetup.Object, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(-30);
        }

        [Fact]
        public void GetValues_InitializedData_CalculateIncome()
        {
            //Setup
            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(null)).Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel {Id = 1, Name = "Einkaufen"},
                new CategoryViewModel {Id = 2, Name = "Ausgehen"},
                new CategoryViewModel {Id = 3, Name = "Foo"}
            });

            var categoryRepo = categoryRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>())).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.GetList().ToList()[0],
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.GetList().ToList()[0],
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.GetList().ToList()[1],
                    CategoryId = 2
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 66,
                    Category = categoryRepo.GetList().ToList()[2],
                    CategoryId = 3
                }
            });

            //Excution
            var result =
                new CategorySummaryDataProvider(paymentRepoSetup.Object, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(3);
            result[0].Value.ShouldBe(-40);
            result[1].Value.ShouldBe(-30);
            result[2].Value.ShouldBe(66);
        }

        [Fact]
        public void GetValues_InitializedData_HandleDateCorrectly()
        {
            //Setup

            var categoryRepoSetup = new Mock<ICategoryRepository>();

            categoryRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<CategoryViewModel, bool>>>())).Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel {Id = 1, Name = "Einkaufen"},
                new CategoryViewModel {Id = 2, Name = "Ausgehen"},
                new CategoryViewModel {Id = 3, Name = "Bier"}
            });
            var categoryRepo = categoryRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>())).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(-5),
                    Amount = 60,
                    Category = categoryRepo.GetList().ToList()[0],
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.GetList().ToList()[1],
                    CategoryId = 2
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(5),
                    Amount = 40,
                    Category = categoryRepo.GetList().ToList()[2],
                    CategoryId = 3
                }
            });

            //Excution
            var result =
                new CategorySummaryDataProvider(paymentRepoSetup.Object, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(-90);
        }
    }
}