using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using Xunit;

namespace MoneyFox.BusinessLogic.Tests.StatisticDataProvider
{
    [ExcludeFromCodeCoverage]
    public class CategorySummaryProviderTests
    {
        [Fact]
        public async void GetValues_NullDependency_NullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(
                () => new CategorySummaryDataProvider(null).GetValues(DateTime.Today, DateTime.Today));
        }

        //[Fact]
        //public void GetValues_CorrectSums()
        //{
        //    // Arrange
        //    var categoryServiceMock = new Mock<IStatisticDbAccess>();
        //    categoryServiceMock.Setup(x => x.GetAllCategoriesWithPayments())
        //                       .Returns(Task.FromResult(new List<Category>
        //                       {
        //                           new Category("Ausgehen")
        //                           {
        //                               Payments =
        //                               {
        //                                   new Payment(DateTime.Today, 60, PaymentType.Income)
        //                               }
                                       
        //                                   Id = 1,
        //                                   Name = "Ausgehen",
        //                                   Payments = new List<PaymentEntity>
        //                                   {
        //                                       new PaymentEntity
        //                                       {
        //                                           Id = 1,
        //                                           Type = PaymentType.Income,
        //                                           Date = DateTime.Today,
        //                                           Amount = 60,
        //                                           CategoryId = 1
        //                                       },
        //                                       new PaymentEntity
        //                                       {
        //                                           Id = 2,
        //                                           Type = PaymentType.Expense,
        //                                           Date = DateTime.Today,
        //                                           Amount = 90,
        //                                           CategoryId = 1
        //                                       },
        //                                       new PaymentEntity
        //                                       {
        //                                           Id = 3,
        //                                           Type = PaymentType.Transfer,
        //                                           Date = DateTime.Today,
        //                                           Amount = 40,
        //                                           CategoryId = 1
        //                                       }
        //                                   }
        //                           }
        //                       }));

        //    // Act
        //    var result =
        //        new CategorySummaryDataProvider(categoryServiceMock.Object).GetValues(DateTime.Today.AddDays(-3),
        //                                                                              DateTime.Today.AddDays(3))
        //                                                                   .Result.ToList();

        //    // Assert
        //    result.Count.ShouldEqual(1);
        //    result.First().Value.ShouldEqual(-30);
        //}
    }
}