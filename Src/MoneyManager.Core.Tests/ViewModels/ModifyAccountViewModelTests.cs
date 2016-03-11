using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Localization;
using Moq;

namespace MoneyManager.Core.Tests.ViewModels
{
    [TestClass]
    public class ModifyAccountViewModelTests
    {
        [TestMethod]
        public void Title_EditAccount_CorrectTitle()
        {
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2, Name = accountname});

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object)
            {IsEdit = true};

            viewmodel.Title.ShouldBe(Strings.EditLabel + " " + accountname);
        }

        [TestMethod]
        public void Title_AddAccount_CorrectTitle()
        {
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2, Name = accountname});

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object)
            {IsEdit = false};

            viewmodel.Title.ShouldBe(Strings.AddAccountTitle);
        }
    }
}