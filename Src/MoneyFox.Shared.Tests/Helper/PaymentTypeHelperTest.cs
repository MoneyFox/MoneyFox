using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Shared.Tests.Helper
{
    [TestClass]
    public class PaymentTypeHelperTest
    {
        [TestMethod]
        public void GetEnumFromString_Expense_Titel()
        {
            Assert.AreEqual(PaymentType.Expense, PaymentTypeHelper.GetEnumFromString("Expense"));
        }

        [TestMethod]
        public void GetEnumFromString_Income_Titel()
        {
            Assert.AreEqual(PaymentType.Income, PaymentTypeHelper.GetEnumFromString("Income"));
        }

        [TestMethod]
        public void GetEnumFromString_Transfer_Titel()
        {
            Assert.AreEqual(PaymentType.Transfer, PaymentTypeHelper.GetEnumFromString("Transfer"));
        }


        [TestMethod]
        public void GetEnumFrostring_ExpenseIntEditTrue_Titel()
        {
            Assert.AreEqual(Strings.EditSpendingTitle, PaymentTypeHelper.GetViewTitleForType(0, true));
        }

        [TestMethod]
        public void GetEnumFrostring_IncomeIntEditTrue_Titel()
        {
            Assert.AreEqual(Strings.EditIncomeTitle, PaymentTypeHelper.GetViewTitleForType(1, true));
        }

        [TestMethod]
        public void GetEnumFrostring_TransferIntEditTrue_Titel()
        {
            Assert.AreEqual(Strings.EditTransferTitle, PaymentTypeHelper.GetViewTitleForType(2, true));
        }

        [TestMethod]
        public void GetEnumFrostring_ExpenseIntEditFalse_Titel()
        {
            Assert.AreEqual(Strings.AddExpenseTitle, PaymentTypeHelper.GetViewTitleForType(0, false));
        }

        [TestMethod]
        public void GetEnumFrostring_IncomeIntEditFalse_Titel()
        {
            Assert.AreEqual(Strings.AddIncomeTitle, PaymentTypeHelper.GetViewTitleForType(1, false));
        }

        [TestMethod]
        public void GetEnumFrostring_TransferIntEditFalse_Titel()
        {
            Assert.AreEqual(Strings.AddTransferTitle, PaymentTypeHelper.GetViewTitleForType(2, false));
        }

        [TestMethod]
        public void GetEnumFrostring_ExpenseEnumEditTrue_Titel()
        {
            Assert.AreEqual(Strings.EditSpendingTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Expense, true));
        }

        [TestMethod]
        public void GetEnumFrostring_IncomeEnumEditTrue_Titel()
        {
            Assert.AreEqual(Strings.EditIncomeTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Income, true));
        }

        [TestMethod]
        public void GetEnumFrostring_TransferEnumEditTrue_Titel()
        {
            Assert.AreEqual(Strings.EditTransferTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Transfer, true));
        }

        [TestMethod]
        public void GetEnumFrostring_ExpenseEnumEditFalse_Titel()
        {
            Assert.AreEqual(Strings.AddExpenseTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Expense, false));
        }

        [TestMethod]
        public void GetEnumFrostring_IncomeEnumEditFalse_Titel()
        {
            Assert.AreEqual(Strings.AddIncomeTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Income, false));
        }

        [TestMethod]
        public void GetEnumFrostring_TransferEnumEditFalse_Titel()
        {
            Assert.AreEqual(Strings.AddTransferTitle, PaymentTypeHelper.GetViewTitleForType(PaymentType.Transfer, false));
        }
    }
}