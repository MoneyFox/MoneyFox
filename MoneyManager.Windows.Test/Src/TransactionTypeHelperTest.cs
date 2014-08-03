using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Src;

namespace MoneyManager.Windows.Test.Src
{
    [TestClass]
    public class TransactionTypeHelperTest
    {
        [TestMethod]
        public void GetEnumFromStringTest()
        {
            var resultSpending = TransactionTypeHelper.GetEnumFromString("spending");
            var resultIncome = TransactionTypeHelper.GetEnumFromString("income");
            var resultRefund = TransactionTypeHelper.GetEnumFromString("refund ");
            var resultTransfer = TransactionTypeHelper.GetEnumFromString("transfer");

            Assert.AreEqual(TransactionType.Spending, resultSpending);
            Assert.AreEqual(TransactionType.Income, resultIncome);
            Assert.AreEqual(TransactionType.Refund, resultRefund);
            Assert.AreEqual(TransactionType.Transfer, resultTransfer);
        }

        [TestMethod]
        public void GetViewTitleForTypeTest()
        {
            var resultSpending = TransactionTypeHelper.GetViewTitleForType(TransactionType.Spending);
            var resultIncome = TransactionTypeHelper.GetViewTitleForType(TransactionType.Income);
            var resultRefund = TransactionTypeHelper.GetViewTitleForType(TransactionType.Refund);
            var resultTransfer = TransactionTypeHelper.GetViewTitleForType(TransactionType.Transfer);

            Assert.AreEqual(Utilities.GetTranslation("SpendingTitle"), resultSpending);
            Assert.AreEqual(Utilities.GetTranslation("IncomeTitle"), resultIncome);
            Assert.AreEqual(Utilities.GetTranslation("RefundTitle"), resultRefund);
            Assert.AreEqual(Utilities.GetTranslation("TransferTitle"), resultTransfer);
        }
    }
}