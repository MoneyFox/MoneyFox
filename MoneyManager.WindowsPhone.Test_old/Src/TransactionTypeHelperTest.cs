using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Foundation;
using MoneyManager.Src;

namespace MoneyManager.WindowsPhone.Test.Src
{
    [TestClass]
    internal class TransactionTypeHelperTest
    {
        [TestMethod]
        public void GetEnumFromStringTest()
        {
            TransactionType enumSpending = TransactionTypeHelper.GetEnumFromString("Spending");
            TransactionType enumIncome = TransactionTypeHelper.GetEnumFromString("Income");

            Assert.AreEqual(TransactionType.Spending, enumSpending);
            Assert.AreEqual(TransactionType.Income, enumIncome);
        }

        [TestMethod]
        public void GetViewTitleForTypeIntTest()
        {
            string titleSpending = TransactionTypeHelper.GetViewTitleForType(0);
            string titleIncome = TransactionTypeHelper.GetViewTitleForType(1);

            Assert.AreEqual(Translation.GetTranslation("SpendingTitle"), titleSpending);
            Assert.AreEqual(Translation.GetTranslation("IncomeTitle"), titleIncome);
        }

        [TestMethod]
        public void GetViewTitleForTypeTest()
        {
            string titleSpending = TransactionTypeHelper.GetViewTitleForType(TransactionType.Spending);
            string titleIncome = TransactionTypeHelper.GetViewTitleForType(TransactionType.Income);

            Assert.AreEqual(Translation.GetTranslation("SpendingTitle"), titleSpending);
            Assert.AreEqual(Translation.GetTranslation("IncomeTitle"), titleIncome);
        }
    }
}