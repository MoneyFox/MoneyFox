using System.Globalization;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation;
using MoneyManager.Localization;
using Xunit;

namespace MoneyManager.Core.Tests.Helper
{
    public class TransactionTypeLogicTest
    {
        [Theory]
        [InlineData("Spending", TransactionType.Spending)]
        [InlineData("Income", TransactionType.Income)]
        [InlineData("Transfer", TransactionType.Transfer)]
        public void GetEnumFrostring_String_Titel(string inputString, TransactionType expectedType)
        {
            TransactionTypeHelper.GetEnumFromString(inputString).ShouldBe(expectedType);
        }

        [Theory]
        [InlineData(0, "en-US", "Spending")]
        [InlineData(1, "en-US", "Income")]
        [InlineData(2, "en-US", "Transfer")]
        public void GetEnumFrostring_Int_Titel(int input, string culture, string expectedTitle)
        {
            Strings.Culture = new CultureInfo(culture);
            TransactionTypeHelper.GetViewTitleForType(input).ShouldBe(expectedTitle);
        }

        [Theory]
        [InlineData(TransactionType.Spending, "en-US", "Spending")]
        [InlineData(TransactionType.Income, "en-US", "Income")]
        [InlineData(TransactionType.Transfer, "en-US", "Transfer")]
        [InlineData(TransactionType.Spending, "de-DE", "Ausgabe")]
        [InlineData(TransactionType.Income, "de-DE", "Einkommen")]
        [InlineData(TransactionType.Transfer, "de-DE", "Überweisung")]
        public void GetEnumFrostring_Type_Titel(TransactionType input, string culture, string expectedTitle)
        {
            Strings.Culture = new CultureInfo(culture);
            TransactionTypeHelper.GetViewTitleForType(input).ShouldBe(expectedTitle);
        }
    }
}