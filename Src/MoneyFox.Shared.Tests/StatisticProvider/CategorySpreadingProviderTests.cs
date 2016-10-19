using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.StatisticDataProvider;
using Moq;

namespace MoneyFox.Shared.Tests.StatisticProvider
{
    [TestClass]
    public class CategorySpreadingProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetValues_NullDependency_NullReferenceException()
        {
            new CategorySpreadingDataProvider(null).GetValues(DateTime.Today, DateTime.Today);
        }

        [TestMethod]
        public void GetValues_InitializedData_IgnoreTransfers()
        {
            //Setup
            var testCat = new CategoryViewModel {Id = 2, Name = "Ausgehen"};

            var paymentList = new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = testCat,
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = testCat,
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Transfer,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = testCat,
                    CategoryId = 1
                }
            };

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>()))
                .Returns((Expression<Func<PaymentViewModel, bool>> filter) => paymentList.Where(filter.Compile()).ToList());

            //Excution
            var result =
                new CategorySpreadingDataProvider(paymentRepoSetup.Object).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(30);
        }

        [TestMethod]
        public void GetValues_InitializedData_CalculateIncome()
        {
            //Setup

            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<CategoryViewModel, bool>>>())).Returns(new List<CategoryViewModel>
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
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
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
                    Category = categoryRepo.GetList().ToList()[0],
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.GetList().ToList()[1],
                    CategoryId = 2
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 66,
                    Category = categoryRepo.GetList().ToList()[2],
                    CategoryId = 3
                }
            });

            //Excution
            var result =
                new CategorySpreadingDataProvider(paymentRepoSetup.Object).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(2);
            result[0].Value.ShouldBe(40);
            result[1].Value.ShouldBe(30);
        }

        [TestMethod]
        public void GetValues_InitializedData_HandleDateCorrectly()
        {
            //Setup
            var testList = new List<CategoryViewModel>
            {
                new CategoryViewModel {Id = 1, Name = "Einkaufen"},
                new CategoryViewModel {Id = 2, Name = "Ausgehen"},
                new CategoryViewModel {Id = 3, Name = "Bier"}
            };

            var paymentList = new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(-5),
                    Amount = 60,
                    Category = testList[0],
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = testList[1],
                    CategoryId = 2
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today.AddDays(5),
                    Amount = 40,
                    Category = testList[2],
                    CategoryId = 3
                }
            };

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>()))
                .Returns((Expression<Func<PaymentViewModel, bool>> filter) => paymentList.Where(filter.Compile()).ToList());

            //Excution
            var result =
                new CategorySpreadingDataProvider(paymentRepoSetup.Object).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(90);
        }

        [TestMethod]
        public void GetValues_InitializedData_AddOtherItem()
        {
            //Setup
            var categoryRepoSetup = new Mock<ICategoryRepository>();
            categoryRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<CategoryViewModel, bool>>>())).Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel {Id = 1, Name = "Einkaufen"},
                new CategoryViewModel {Id = 2, Name = "Ausgehen"},
                new CategoryViewModel {Id = 3, Name = "Bier"},
                new CategoryViewModel {Id = 4, Name = "Wein"},
                new CategoryViewModel {Id = 5, Name = "Wodka"},
                new CategoryViewModel {Id = 6, Name = "Limoncella"},
                new CategoryViewModel {Id = 7, Name = "Spagetthi"},
                new CategoryViewModel {Id = 8, Name = "Tomaten"}
            });

            var categoryRepo = categoryRepoSetup.Object;

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>())).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 1,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 10,
                    Category = categoryRepo.GetList().ToList()[0],
                    CategoryId = 1
                },
                new PaymentViewModel
                {
                    Id = 2,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 20,
                    Category = categoryRepo.GetList().ToList()[1],
                    CategoryId = 2
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 30,
                    Category = categoryRepo.GetList().ToList()[2],
                    CategoryId = 3
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.GetList().ToList()[3],
                    CategoryId = 4
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 50,
                    Category = categoryRepo.GetList().ToList()[4],
                    CategoryId = 5
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.GetList().ToList()[5],
                    CategoryId = 6
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 70,
                    Category = categoryRepo.GetList().ToList()[6],
                    CategoryId = 7
                },
                new PaymentViewModel
                {
                    Id = 3,
                    Type = (int) PaymentType.Expense,
                    Date = DateTime.Today,
                    Amount = 80,
                    Category = categoryRepo.GetList().ToList()[7],
                    CategoryId = 8
                }
            });

            //Excution
            var result =
                new CategorySpreadingDataProvider(paymentRepoSetup.Object).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(7);
            result[0].Value.ShouldBe(80);
            result[1].Value.ShouldBe(70);
            result[2].Value.ShouldBe(60);
            result[3].Value.ShouldBe(50);
            result[4].Value.ShouldBe(40);
            result[5].Value.ShouldBe(30);
            result[6].Value.ShouldBe(30);
        }
    }
}