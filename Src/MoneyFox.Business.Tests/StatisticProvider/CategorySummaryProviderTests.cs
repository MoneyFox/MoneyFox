using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using Moq;
using Xunit;

namespace MoneyFox.Business.Tests.StatisticProvider
{
    public class CategorySummaryProviderTests
    {
        [Fact]
        public async void GetValues_NullDependency_NullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(
                () => new CategorySummaryDataProvider(null).GetValues(DateTime.Today, DateTime.Today));
        }

        [Fact]
        public void GetValues_CorrectSums()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(x => x.GetAllCategoriesWithPayments())
                               .Returns(Task.FromResult<IEnumerable<Category>>(new List<Category>
                               {
                                   new Category
                                   {
                                       Data =
                                       {
                                           Id = 1,
                                           Name = "Ausgehen",
                                           Payments = new List<PaymentEntity>
                                           {
                                               new PaymentEntity
                                               {
                                                   Id = 1,
                                                   Type = PaymentType.Income,
                                                   Date = DateTime.Today,
                                                   Amount = 60,
                                                   CategoryId = 1
                                               },
                                               new PaymentEntity
                                               {
                                                   Id = 2,
                                                   Type = PaymentType.Expense,
                                                   Date = DateTime.Today,
                                                   Amount = 90,
                                                   CategoryId = 1
                                               },
                                               new PaymentEntity
                                               {
                                                   Id = 3,
                                                   Type = PaymentType.Transfer,
                                                   Date = DateTime.Today,
                                                   Amount = 40,
                                                   CategoryId = 1
                                               }
                                           }
                                       }
                                   }
                               }));

            // Act
            var result =
                new CategorySummaryDataProvider(categoryServiceMock.Object).GetValues(DateTime.Today.AddDays(-3),
                                                                                      DateTime.Today.AddDays(3))
                                                                           .Result.ToList();

            // Assert
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(-30);
        }
    }
}