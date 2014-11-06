using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;

namespace MoneyManager.Business.WindowsPhone.Test.Logic
{
    [TestClass]
    public class TransactionTypeLogicTest
    {
        [TestMethod]
        public void GetEnumFromStringTest()
        {
            var typeSpending = TransactionTypeLogic.GetEnumFromString("Spending");
            var typeIncome = TransactionTypeLogic.GetEnumFromString("Income");
            var typeTransfer = TransactionTypeLogic.GetEnumFromString("Transfer");

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }
    }
}
