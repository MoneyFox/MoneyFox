using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.WindowsPhone.Test.Mocks;

namespace MoneyManager.Business.WindowsPhone.Test.Repositories {
    [TestClass]
    public class RecurringTransactionRepositoryTest {
        private RecurringTransactionDataAccessMock _recurringTransactionDataAccessMock;

        [TestInitialize]
        public void Init() {
            _recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
        }


    }
}
