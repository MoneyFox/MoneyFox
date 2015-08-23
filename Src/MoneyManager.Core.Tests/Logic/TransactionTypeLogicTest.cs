using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;


namespace MoneyManager.Business.WindowsPhone.Test.Logic
{
    [TestClass]
    public class TransactionTypeLogicTest
    {
        [TestMethod]
        public void GetEnumFromString_String_Title()
        {
            var typeSpending = TransactionTypeLogic.GetEnumFromString("Spending");
            var typeIncome = TransactionTypeLogic.GetEnumFromString("Income");
            var typeTransfer = TransactionTypeLogic.GetEnumFromString("Transfer");

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }

        [TestMethod]
        public void GetViewTitleForType_Int_Title()
        {
            var typeSpending = TransactionTypeLogic.GetViewTitleForType(0);
            var typeIncome = TransactionTypeLogic.GetViewTitleForType(1);
            var typeTransfer = TransactionTypeLogic.GetViewTitleForType(2);

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }

        [TestMethod]
        public void GetViewTitleForType_TransactionType_Title()
        {
            var typeSpending = TransactionTypeLogic.GetViewTitleForType(TransactionType.Spending);
            var typeIncome = TransactionTypeLogic.GetViewTitleForType(TransactionType.Income);
            var typeTransfer = TransactionTypeLogic.GetViewTitleForType(TransactionType.Spending);

            Assert.AreEqual(TransactionType.Spending, typeSpending);
            Assert.AreEqual(TransactionType.Income, typeIncome);
            Assert.AreEqual(TransactionType.Transfer, typeTransfer);
        }
    }
}