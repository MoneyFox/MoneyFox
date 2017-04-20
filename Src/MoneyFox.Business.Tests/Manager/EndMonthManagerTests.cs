using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Business.Manager;
using MoneyFox.Foundation;
using Moq;
using Xunit;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Business.Tests.Manager
{
    public class EndMonthManagerTests
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
                                    Data = {Id = 10, ChargedAccountId = 1, Amount = 100, Date = DateTime.Now, Type = paymentType }
                                },
                                new Payment
                                {
                                    Data = {Id = 15, ChargedAccountId = 1, Amount = 100, Date = DateTime.Now, , Type = paymentType }
                                }
                            }));

            // Act
            await new EndOfMonthManager(paymentRepoSetup.Object).CheckIfAccountsAreOverdrawn(accounts);

            // Assert
            Assert.Equal(expectedResult, account1.Data.IsOverdrawn);
        }

        [Fact]
        public async void GetTotalEndOfMonthBalance_TwoAccounts_SumOfAccounts()
        {
            // Arrange
            var paymentMockSetup = new Mock<IPaymentService>();
            paymentMockSetup.Setup(x => x.GetUnclearedPayments(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<Payment>>(new List<Payment>()));

            // Act
            var result = await new EndOfMonthManager(paymentMockSetup.Object)
                .GetTotalEndOfMonthBalance(new List<Account>
                {
                    new Account {Data = {CurrentBalance = 500}},
                    new Account {Data = {CurrentBalance = 200}}
                });

            // Assert
            Assert.Equal(700, result);
        }
    }
}
