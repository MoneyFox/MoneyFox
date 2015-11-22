using System.Collections;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Localization;
using MoneyManager.TestFoundation;
using Xunit;

namespace MoneyManager.Core.Tests.Helper
{
    public class TransactionTypeLogicTest
    {
        public static IEnumerable GetEnumFrostringWithIntData
        {
            get
            {
                //Editmode true
                yield return new object[] {0, Strings.EditSpendingTitle, true};
                yield return new object[] {1, Strings.EditIncomeTitle, true};
                yield return new object[] {2, Strings.EditTransferTitle, true};

                //Editmode false
                yield return new object[] {0, Strings.AddSpendingTitle, false};
                yield return new object[] {1, Strings.AddIncomeTitle, false};
                yield return new object[] {2, Strings.AddTransferTitle, false};
            }
        }

        public static IEnumerable GetEnumFrostringWithEnumData
        {
            get
            {
                //Editmode true
                yield return new object[] {TransactionType.Spending, Strings.EditSpendingTitle, true};
                yield return new object[] {TransactionType.Income, Strings.EditIncomeTitle, true};
                yield return new object[] {TransactionType.Transfer, Strings.EditTransferTitle, true};

                //Editmode false
                yield return new object[] {TransactionType.Spending, Strings.AddSpendingTitle, false};
                yield return new object[] {TransactionType.Income, Strings.AddIncomeTitle, false};
                yield return new object[] {TransactionType.Transfer, Strings.AddTransferTitle, false};
            }
        }

        [Theory]
        [InlineData("Spending", TransactionType.Spending)]
        [InlineData("Income", TransactionType.Income)]
        [InlineData("Transfer", TransactionType.Transfer)]
        public void GetEnumFrostring_String_Titel(string inputString, TransactionType expectedType)
        {
            TransactionTypeHelper.GetEnumFromString(inputString).ShouldBe(expectedType);
        }

        [Theory]
        [MemberData(nameof(GetEnumFrostringWithIntData))]
        public void GetEnumFrostring_Int_Titel(int input, string expectedTitle, bool isEditMode)
        {
            TransactionTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);
        }

        [Theory]
        [MemberData(nameof(GetEnumFrostringWithEnumData))]
        public void GetEnumFrostring_Type_Titel(TransactionType input, string expectedTitle, bool isEditMode)
        {
            TransactionTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);
        }
    }
}