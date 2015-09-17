using MoneyManager.Core.Converter;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class TransactionAmountConverterTests
    {
        [Theory]
        [InlineData(TransactionType.Spending, "- CHF 80.00")]
        [InlineData(TransactionType.Income, "+ CHF 80.00")]
        public void Converter_Transaction_AmountSign(TransactionType type, string result)
        {
            new TransactionAmountConverter().Convert(new FinancialTransaction {Amount = 80, Type = (int) type}, null, null, null).ShouldBe(result);
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
            var account = new Account
            {
                Id = 4,
                CurrentBalance = 400
            };

            new TransactionAmountConverter()
                .Convert(new FinancialTransaction
                {
                    Amount = 80,
                    Type = (int) TransactionType.Transfer,
                    ChargedAccount = account
                }, null, account, null)
                .ShouldBe("- CHF 80.00");
        }

        [Fact]
        public void Converter_TransferSameAccount_Plus()
        {
            new TransactionAmountConverter()
                .Convert(new FinancialTransaction
                {
                    Amount = 80,
                    Type = (int) TransactionType.Transfer,
                    ChargedAccount = new Account
                    {
                        Id = 4,
                        CurrentBalance = 400
                    }
                }, null, new Account(), null)
                .ShouldBe("+ CHF 80.00");
        }
    }
}