using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class AccountListViewModelTests : MvxIoCSupportingTest
    {
        public AccountListViewModelTests()
        {
            if (MvxBindingSingletonCache.Instance == null)
            {
                Setup();
            }
        }

        [Fact]
        public void EditAccount_NoParams_PropertiesSet()
        {
            new AccountListViewModel(new Mock<IRepository<Account>>().Object);
        }
    }
}
