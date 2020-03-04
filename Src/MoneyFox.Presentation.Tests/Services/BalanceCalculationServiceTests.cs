using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetExcludedAccount;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Payments.Queries.GetUnclearedPaymentsOfThisMonth;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Services;
using Moq;
using Xunit;

namespace MoneyFox.Presentation.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class BalanceCalculationServiceTests
    {
        private readonly Mock<IMediator> mediatorMock;

        public BalanceCalculationServiceTests()
        {
            var account1 = new Account("Foo1", 100);
            var account2 = new Account("Foo2", 100);

            var accounts = new List<Account>
            {
                account1,
                account2
            };

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Now, 100, PaymentType.Transfer, account1, account2),
                new Payment(DateTime.Now, 150, PaymentType.Expense, account1),
                new Payment(DateTime.Now, 300, PaymentType.Income, account2)
            };

            mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetUnclearedPaymentsOfThisMonthQuery>(), default))
                        .ReturnsAsync(paymentList);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetExcludedAccountQuery>(), default))
                        .ReturnsAsync(accounts);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetIncludedAccountBalanceSummaryQuery>(), default))
                        .ReturnsAsync(700);
        }

        [Fact]
        public async Task GetTotalEndOfMonthBalance_TwoAccounts_CorrectSum()
        {
            // Arrange
            // Act
            decimal result = await new BalanceCalculationService(mediatorMock.Object).GetTotalEndOfMonthBalance();

            // Assert
            Assert.Equal(850, result);
        }

        [Fact]
        public async Task GetTotalEndOfMonthBalance_CorrectSum()
        {
            // Arrange
            var account1 = new Account("Foo1", 100);

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Now, 100, PaymentType.Expense, account1),
                new Payment(DateTime.Now, 200, PaymentType.Income, account1),
                new Payment(DateTime.Now, 450, PaymentType.Expense, account1),
                new Payment(DateTime.Now, 150, PaymentType.Expense, account1)
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetUnclearedPaymentsOfThisMonthQuery>(), default))
                        .ReturnsAsync(paymentList);
            // Act
            decimal result = await new BalanceCalculationService(mediatorMock.Object)
               .GetTotalEndOfMonthBalance();

            // Assert
            Assert.Equal(200, result);
        }

        [Fact]
        public async Task GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            // Arrange

            // Act
            await new BalanceCalculationService(mediatorMock.Object).GetTotalBalance();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<GetIncludedAccountBalanceSummaryQuery>(), default), Times.Once);
        }
    }
}
