using System.Diagnostics.CodeAnalysis;
using MoneyFox.Foundation;
using MoneyFox.Presentation.Converter;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.Converter
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class PaymentAmountConverterTests 
    {
        [Fact]
        public void Converter_Payment_NegativeAmountSign()
        {
            new PaymentAmountConverter()
                .Convert(new PaymentViewModel{Amount = 80, Type = PaymentType.Expense}, null, null, null)
                .ShouldEqual("- " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_Payment_PositiveAmountSign()
        {
            new PaymentAmountConverter()
                .Convert(new PaymentViewModel {Amount = 80, Type = PaymentType.Income}, null, null, null)
                .ShouldEqual("+ " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
            var account = new AccountViewModel
            {
                Id = 4,
                CurrentBalance = 400
            };

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel
                {
                    Amount = 80,
                    Type = PaymentType.Transfer,
                    ChargedAccountId = account.Id,
                    ChargedAccount = account,
                    CurrentAccountId = account.Id
                }, null, account, null)
                .ShouldEqual("- " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Plus()
        {
            var account = new AccountViewModel
            {
                Id = 4,
                CurrentBalance = 400
            };

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel
                {
                    Amount = 80,
                    Type = PaymentType.Transfer,
                    ChargedAccount = new AccountViewModel(),
                    CurrentAccountId = account.Id
                }, null, new AccountViewModel(), null)
                .ShouldEqual("+ " + 80.ToString("C"));
        }
    }
}