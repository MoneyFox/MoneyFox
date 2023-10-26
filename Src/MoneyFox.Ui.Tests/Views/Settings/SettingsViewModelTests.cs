namespace MoneyFox.Ui.Tests.Views.Settings;

using System.Globalization;
using Core.Common.Settings;
using Domain;
using FluentAssertions;
using MediatR;
using MoneyFox.Core.Queries;
using MoneyFox.Domain.Aggregates.AccountAggregate;
using NSubstitute;
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
            var viewModel = new SettingsViewModel(settingsFacade, mediator);

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
            var viewModel = new SettingsViewModel(settingsFacade, mediator);

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
            var viewModel = new SettingsViewModel(settingsFacade, mediator);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
            viewModel.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
        }


        [Fact]
        public void SetAccountFromSettings()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();
            var accounts = new List<Account>
            {
                new Account("Acc1"),
                new Account ("Acc2")
            };
            settingsFacade.DefaultAccount.Returns(accounts[1].Id);
            mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);

            // Act
            var viewModel = new SettingsViewModel(settingsFacade, mediator);
            viewModel.LoadAccounts().Wait();

            // Assert
            viewModel.AvailableAccounts.Should().NotBeNull();
            viewModel.SelectedAccount.Id.Should().Be(accounts[1].Id);
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
            var viewModel = new SettingsViewModel(settingsFacade, mediator);

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
            var accounts = new List<Account>
            {
                new Account("Acc1"),
                new Account ("Acc2")
            };
            mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);
            var viewModel = new SettingsViewModel(settingsFacade, mediator);

            // Act
            var newAccount = new Account("Acc3");
            var defaultAccount = new AccountLiteViewModel(newAccount.Id, newAccount.Name);
            viewModel.SelectedAccount = defaultAccount;

            // Assert
            settingsFacade.DefaultAccount.Should().Be(defaultAccount.Id);
        }
    }
}
