using System;
using System.Linq.Expressions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.Converter;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class TransactionAmountConverterTests : MvxIoCSupportingTest
    {
        public TransactionAmountConverterTests()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [Theory]
        [InlineData(TransactionType.Spending, "- ")]
        [InlineData(TransactionType.Income, "+ ")]
        public void Converter_Transaction_AmountSign(TransactionType type, string result)
        {
            new TransactionAmountConverter().Convert(new FinancialTransaction {Amount = 80, Type = (int) type}, null,
                null, null).ShouldBe(result + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
            var account = new Account
            {
                Id = 4,
                CurrentBalance = 400
            };

            var mock = new Mock<IRepository<Account>>();
            mock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            mock.SetupGet(x => x.Selected).Returns(account);

            Mvx.RegisterSingleton(mock.Object);

            new TransactionAmountConverter()
                .Convert(new FinancialTransaction
                {
                    Amount = 80,
                    Type = (int) TransactionType.Transfer,
                    ChargedAccountId = account.Id,
                    ChargedAccount = account
                }, null, account, null)
                .ShouldBe("- " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Plus()
        {
            var account = new Account
            {
                Id = 4,
                CurrentBalance = 400
            };

            var mock = new Mock<IRepository<Account>>();
            mock.SetupGet(x => x.Selected).Returns(account);

            Mvx.RegisterSingleton(mock.Object);

            new TransactionAmountConverter()
                .Convert(new FinancialTransaction
                {
                    Amount = 80,
                    Type = (int) TransactionType.Transfer,
                    ChargedAccount = new Account()
                }, null, new Account(), null)
                .ShouldBe("+ " + 80.ToString("C"));
        }
    }
}