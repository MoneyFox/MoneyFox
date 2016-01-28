using System.Collections;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Localization;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Core.Tests.Helper
{
    public class PaymentTypeHelperTest
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
                yield return new object[] {PaymentType.Spending, Strings.EditSpendingTitle, true};
                yield return new object[] {PaymentType.Income, Strings.EditIncomeTitle, true};
                yield return new object[] {PaymentType.Transfer, Strings.EditTransferTitle, true};

                //Editmode false
                yield return new object[] {PaymentType.Spending, Strings.AddSpendingTitle, false};
                yield return new object[] {PaymentType.Income, Strings.AddIncomeTitle, false};
                yield return new object[] {PaymentType.Transfer, Strings.AddTransferTitle, false};
            }
        }

        [Theory]
        [InlineData("Spending", PaymentType.Spending)]
        [InlineData("Income", PaymentType.Income)]
        [InlineData("Transfer", PaymentType.Transfer)]
        public void GetEnumFrostring_String_Titel(string inputString, PaymentType expectedType)
        {
            PaymentTypeHelper.GetEnumFromString(inputString).ShouldBe(expectedType);
        }

        [Theory]
        [MemberData(nameof(GetEnumFrostringWithIntData))]
        public void GetEnumFrostring_Int_Titel(int input, string expectedTitle, bool isEditMode)
        {
            PaymentTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);
        }

        [Theory]
        [MemberData(nameof(GetEnumFrostringWithEnumData))]
        public void GetEnumFrostring_Type_Titel(PaymentType input, string expectedTitle, bool isEditMode)
        {
            PaymentTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);
        }
    }
}