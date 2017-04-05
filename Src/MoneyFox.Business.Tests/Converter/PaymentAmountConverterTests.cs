using MoneyFox.Business.Converter;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Tests;
using Moq;
using MvvmCross.Platform;
using Xunit;

namespace MoneyFox.Business.Tests.Converter
{
    [Collection("MvxIocCollection")]
    public class PaymentAmountConverterTests 
    {
        [Fact]
        public void Converter_Payment_NegativeAmountSign()
        {
            new PaymentAmountConverter()
                .Convert(new PaymentViewModel {Amount = 80, Type = PaymentType.Expense}, null, null, null)
                .ShouldBe("- " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_Payment_PositiveAmountSign()
        {
            new PaymentAmountConverter()
                .Convert(new PaymentViewModel {Amount = 80, Type = PaymentType.Income}, null, null, null)
                .ShouldBe("+ " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
            var account = new AccountViewModel
            {
                Id = 4,
                CurrentBalance = 400
            };

            Mvx.RegisterSingleton(new Mock<IAccountRepository>().Object);

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel
                {
                    Amount = 80,
                    Type = PaymentType.Transfer,
                    ChargedAccountId = account.Id,
                    ChargedAccount = account,
                    CurrentAccountId = account.Id
                }, null, account, null)
                .ShouldBe("- " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Plus()
        {
            var account = new AccountViewModel
            {
                Id = 4,
                CurrentBalance = 400
            };

            var mock = new Mock<IAccountRepository>();

            Mvx.RegisterSingleton(mock.Object);

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel
                {
                    Amount = 80,
                    Type = PaymentType.Transfer,
                    ChargedAccount = new AccountViewModel(),
                    CurrentAccountId = account.Id
                }, null, new AccountViewModel(), null)
                .ShouldBe("+ " + 80.ToString("C"));
        }
    }
}