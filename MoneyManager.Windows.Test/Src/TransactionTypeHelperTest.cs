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

            Assert.AreEqual(TransactionType.Spending, resultSpending);
            Assert.AreEqual(TransactionType.Income, resultIncome);
        }

        [TestMethod]
        public void GetViewTitleForTypeTest()
        {
            var resultSpending = TransactionTypeHelper.GetViewTitleForType(TransactionType.Spending);
            var resultIncome = TransactionTypeHelper.GetViewTitleForType(TransactionType.Income);

            Assert.AreEqual(Utilities.GetTranslation("SpendingTitle"), resultSpending);
            Assert.AreEqual(Utilities.GetTranslation("IncomeTitle"), resultIncome);
        }
    }
}