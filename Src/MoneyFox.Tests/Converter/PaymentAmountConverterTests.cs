using MoneyFox.Business.ViewModels;
using MoneyFox.Converter;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using Should;
using Xunit;

namespace MoneyFox.Tests.Converter
{
    [Collection("MvxIocCollection")]
    public class PaymentAmountConverterTests 
    {
        [Fact]
        public void Converter_Payment_NegativeAmountSign()
        {
            new PaymentAmountConverter()
                .Convert(new PaymentViewModel(new Payment()) {Amount = 80, Type = PaymentType.Expense}, null, null, null)
                .ShouldEqual("- " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_Payment_PositiveAmountSign()
        {
            new PaymentAmountConverter()
                .Convert(new PaymentViewModel(new Payment()) {Amount = 80, Type = PaymentType.Income}, null, null, null)
                .ShouldEqual("+ " + 80.ToString("C"));
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
            var account = new AccountViewModel(new Account())
            {
                Id = 4,
                CurrentBalance = 400
            };

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel(new Payment())
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
            var account = new AccountViewModel(new Account())
            {
                Id = 4,
                CurrentBalance = 400
            };

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel(new Payment())
                {
                    Amount = 80,
                    Type = PaymentType.Transfer,
                    ChargedAccount = new AccountViewModel(new Account()),
                    CurrentAccountId = account.Id
                }, null, new AccountViewModel(new Account()), null)
                .ShouldEqual("+ " + 80.ToString("C"));
        }
    }
}