using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using MockQueryable.Moq;
using MoneyFox.Domain;
using MoneyFox.Foundation;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Xunit;

namespace MoneyFox.Presentation.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class BalanceCalculationServiceTests
    {
        [Fact]
        public async Task GetTotalEndOfMonthBalance_TwoAccounts_CorrectSum()
        {
            // Arrange
            var account1 = new AccountViewModel { Id = 1, CurrentBalance = 100 };
            var account2 = new AccountViewModel { Id = 2, CurrentBalance = 100 };

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
                    Id = 17,
                    ChargedAccount = account1,
                    Amount = 150,
                    Date = DateTime.Now,
                    Type = PaymentType.Expense
                },
                new PaymentViewModel
                {
                    Id = 16,
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
            Assert.Equal(350, result);
        }


        [Fact]
        public async Task GetTotalEndOfMonthBalance_ClearedPaymentsIgnored_CorrectSum()
        {
            // Arrange
            var account1 = new AccountViewModel { Id = 1, CurrentBalance = 100 };

            var accounts = new List<AccountViewModel>
            {
                account1
            };

            var paymentList = new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 10,
                    ChargedAccount = account1,
                    Amount = 100,
                    IsCleared = false,
                    Date = DateTime.Now,
                    Type = PaymentType.Expense
                },
                new PaymentViewModel
                {
                    Id = 15,
                    ChargedAccount = account1,
                    Amount = 200,
                    IsCleared = false,
                    Date = DateTime.Now,
                    Type = PaymentType.Income
                },
                new PaymentViewModel
                {
                    Id = 20,
                    ChargedAccount = account1,
                    Amount = 450,
                    IsCleared = true,
                    Date = DateTime.Now,
                    Type = PaymentType.Expense
                },
                new PaymentViewModel
                {
                    Id = 25,
                    ChargedAccount = account1,
                    Amount = 150,
                    IsCleared = true,
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
            Assert.Equal(200, result);
        }

        [Fact]
        public async Task GetEndOfMonthBalanceForAccount_CorrectSum()
        {
            // Arrange
            var account1 = new AccountViewModel { Id = 1, CurrentBalance = 100 };


            var paymentList = new List<PaymentViewModel>
            {
                new PaymentViewModel
                {
                    Id = 10,
                    ChargedAccount = account1,
                    Amount = 100,
                    IsCleared = false,
                    Date = DateTime.Now,
                    Type = PaymentType.Expense
                },
                new PaymentViewModel
                {
                    Id = 15,
                    ChargedAccount = account1,
                    Amount = 200,
                    IsCleared = false,
                    Date = DateTime.Now,
                    Type = PaymentType.Income
                },
                new PaymentViewModel
                {
                    Id = 20,
                    ChargedAccount = account1,
                    Amount = 450,
                    IsCleared = true,
                    Date = DateTime.Now,
                    Type = PaymentType.Expense
                },
                new PaymentViewModel
                {
                    Id = 25,
                    ChargedAccount = account1,
                    Amount = 150,
                    IsCleared = true,
                    Date = DateTime.Now,
                    Type = PaymentType.Income
                }
            };

            var mock = paymentList.AsQueryable().BuildMock();

            var crudServiceSetup = new Mock<ICrudServicesAsync>();
            crudServiceSetup.Setup(x => x.ReadManyNoTracked<PaymentViewModel>())
                .Returns(mock.Object);

            // Act
            var result = await new BalanceCalculationService(crudServiceSetup.Object)
                .GetEndOfMonthBalanceForAccount(account1);

            // Assert
            Assert.Equal(200, result);
        }

        [Fact]
        public async Task GetTotalBalance_TwoAccounts_SumOfAccounts()
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