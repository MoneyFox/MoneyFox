using System;
using System.Collections.Generic;
using System.Linq;
using GenericServices;
using MockQueryable.Moq;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Services
{
    public class BalanceCalculationServiceTests
    {
        [Fact]
        public async void GetTotalEndOfMonthBalance_TwoAccounts_CorrectSum()
        {
            // Arrange
            var account1 = new AccountViewModel { Id = 1, CurrentBalance = 100 };
            var account2 = new AccountViewModel { Id = 1, CurrentBalance = 100 };

            var accounts = new List<AccountViewModel>
            {
                account1,
                account2
            };

            var paymentList = new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 10,
                    ChargedAccount = account1,
                    TargetAccount = account2,
                    Amount = 100,
                    Date = DateTime.Now,
                    Type = PaymentType.Transfer
                },
                new PaymentViewModel
                {
                    Id = 15,
                    ChargedAccount = account1,
                    Amount = 200,
                    Date = DateTime.Now,
                    Type = PaymentType.Expense
                },
                new PaymentViewModel
                {
                    Id = 15,
                    ChargedAccount = account2,
                    Amount = 300,
                    Date = DateTime.Now,
                    Type = PaymentType.Income
                }
            };

            var mock = paymentList.AsQueryable().BuildMock();

            var crudServiceSetup = new Mock<ICrudServicesAsync>();
            crudServiceSetup.Setup(x => x.ReadManyNoTracked<PaymentViewModel>())
                .Returns(mock.Object);
            crudServiceSetup.Setup(x => x.ReadManyNoTracked<AccountViewModel>())
                .Returns(accounts.AsQueryable().BuildMock().Object);

            // Act
            var result = await new BalanceCalculationService(crudServiceSetup.Object)
                .GetTotalEndOfMonthBalance();

            // Assert
            Assert.Equal(300, result);
        }

        [Fact]
        public async void GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            // Arrange
            var accounts = new List<AccountViewModel>
            {
                new AccountViewModel { Id = 1, CurrentBalance = 500 },
                new AccountViewModel { Id = 1, CurrentBalance = 200 }
            };

            var crudServiceSetup = new Mock<ICrudServicesAsync>();

            crudServiceSetup.Setup(x => x.ReadManyNoTracked<AccountViewModel>())
                .Returns(accounts.AsQueryable().BuildMock().Object);

            // Act
            var result = await new BalanceCalculationService(crudServiceSetup.Object)
                .GetTotalBalance();

            // Assert
            Assert.Equal(700, result);
        }
    }
}