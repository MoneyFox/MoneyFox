using System.Globalization;
using MoneyManager.Core.Helper;
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

        [Theory]
        //Editmode true
        [InlineData(0, "en-US", "Edit Spending", true)]
        [InlineData(1, "en-US", "Edit Income", true)]
        [InlineData(2, "en-US", "Edit Transfer", true)]
        [InlineData(0, "de-DE", "Ausgabe bearbeiten", true)]
        [InlineData(1, "de-DE", "Einkommen bearbeiten", true)]
        [InlineData(2, "de-DE", "Überweisung bearbeiten", true)]
        //Editmode false
        [InlineData(0, "en-US", "Add Spending", false)]
        [InlineData(1, "en-US", "Add Income", false)]
        [InlineData(2, "en-US", "Add Transfer", false)]
        [InlineData(0, "de-DE", "Ausgabe hinzufügen", false)]
        [InlineData(1, "de-DE", "Einkommen hinzufügen", false)]
        [InlineData(2, "de-DE", "Überweisung hinzufügen", false)]
        public void GetEnumFrostring_Int_Titel(int input, string culture, string expectedTitle, bool isEditMode)
        {
            Strings.Culture = new CultureInfo(culture);
            TransactionTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);
        }

        [Theory]
        //Editmode true
        [InlineData(TransactionType.Spending, "en-US", "Edit Spending", true)]
        [InlineData(TransactionType.Income, "en-US", "Edit Income", true)]
        [InlineData(TransactionType.Transfer, "en-US", "Edit Transfer", true)]
        [InlineData(TransactionType.Spending, "de-DE", "Ausgabe bearbeiten", true)]
        [InlineData(TransactionType.Income, "de-DE", "Einkommen bearbeiten", true)]
        [InlineData(TransactionType.Transfer, "de-DE", "Überweisung bearbeiten", true)]
        //Editmode false
        [InlineData(TransactionType.Spending, "en-US", "Add Spending", false)]
        [InlineData(TransactionType.Income, "en-US", "Add Income", false)]
        [InlineData(TransactionType.Transfer, "en-US", "Add Transfer", false)]
        [InlineData(TransactionType.Spending, "de-DE", "Ausgabe hinzufügen", false)]
        [InlineData(TransactionType.Income, "de-DE", "Einkommen hinzufügen", false)]
        [InlineData(TransactionType.Transfer, "de-DE", "Überweisung hinzufügen", false)]
        public void GetEnumFrostring_Type_Titel(TransactionType input, string culture, string expectedTitle,
            bool isEditMode)
        {
            Strings.Culture = new CultureInfo(culture);
            TransactionTypeHelper.GetViewTitleForType(input, isEditMode).ShouldBe(expectedTitle);
        }
    }
}