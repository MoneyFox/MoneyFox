using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Resources;
using MoneyFox.Foundation.Tests;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;
using Ploeh.AutoFixture;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class ModifyAccountViewModelTests : MvxIoCSupportingTest
    {
        public ModifyAccountViewModelTests()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [Fact]
        public void Title_EditAccount_CorrectTitle()
        {            
            // Arrange
            var accountname = "Sparkonto";

            var settingsManagerMock = new Mock<ISettingsManager>();

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountRepository>().Object, 
                new Mock<IDialogService>().Object, 
                settingsManagerMock.Object,
                new Mock<IBackupManager>().Object)
            {
                IsEdit = true,
                SelectedAccount = new AccountViewModel {Id = 3, Name = accountname}
            };

            // Act / Assert
            viewmodel.Title.ShouldBe(string.Format(Strings.EditAccountTitle, accountname));
        }

        [Fact]
        public void Title_AddAccount_CorrectTitle()
        {
            // Arrange
            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object,
                new Mock<ISettingsManager>().Object,
                new Mock<IBackupManager>().Object)
            { IsEdit = false};

            // Act / Assert
            viewmodel.Title.ShouldBe(Strings.AddAccountTitle);
        }

        [Theory]
        [InlineData("Test AccountViewModel", "Test AccountViewModel")]
        [InlineData("Test AccountViewModel", "TESt AccountViewModel")]
        public void SaveCommand_DoesNotAllowDuplicateNames(string name1, string name2)
        {
            // Arrange
            var accountList = new List<AccountViewModel>();
            var settingsManagerMock = new Mock<ISettingsManager>();

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(c => c.GetList(It.IsAny<Expression<Func<AccountViewModel, bool>>>()))
                .Returns(accountList);
            accountRepositorySetup.Setup(c => c.Save(It.IsAny<AccountViewModel>()))
                .Callback((AccountViewModel acc) => { accountList.Add(acc); });

            var account = new AccountViewModel
            {
                Id = 1,
                Name = "Test AccountViewModel"
            };
            var newAccount = new AccountViewModel
            {
                Name = "Test AccountViewModel"
            };
            accountList.Add(account);
            
            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, 
                new Mock<IDialogService>().Object, 
                settingsManagerMock.Object,
                new Mock<IBackupManager>().Object)
            {
                IsEdit = false,
                SelectedAccount = newAccount
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            Assert.Equal(1, accountList.Count);
        }

        [Fact]
        public void SaveCommand_SavesAccount()
        {
            // Arrange
            var accountList = new List<AccountViewModel>();

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(c => c.GetList(It.IsAny<Expression<Func<AccountViewModel, bool>>>()))
                .Returns(accountList);
            accountRepositorySetup.Setup(c => c.Save(It.IsAny<AccountViewModel>()))
                .Callback((AccountViewModel acc) => { accountList.Add(acc); });

            var settingsManagerMock = new Mock<ISettingsManager>();

            var account = new AccountViewModel
            {
                Id = 1,
                Name = "Test AccountViewModel"
            };

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object, 
                new Mock<IDialogService>().Object, 
                settingsManagerMock.Object,
                new Mock<IBackupManager>().Object)
            {
                IsEdit = false,
                SelectedAccount = account
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            Assert.Equal(1, accountList.Count);
        }

        [Fact]
        public void Save_UpdateTimeStamp()
        {
            // Arrange
            var account = new AccountViewModel {Id = 0, Name = "AccountViewModel"};

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(x => x.Save(account)).Returns(true);
            accountRepositorySetup.Setup(x => x.GetList(null)).Returns(() => new List<AccountViewModel>());

            var localDateSetting = DateTime.MinValue;
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>())
                .Callback((DateTime x) => localDateSetting = x);

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object,
                new Mock<IDialogService>().Object, 
                settingsManagerMock.Object,
                new Mock<IBackupManager>().Object)
            {
                IsEdit = false,
                SelectedAccount = account
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }

        [Theory]
        [InlineData("35", 35, "de-CH")]
        [InlineData("35.5", 35.5, "de-CH")]
        [InlineData("35,5", 35.5, "de-CH")]
        [InlineData("35.50", 35.5, "de-CH")]
        [InlineData("35,50", 35.5, "de-CH")]
        [InlineData("3,500.5", 3500.5, "de-CH")]
        [InlineData("3,500.50", 3500.5, "de-CH")]
        [InlineData("3.500,5", 3500.5, "de-CH")]
        [InlineData("3.500,50", 3500.5, "de-CH")]
        [InlineData("35", 35, "de-DE")]
        [InlineData("35,5", 35.5, "de-DE")]
        [InlineData("35,50", 35.5, "de-DE")]
        [InlineData("35.5", 35.5, "de-DE")]
        [InlineData("35.50", 35.5, "de-DE")]
        [InlineData("3,500.5", 3500.5, "de-DE")]
        [InlineData("3,500.50", 3500.5, "de-DE")]
        [InlineData("3.500,5", 3500.5, "de-DE")]
        [InlineData("3.500,50", 3500.5, "de-DE")]
        [InlineData("35", 35, "en-GB")]
        [InlineData("35,5", 35.5, "en-GB")]
        [InlineData("35,50", 35.5, "en-GB")]
        [InlineData("35.5", 35.5, "en-GB")]
        [InlineData("35.50", 35.5, "en-GB")]
        [InlineData("3,500.5", 3500.5, "en-GB")]
        [InlineData("3,500.50", 3500.5, "en-GB")]
        [InlineData("3.500,5", 3500.5, "en-GB")]
        [InlineData("3.500,50", 3500.5, "en-GB")]
        [InlineData("35", 35, "en-US")]
        [InlineData("35,5", 35.5, "en-US")]
        [InlineData("35,50", 35.5, "en-US")]
        [InlineData("35.5", 35.5, "en-US")]
        [InlineData("35.50", 35.5, "en-US")]
        [InlineData("3,500.5", 3500.5, "en-US")]
        [InlineData("3,500.50", 3500.5, "en-US")]
        [InlineData("3.500,5", 3500.5, "en-US")]
        [InlineData("3.500,50", 3500.5, "en-US")]
        [InlineData("35", 35, "it-IT")]
        [InlineData("35,5", 35.5, "it-IT")]
        [InlineData("35,50", 35.5, "it-IT")]
        [InlineData("35.5", 35.5, "it-IT")]
        [InlineData("35.50", 35.5, "it-IT")]
        [InlineData("3,500.5", 3500.5, "it-IT")]
        [InlineData("3,500.50", 3500.5, "it-IT")]
        [InlineData("3.500,5", 3500.5, "it-IT")]
        public void AmountString(string amount, double convertedAmount, string culture)
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture, false);
            var account = new Fixture().Create<AccountViewModel>();

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(x => x.Save(account)).Returns(true);
            accountRepositorySetup.Setup(x => x.GetList(null)).Returns(() => new List<AccountViewModel>());

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object,
                new Mock<IDialogService>().Object,
                new Mock<ISettingsManager>().Object,
                new Mock<IBackupManager>().Object)
            {
                SelectedAccount = account
            };

            // Act
            viewmodel.AmountString = amount;

            // Assert
            viewmodel.AmountString.ShouldBe(convertedAmount.ToString("N", CultureInfo.CurrentCulture));
        }

    }
}