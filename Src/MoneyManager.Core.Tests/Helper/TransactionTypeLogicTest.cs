using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Localization;
using MoneyManager.TestFoundation;
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

        public static IEnumerable GetEnumFrostringData
        {
            get
            {
                //Editmode true
                yield return new object[] { 0, Strings.EditSpendingTitle, true };
                yield return new object[] { 1, Strings.EditIncomeTitle, true };
                yield return new object[] { 2, Strings.EditTransferTitle, true };
                
                //Editmode false
                yield return new object[] { 0, Strings.AddSpendingTitle, false };
                yield return new object[] { 1, Strings.AddIncomeTitle, false };
                yield return new object[] { 2, Strings.AddTransferTitle, false };
            }
        }
            
        [Theory]
        [MemberData(nameof(GetEnumFrostringData))]
        public void GetEnumFrostring_Int_Titel(int input, string expectedTitle, bool isEditMode)
        {
            TransactionTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);
        }

        [Theory]
        //Editmode true
        [InlineData(TransactionType.Spending, "en-US", "Edit Spending", true)]
        [InlineData(TransactionType.Income, "en-US", "Edit Income", true)]
        [InlineData(TransactionType.Transfer, "en-US", "Edit Transfer", true)]
        [InlineData(TransactionType.Spending, "de-DE", "Ausgabe bearbeiten", true)]
        [InlineData(TransactionType.Income, "de-DE", "Einnahme bearbeiten", true)]
        [InlineData(TransactionType.Transfer, "de-DE", "Übertrag bearbeiten", true)]
        //Editmode false
        [InlineData(TransactionType.Spending, "en-US", "Add Spending", false)]
        [InlineData(TransactionType.Income, "en-US", "Add Income", false)]
        [InlineData(TransactionType.Transfer, "en-US", "Add Transfer", false)]
        [InlineData(TransactionType.Spending, "de-DE", "Ausgabe hinzufügen", false)]
        [InlineData(TransactionType.Income, "de-DE", "Einnahme hinzufügen", false)]
        [InlineData(TransactionType.Transfer, "de-DE", "Übertrag hinzufügen", false)]
        public void GetEnumFrostring_Type_Titel(TransactionType input, string culture, string expectedTitle,
            bool isEditMode)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            Strings.Culture = new CultureInfo(culture);
            TransactionTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);

            Strings.Culture = CultureInfo.CurrentUICulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentUICulture;
        }
    }
}