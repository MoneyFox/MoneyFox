using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MoneyFox.Service.DataServices;
using Moq;
using Xunit;

namespace MoneyFox.Business.Tests.StatisticProvider
{
    public class CategorySpreadingProviderTests
    {
        [Fact]
        public async void GetValues_NullDependency_NullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(
                () => new CategorySpreadingDataProvider(null).GetValues(DateTime.Today, DateTime.Today));
        }

        [Fact]
        public void GetValues_CorrectSums()
        {
            // Arrange
            var testCat1 = new CategoryEntity {Id = 2, Name = "Ausgehen"};
            var testCat2 = new CategoryEntity {Id = 3, Name = "Rent"};
            var testCat3 = new CategoryEntity {Id = 4, Name = "Food"};

            var paymentList = new List<Payment>
            {
                new Payment
                {
                    Data =
                    {
                        Id = 1,
                        Type = PaymentType.Income,
                        Date = DateTime.Today,
                        Amount = 60,
                        Category = testCat1,
                        CategoryId = testCat1.Id
                    }
                },
                new Payment
                {
                    Data =
                    {
                        Id = 2,
                        Type = PaymentType.Expense,
                        Date = DateTime.Today,
                        Amount = 90,
                        Category = testCat1,
                        CategoryId = testCat1.Id
                    }
                },
                new Payment
                {
                    Data =
                    {
                        Id = 3,
                        Type = PaymentType.Expense,
                        Date = DateTime.Today,
                        Amount = 10,
                        Category = testCat3,
                        CategoryId = testCat3.Id
                    }
                },
                new Payment
                {
                    Data =
                    {
                        Id = 4,
                        Type = PaymentType.Expense,
                        Date = DateTime.Today,
                        Amount = 90,
                        Category = testCat2,
                        CategoryId = testCat2.Id
                    }
                }
            };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(x => x.GetPaymentsWithoutTransfer(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<IEnumerable<Payment>>(paymentList));

            // Act
            var provider = new CategorySpreadingDataProvider(paymentService.Object);
            var result =  provider.GetValues(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)).Result.ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(90, result[0].Value);
            Assert.Equal(30, result[1].Value);
            Assert.Equal(10, result[2].Value);
        }
    }
}