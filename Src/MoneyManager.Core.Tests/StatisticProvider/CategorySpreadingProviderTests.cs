using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.StatisticProvider
{
    public class CategorySpreadingProviderTests
    {
        [Fact]
        public void GetValues_NullDependency_NullReferenceException()
        {
            Assert.Throws<NullReferenceException>(
                () => new CategorySpreadingProvider(null, null).GetValues(DateTime.Today, DateTime.Today));
        }

        [Fact]
        public void GetValues_InitializedData_IgnoreTransfers()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 2, Name = "Ausgehen"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = 1,
                    Type = (int) TransactionType.Income,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.Data.First(),
                    CategoryId = 1
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.Data.First(),
                    CategoryId = 1
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Transfer,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.Data.First(),
                    CategoryId = 1
                }
            });

            //Excution
            var result =
                new CategorySpreadingProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(30);
        }

        [Fact]
        public void GetValues_InitializedData_CalculateIncome()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"},
                new Category {Id = 2, Name = "Ausgehen"},
                new Category {Id = 3, Name = "Foo"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = 1,
                    Type = (int) TransactionType.Income,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.Data[1],
                    CategoryId = 2
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Income,
                    Date = DateTime.Today,
                    Amount = 66,
                    Category = categoryRepo.Data[2],
                    CategoryId = 3
                }
            });

            //Excution
            var result =
                new CategorySpreadingProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(2);
            result[0].Value.ShouldBe(30);
            result[1].Value.ShouldBe(40);
        }

        [Fact]
        public void GetValues_IncomeHigherThanSpending_SpendingSetToZero()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = 1,
                    Type = (int) TransactionType.Income,
                    Date = DateTime.Today,
                    Amount = 100,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                }
            });

            //Excution
            var result =
                new CategorySpreadingProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result[0].Value.ShouldBe(0);
        }

        [Fact]
        public void GetValues_InitializedData_HandleDateCorrectly()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"},
                new Category {Id = 2, Name = "Ausgehen"},
                new Category {Id = 3, Name = "Bier"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = 1,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today.AddDays(-5),
                    Amount = 60,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 90,
                    Category = categoryRepo.Data[1],
                    CategoryId = 2
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today.AddDays(5),
                    Amount = 40,
                    Category = categoryRepo.Data[2],
                    CategoryId = 3
                }
            });

            //Excution
            var result =
                new CategorySpreadingProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(90);
        }

        [Fact]
        public void GetValues_InitializedData_AddOtherItem()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"},
                new Category {Id = 2, Name = "Ausgehen"},
                new Category {Id = 3, Name = "Bier"},
                new Category {Id = 4, Name = "Wein"},
                new Category {Id = 5, Name = "Wodka"},
                new Category {Id = 6, Name = "Limoncella"},
                new Category {Id = 7, Name = "Spagetthi"},
                new Category {Id = 8, Name = "Tomaten"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = 1,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 10,
                    Category = categoryRepo.Data[0],
                    CategoryId = 1
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 20,
                    Category = categoryRepo.Data[1],
                    CategoryId = 2
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 30,
                    Category = categoryRepo.Data[2],
                    CategoryId = 3
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 40,
                    Category = categoryRepo.Data[3],
                    CategoryId = 4
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 50,
                    Category = categoryRepo.Data[4],
                    CategoryId = 5
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 60,
                    Category = categoryRepo.Data[5],
                    CategoryId = 6
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 70,
                    Category = categoryRepo.Data[6],
                    CategoryId = 7
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Type = (int) TransactionType.Spending,
                    Date = DateTime.Today,
                    Amount = 80,
                    Category = categoryRepo.Data[7],
                    CategoryId = 8
                }
            });

            //Excution
            var result =
                new CategorySpreadingProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3),
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