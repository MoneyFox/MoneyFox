using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Converter;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyManager.Core.Tests.Converter
{
    [TestClass]
    public class PaymentAmountConverterTests : MvxIoCSupportingTest
    {
        [TestMethod]
        public void Converter_Payment_NegativeAmountSign()
        {
            new PaymentAmountConverter().Convert(new Payment {Amount = 80, Type = (int)PaymentType.Expense}, null,
                null, null).ShouldBe("- " + 80.ToString("C"));
        }
        
        [TestMethod]
        public void Converter_Payment_PositiveAmountSign()
        {
            new PaymentAmountConverter().Convert(new Payment {Amount = 80, Type = (int)PaymentType.Income }, null,
                null, null).ShouldBe("+ " + 80.ToString("C"));
        }

        [TestMethod]
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

        [TestMethod]
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