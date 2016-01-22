using System;
using System.Linq.Expressions;
using MoneyManager.Core.Converter;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class TransactionAmountConverterTests : MvxIoCSupportingTest
    {
        [Theory]
        [InlineData(PaymentType.Spending, "- ")]
        [InlineData(PaymentType.Income, "+ ")]
        public void Converter_Transaction_AmountSign(PaymentType type, string result)
        {
            new PaymentAmountConverter().Convert(new Payment {Amount = 80, Type = (int) type}, null,
                null, null).ShouldBe(result + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
            ClearAll();
            Setup();

            var account = new Account
            {
                Id = 4,
                CurrentBalance = 400
            };

            var mock = new Mock<IRepository<Account>>();
            mock.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            mock.SetupGet(x => x.Selected).Returns(account);

            Mvx.RegisterSingleton(mock.Object);

            new PaymentAmountConverter()
                .Convert(new Payment
                {
                    Amount = 80,
                    Type = (int) PaymentType.Transfer,
                    ChargedAccountId = account.Id,
                    ChargedAccount = account
                }, null, account, null)
                .ShouldBe("- " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Plus()
        {
            ClearAll();
            Setup();
            var account = new Account
            {
                Id = 4,
                CurrentBalance = 400
            };

            var mock = new Mock<IRepository<Account>>();
            mock.SetupGet(x => x.Selected).Returns(account);

            Mvx.RegisterSingleton(mock.Object);

            new PaymentAmountConverter()
                .Convert(new Payment
                {
                    Amount = 80,
                    Type = (int) PaymentType.Transfer,
                    ChargedAccount = new Account()
                }, null, new Account(), null)
                .ShouldBe("+ " + 80.ToString("C"));
        }
    }
}