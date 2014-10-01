using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Src;

namespace MoneyManager.WindowsPhone.Test.Src
{
    [TestClass]
    public class TransactionTypeHelperTest
    {
        [TestMethod]
        public void GetEnumFromStringTest()
        {
            var enumSpending = TransactionTypeHelper.GetEnumFromString("spending");
            var enumIncome = TransactionTypeHelper.GetEnumFromString("income");

            Assert.AreEqual(TransactionType.Spending, enumSpending);
            Assert.AreEqual(TransactionType.Income, enumIncome);
        }

        [TestMethod]
        public void GetViewTitleForTypeIntTest()
        {
            var titleSpending = TransactionTypeHelper.GetViewTitleForType(0);
            var titleIncome = TransactionTypeHelper.GetViewTitleForType(1);
            
            Assert.AreEqual(Utilities.GetTranslation("SpendingTitle"), titleSpending);
            Assert.AreEqual(Utilities.GetTranslation("IncomeTitle"), titleIncome);
        }        
        
        [TestMethod]
        public void GetViewTitleForTypeTest()
        {
            var titleSpending = TransactionTypeHelper.GetViewTitleForType(TransactionType.Spending);
            var titleIncome = TransactionTypeHelper.GetViewTitleForType(TransactionType.Income);
            
            Assert.AreEqual(Utilities.GetTranslation("SpendingTitle"), titleSpending);
            Assert.AreEqual(Utilities.GetTranslation("IncomeTitle"), titleIncome);
        }
    }
}
