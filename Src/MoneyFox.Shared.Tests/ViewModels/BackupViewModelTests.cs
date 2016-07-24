using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels {
    public class BackupViewModelTests : MvxIoCSupportingTest {
        public BackupViewModelTests()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        public void Loaded_NoConnectivity_NothingCalled()
        {
            
        }
    }
}
