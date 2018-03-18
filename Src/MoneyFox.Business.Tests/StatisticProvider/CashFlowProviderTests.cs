using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using Moq;
using Xunit;
using MoneyFox.Business.StatisticDataProvider;
using Should;

namespace MoneyFox.Business.Tests.StatisticProvider
{
    public class CashFlowProviderTests
    {
        [Fact]
        public async void GetValues_NullDependency_NullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(
                () => new CashFlowDataProvider(null).GetCashFlow(DateTime.Today, DateTime.Today));
        }

        [Fact]
        public async void GetValues_CorrectSums()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetPaymentsWithoutTransfer(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                            .Returns(Task.FromResult<IEnumerable<Payment>>(new List<Payment>
                            {
                                new Payment
                                {
                                    Data =
                                    {
                                        Id = 1,
                                        Type = PaymentType.Income,
                                        Date = DateTime.Today,
                                        Amount = 60
                                    }
                                },
                                new Payment
                                {
                                    Data =
                                    {
                                        Id = 3,
                                        Type = PaymentType.Income,
                                        Date = DateTime.Today,
                                        Amount = 70
                                    }
                                },
                                new Payment
                                {
                                    Data =
                                    {
                                        Id = 2,
                                        Type = PaymentType.Expense,
                                        Date = DateTime.Today,
                                        Amount = 50
                                    }
                                },
                                new Payment
                                {
                                    Data =
                                    {
                                        Id = 3,
                                        Type = PaymentType.Expense,
                                        Date = DateTime.Today,
                                        Amount = 40
                                    }
                                }
                            }));

            // Act
            var result = await new CashFlowDataProvider(paymentServiceMock.Object)
                .GetCashFlow(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3));

            // Assert
            result[0].Value.ShouldEqual(130);
            result[1].Value.ShouldEqual(90);
            result[2].Value.ShouldEqual(40);
        }
    }
}