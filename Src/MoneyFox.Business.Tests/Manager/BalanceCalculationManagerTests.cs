using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using Moq;
using Xunit;

namespace MoneyFox.Business.Tests.Manager
{
    public class BalanceCalculationManagerTests
    {
        [Theory]
        [InlineData(PaymentType.Expense, true)]
        [InlineData(PaymentType.Income, false)]
        public async void EndofMonthManager_AccountIsOverdrawnCorrectSet(PaymentType paymentType, bool expectedResult)
        {
            // Arrange
            var account1 = new Account
            {
                Data =
                {
                    Id = 1,
                    CurrentBalance = 100
                }
            };
            
            var accounts = new List<Account>
            {
                new Account{ Data = {Id=2, CurrentBalance=100}}, 
                account1
            };

            var paymentRepoSetup = new Mock<IPaymentService>();
            paymentRepoSetup.Setup(x => x.GetUnclearedPayments(It.IsAny<DateTime>(), It.IsAny<int>()))
                            .Returns(Task.FromResult<IEnumerable<Payment>>(new List<Payment>
                            {
                                new Payment
                                {
                                    Data =
                                    {
                                        Id = 10,
                                        ChargedAccountId = 1,
                                        TargetAccountId = 2,
                                        Amount = 100,
                                        Date = DateTime.Now,
                                        Type = paymentType
                                    }
                                },
                                new Payment
                                {
                                    Data =
                                    {
                                        Id = 15,
                                        ChargedAccountId = 1,
                                        TargetAccountId = 2,
                                        Amount = 100,
                                        Date = DateTime.Now,
                                        Type = paymentType
                                    }
                                }
                            }));

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAllAccounts())
                              .ReturnsAsync(accounts);

            // Act
            await new BalanceCalculationManager(paymentRepoSetup.Object, accountServiceMock.Object).CheckIfAccountsAreOverdrawn();

            // Assert
            Assert.Equal(expectedResult, account1.Data.IsOverdrawn);
        }

        [Fact]
        public async void GetTotalEndOfMonthBalance_TwoAccounts_SumOfAccounts()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetUnclearedPayments(It.IsAny<DateTime>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Payment>());

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetNotExcludedAccounts())
                .ReturnsAsync(new List<Account>
                              {
                                  new Account {Data = { CurrentBalance = 500}},
                                  new Account {Data = { CurrentBalance = 200}}
                              });

            // Act
            var result = await new BalanceCalculationManager(paymentServiceMock.Object, accountServiceMock.Object)
                .GetTotalEndOfMonthBalance();

            // Assert
            Assert.Equal(700, result);
        }
    }
}
