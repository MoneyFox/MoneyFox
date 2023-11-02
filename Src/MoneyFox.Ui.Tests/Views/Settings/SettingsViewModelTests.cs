namespace MoneyFox.Ui.Tests.Views.Settings;

using System.Collections.Immutable;
using System.Globalization;
using Core.Common.Settings;
using Core.Queries;
using Domain;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Ui.Views.Settings;

public class SettingsViewModelTests
{
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;
    private readonly SettingsViewModel viewModel;

    private SettingsViewModelTests()
    {
        settingsFacade = Substitute.For<ISettingsFacade>();
        mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(ImmutableList<Account>.Empty);
        viewModel = new(settingsFacade: settingsFacade, mediator: mediator);
    }

    public sealed class Initialize : SettingsViewModelTests
    {
        [Fact]
        public async Task SetCurrencyFromSettings()
        {
            // Arrange
            settingsFacade.DefaultCurrency.Returns("USD");

            // Act
            await viewModel.InitializeAsync();

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
            viewModel.SelectedCurrency.AlphaIsoCode.Should().Be("USD");
        }

        [Fact]
        public async Task SetCurrencyFromRegion_WhenSettingNotSetYet()
        {
            // Act
            await viewModel.InitializeAsync();

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
            viewModel.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
        }

        [Fact]
        public async Task SetAccountFromSettings()
        {
            // Arrange
            var accounts = new List<Account> { new("Acc1"), new("Acc2") };
            settingsFacade.DefaultAccount.Returns(accounts[1].Id);
            mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);

            // Act
            await viewModel.InitializeAsync();

            // Assert
            viewModel.AvailableAccounts.Should().NotBeNull();
            viewModel.SelectedAccount!.Id.Should().Be(accounts[1].Id);
        }
    }

    public sealed class CurrencySelected : SettingsViewModelTests
    {
        [Fact]
        public void UpdateSettingsOnSet()
        {
            // Act
            var newCurrency = new CurrencyViewModel(Currencies.CHF.AlphaIsoCode);
            viewModel.SelectedCurrency = newCurrency;

            // Assert
            settingsFacade.DefaultCurrency.Should().Be(newCurrency.AlphaIsoCode);
        }
    }

    public sealed class AccountSelected : SettingsViewModelTests
    {
        [Fact]
        public void UpdateAccountOnSet()
        {
            // Arrange
            var accounts = new List<Account> { new("Acc1"), new("Acc2") };
            mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);

            // Act
            var newAccount = new Account("Acc3");
            var defaultAccount = new AccountLiteViewModel(Id: newAccount.Id, Name: newAccount.Name);
            viewModel.SelectedAccount = defaultAccount;

            // Assert
            settingsFacade.DefaultAccount.Should().Be(defaultAccount.Id);
        }
    }
}
