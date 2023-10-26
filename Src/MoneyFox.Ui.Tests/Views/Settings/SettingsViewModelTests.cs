namespace MoneyFox.Ui.Tests.Views.Settings;

using System.Globalization;
using Core.Common.Settings;
using Core.Queries;
using Domain;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Ui.Views.Settings;

public static class SettingsViewModelTests
{
    public sealed class Constructor
    {
        [Fact]
        public void CollectionNotNullAfterCreation()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();

            // Act
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, mediator: mediator);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
        }

        [Fact]
        public void SetCurrencyFromSettings()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();
            settingsFacade.DefaultCurrency.Returns("USD");

            // Act
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, mediator: mediator);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
            viewModel.SelectedCurrency.AlphaIsoCode.Should().Be("USD");
        }

        [Fact]
        public void SetCurrencyFromRegion_WhenSettingNotSetYet()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();

            // Act
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, mediator: mediator);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
            viewModel.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
        }

        [Fact]
        public async Task SetAccountFromSettings()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();
            var accounts = new List<Account> { new("Acc1"), new("Acc2") };
            settingsFacade.DefaultAccount.Returns(accounts[1].Id);
            mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);

            // Act
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, mediator: mediator);
            await viewModel.InitializeAsync();

            // Assert
            viewModel.AvailableAccounts.Should().NotBeNull();
            viewModel.SelectedAccount!.Id.Should().Be(accounts[1].Id);
        }
    }

    public sealed class CurrencySelected
    {
        [Fact]
        public void UpdateSettingsOnSet()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, mediator: mediator);

            // Act
            var newCurrency = new CurrencyViewModel(Currencies.CHF.AlphaIsoCode);
            viewModel.SelectedCurrency = newCurrency;

            // Assert
            settingsFacade.DefaultCurrency.Should().Be(newCurrency.AlphaIsoCode);
        }
    }

    public sealed class AccountSelected
    {
        [Fact]
        public void UpdateAccountOnSet()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();
            var accounts = new List<Account> { new("Acc1"), new("Acc2") };
            mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, mediator: mediator);

            // Act
            var newAccount = new Account("Acc3");
            var defaultAccount = new AccountLiteViewModel(Id: newAccount.Id, Name: newAccount.Name);
            viewModel.SelectedAccount = defaultAccount;

            // Assert
            settingsFacade.DefaultAccount.Should().Be(defaultAccount.Id);
        }
    }
}
