using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Ui.Shared.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Ui.Shared.Tests.Utilities
{
    [ExcludeFromCodeCoverage]
    public class PaymentTypeHelperTests
    {
        [Theory]
        [InlineData(PaymentType.Expense, "Expense")]
        [InlineData(PaymentType.Income, "Income")]
        [InlineData(PaymentType.Transfer, "Transfer")]
        public void GetEnumFromString_Expense_Title(PaymentType type, string typeString)
        {
            Assert.Equal(type, PaymentTypeHelper.GetEnumFromString(typeString));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-1)]
        public void GetTypeString_InvalidType_Exception(int enumInt)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => PaymentTypeHelper.GetTypeString(enumInt));
        }

        [Fact]
        public void GetEnumFromString_ExpenseIntEditTrue_Title()
        {
            Assert.Equal(Strings.EditSpendingTitle, PaymentTypeHelper.GetViewTitleForType(0, true));
        }

        [Fact]
        public void GetEnumFromString_IncomeIntEditTrue_Title()
        {
            Assert.Equal(Strings.EditIncomeTitle, PaymentTypeHelper.GetViewTitleForType(1, true));
        }

        [Fact]
        public void GetEnumFromString_TransferIntEditTrue_Title()
        {
            Assert.Equal(Strings.EditTransferTitle, PaymentTypeHelper.GetViewTitleForType(2, true));
        }

        [Fact]
        public void GetEnumFromString_ExpenseIntEditFalse_Title()
        {
            Assert.Equal(Strings.AddExpenseTitle, PaymentTypeHelper.GetViewTitleForType(0, false));
        }

        [Fact]
        public void GetEnumFromString_IncomeIntEditFalse_Title()
        {
            Assert.Equal(Strings.AddIncomeTitle, PaymentTypeHelper.GetViewTitleForType(1, false));
        }

        [Fact]
        public void GetEnumFromString_TransferIntEditFalse_Title()
        {
            Assert.Equal(Strings.AddTransferTitle, PaymentTypeHelper.GetViewTitleForType(2, false));
        }

        [Fact]
        public void GetEnumFromString_ExpenseEnumEditTrue_Title()
        {
            Assert.Equal(Strings.EditSpendingTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Expense, true));
        }

        [Fact]
        public void GetEnumFromString_IncomeEnumEditTrue_Title()
        {
            Assert.Equal(Strings.EditIncomeTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Income, true));
        }

        [Fact]
        public void GetEnumFromString_TransferEnumEditTrue_Title()
        {
            Assert.Equal(Strings.EditTransferTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Transfer, true));
        }

        [Fact]
        public void GetEnumFromString_ExpenseEnumEditFalse_Title()
        {
            Assert.Equal(Strings.AddExpenseTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Expense, false));
        }

        [Fact]
        public void GetEnumFromString_IncomeEnumEditFalse_Title()
        {
            Assert.Equal(Strings.AddIncomeTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Income, false));
        }

        [Fact]
        public void GetEnumFromString_TransferEnumEditFalse_Title()
        {
            Assert.Equal(Strings.AddTransferTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Transfer, false));
        }
    }
}
