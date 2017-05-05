using System;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using Xunit;

namespace MoneyFox.Business.Tests.Helpers
{
    public class PaymentTypeHelperTest
    {
        [Theory]
        [InlineData(PaymentType.Expense, "Expense")]
        [InlineData(PaymentType.Income, "Income")]
        [InlineData(PaymentType.Transfer, "Transfer")]
        public void GetEnumFromString_Expense_Titel(PaymentType type, string typeString)
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
        public void GetEnumFrostring_ExpenseIntEditTrue_Titel()
        {
            Assert.Equal(Strings.EditSpendingTitle, PaymentTypeHelper.GetViewTitleForType(0, true));
        }

        [Fact]
        public void GetEnumFrostring_IncomeIntEditTrue_Titel()
        {
            Assert.Equal(Strings.EditIncomeTitle, PaymentTypeHelper.GetViewTitleForType(1, true));
        }

        [Fact]
        public void GetEnumFrostring_TransferIntEditTrue_Titel()
        {
            Assert.Equal(Strings.EditTransferTitle, PaymentTypeHelper.GetViewTitleForType(2, true));
        }

        [Fact]
        public void GetEnumFrostring_ExpenseIntEditFalse_Titel()
        {
            Assert.Equal(Strings.AddExpenseTitle, PaymentTypeHelper.GetViewTitleForType(0, false));
        }

        [Fact]
        public void GetEnumFrostring_IncomeIntEditFalse_Titel()
        {
            Assert.Equal(Strings.AddIncomeTitle, PaymentTypeHelper.GetViewTitleForType(1, false));
        }

        [Fact]
        public void GetEnumFrostring_TransferIntEditFalse_Titel()
        {
            Assert.Equal(Strings.AddTransferTitle, PaymentTypeHelper.GetViewTitleForType(2, false));
        }

        [Fact]
        public void GetEnumFrostring_ExpenseEnumEditTrue_Titel()
        {
            Assert.Equal(Strings.EditSpendingTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Expense, true));
        }

        [Fact]
        public void GetEnumFrostring_IncomeEnumEditTrue_Titel()
        {
            Assert.Equal(Strings.EditIncomeTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Income, true));
        }

        [Fact]
        public void GetEnumFrostring_TransferEnumEditTrue_Titel()
        {
            Assert.Equal(Strings.EditTransferTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Transfer, true));
        }

        [Fact]
        public void GetEnumFrostring_ExpenseEnumEditFalse_Titel()
        {
            Assert.Equal(Strings.AddExpenseTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Expense, false));
        }

        [Fact]
        public void GetEnumFrostring_IncomeEnumEditFalse_Titel()
        {
            Assert.Equal(Strings.AddIncomeTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Income, false));
        }

        [Fact]
        public void GetEnumFrostring_TransferEnumEditFalse_Titel()
        {
            Assert.Equal(Strings.AddTransferTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Transfer, false));
        }
    }
}