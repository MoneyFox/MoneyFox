using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MoneyManager.Business.WindowsPhone.Test.Logic
{
  [TestClass]
  public class TransactionLogicTest{
    [TestInitialize]
    public void TestInit(){
      new ViewModelLocator();
    }

    [TestMethod]
    [Ignore]
    public async void SaveTransactionTest(){
      Assert.IsTrue(false);
    }

    [TestMethod]
    [Ignore]
    public void PrepareEditTest(){
      Assert.IsTrue(false);
    }

    [TestMethod]
    [Ignore]
    public async void DeleteTransactionTest(){
      Assert.IsTrue(false);
    }

    [TestMethod]
    [Ignore]
    public void DeleteAssociatedTransactionsFromDatabaseTest(){
      Assert.IsTrue(false);
    }

    [TestMethod]
    [Ignore]
    public async void UpdateTransactionTest(){
      Assert.IsTrue(false);
    }

    [TestMethod]
    [Ignore]
    public async void ClearTransactionsTest(){
      Assert.IsTrue(false);
    }
  }
}
