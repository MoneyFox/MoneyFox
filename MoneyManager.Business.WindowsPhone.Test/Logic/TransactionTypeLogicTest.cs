#region

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.Business.WindowsPhone.Test.Logic {
    [TestClass]
    public class TransactionTypeLogicTest {
        [TestMethod]
        public void GetEnumFromStringTest() {
            TransactionType typeSpending = TransactionTypeLogic.GetEnumFromString("Spending");
            TransactionType typeIncome = TransactionTypeLogic.GetEnumFromString("Income");
            TransactionType typeTransfer = TransactionTypeLogic.GetEnumFromString("Transfer");

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }
    }
}