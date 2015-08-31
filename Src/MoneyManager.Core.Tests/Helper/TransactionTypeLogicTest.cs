using MoneyManager.Core.Helper;
using MoneyManager.Foundation;
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
        [InlineData(0, "Spending")]
        [InlineData(1, "Income")]
        [InlineData(2, "Transfer")]
        public void GetEnumFrostring_Int_Titel(int input, string expectedTitle)
        {
            TransactionTypeHelper.GetViewTitleForType(input).ShouldBe(expectedTitle);
        }
        
        [Theory]
        [InlineData(TransactionType.Spending, "Spending")]
        [InlineData(TransactionType.Income, "Income")]
        [InlineData(TransactionType.Transfer, "Transfer")]
        public void GetEnumFrostring_Type_Titel(TransactionType input, string expectedTitle)
        {
            TransactionTypeHelper.GetViewTitleForType(input).ShouldBe(expectedTitle);
        }
    }
}