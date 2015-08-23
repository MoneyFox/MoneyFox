using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation;

namespace MoneyManager.Core.Tests.Helper
{
    [TestClass]
    public class TransactionTypeLogicTest
    {
        [TestMethod]
        public void GetEnumFromString_String_Title()
        {
            var typeSpending = TransactionTypeHelper.GetEnumFromString("Spending");
            var typeIncome = TransactionTypeHelper.GetEnumFromString("Income");
            var typeTransfer = TransactionTypeHelper.GetEnumFromString("Transfer");

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }

        [TestMethod]
        public void GetViewTitleForType_Int_Title()
        {
            var typeSpending = TransactionTypeHelper.GetViewTitleForType(0);
            var typeIncome = TransactionTypeHelper.GetViewTitleForType(1);
            var typeTransfer = TransactionTypeHelper.GetViewTitleForType(2);

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }

        [TestMethod]
        public void GetViewTitleForType_TransactionType_Title()
        {
            var typeSpending = TransactionTypeHelper.GetViewTitleForType(TransactionType.Spending);
            var typeIncome = TransactionTypeHelper.GetViewTitleForType(TransactionType.Income);
            var typeTransfer = TransactionTypeHelper.GetViewTitleForType(TransactionType.Spending);

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }
    }
}