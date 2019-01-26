using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class ModifyAccountViewModelTests
    {
        [Theory]
        [InlineData("Test AccountViewModel", "Test AccountViewModel")]
        [InlineData("Test AccountViewModel", "TESt AccountViewModel")]
        public void SaveCommand_DoesNotAllowDuplicateNames(string name1, string name2)
        {
            // Arrange
            var accountList = new List<Account>();

            var accountRepositorySetup = new Mock<IAccountService>();
            accountRepositorySetup.Setup(c => c.GetExcludedAccounts()).ReturnsAsync(new List<Account>());
            accountRepositorySetup.Setup(c => c.GetNotExcludedAccounts())
                                  .ReturnsAsync(accountList);
            accountRepositorySetup.Setup(c => c.SaveAccount(It.IsAny<Account>()))
                                  .Callback((Account acc) => { accountList.Add(acc); });

            var account = new Account
            {
                Data =
                {
                    Id = 1,
                    Name = name1
                }
            };
            var accountViewModel = new AccountViewModel(account)
            {
                Name = name2
            };

            accountList.Add(account);

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountService>().Object,
                                                       new Mock<ISettingsManager>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = false,
                SelectedAccount = accountViewModel
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            Assert.Single(accountList);
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
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var account = fixture.Create<AccountViewModel>();

            var accountRepositorySetup = new Mock<IAccountService>();
            accountRepositorySetup.Setup(x => x.SaveAccount(account.Account));
            accountRepositorySetup.Setup(c => c.GetNotExcludedAccounts()).ReturnsAsync(new List<Account>());
            accountRepositorySetup.Setup(c => c.GetExcludedAccounts()).ReturnsAsync(new List<Account>());

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountService>().Object,
                                                       new Mock<ISettingsManager>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
            {
                SelectedAccount = account
            };

            // Act
            viewmodel.AmountString = amount;

            // Assert
            viewmodel.AmountString.ShouldEqual(convertedAmount.ToString("N", CultureInfo.CurrentCulture));
        }

        [Fact]
        public void Save_UpdateTimeStamp()
        {
            // Arrange
            var account = new AccountViewModel(new Account {Data = {Id = 0, Name = "AccountViewModel"}});

            var accountRepositorySetup = new Mock<IAccountService>();
            accountRepositorySetup.Setup(x => x.SaveAccount(account.Account));
            accountRepositorySetup.Setup(c => c.GetNotExcludedAccounts()).ReturnsAsync(new List<Account>());
            accountRepositorySetup.Setup(c => c.GetExcludedAccounts()).ReturnsAsync(new List<Account>());

            var localDateSetting = DateTime.MinValue;
            var settingsManagerMock = new Mock<ISettingsManager>();
            settingsManagerMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>())
                               .Callback((DateTime x) => localDateSetting = x);

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountService>().Object,
                                                       settingsManagerMock.Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
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

        [Fact]
        public void SaveCommand_SavesAccount()
        {
            // Arrange
            var accountList = new List<Account>();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(c => c.GetExcludedAccounts()).ReturnsAsync(new List<Account>());
            accountServiceMock.Setup(c => c.GetNotExcludedAccounts())
                              .ReturnsAsync(new List<Account>());
            accountServiceMock.Setup(c => c.SaveAccount(It.IsAny<Account>()))
                              .Callback((Account acc) => { accountList.Add(acc); })
                              .Returns(Task.CompletedTask);

            var account = new AccountViewModel(new Account())
            {
                Name = "Test AccountViewModel"
            };

            var viewmodel = new ModifyAccountViewModel(accountServiceMock.Object,
                                                       new Mock<ISettingsManager>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = false,
                SelectedAccount = account
            };

            // Act
            viewmodel.SaveCommand.Execute();

            // Assert
            Assert.Single(accountList);
        }

        [Fact]
        public void Title_AddAccount_CorrectTitle()
        {
            // Arrange
            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountService>().Object,
                                                       new Mock<ISettingsManager>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
                {IsEdit = false};

            // Act / Assert
            viewmodel.Title.ShouldEqual(Strings.AddAccountTitle);
        }

        [Fact]
        public void Title_EditAccount_CorrectTitle()
        {
            // Arrange
            var accountname = "Sparkonto";

            var viewmodel = new ModifyAccountViewModel(new Mock<IAccountService>().Object,
                                                       new Mock<ISettingsManager>().Object,
                                                       new Mock<IBackupManager>().Object,
                                                       new Mock<IDialogService>().Object,
                                                       new Mock<IMvxLogProvider>().Object,
                                                       new Mock<IMvxNavigationService>().Object)
            {
                IsEdit = true,
                SelectedAccount = new AccountViewModel(new Account()) {Id = 3, Name = accountname}
            };

            // Act / Assert
            viewmodel.Title.ShouldEqual(string.Format(Strings.EditAccountTitle, accountname));
        }
    }
}