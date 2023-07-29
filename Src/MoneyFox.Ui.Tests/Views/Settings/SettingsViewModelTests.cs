namespace MoneyFox.Ui.Tests.Views.Settings;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Core.Common.Settings;
using Domain;
using FluentAssertions;
using MediatR;
using MoneyFox.Domain.Aggregates.AccountAggregate;
using MoneyFox.Ui.Views.Accounts.AccountModification;
using NSubstitute;
using Ui.Views.Settings;
using Xunit;

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
            var mapper = Substitute.For<IMapper>();

            // Act
            var viewModel = new SettingsViewModel(settingsFacade, mediator, mapper);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
        }

        [Fact]
        public void SetCurrencyFromSettings()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var mediator = Substitute.For<IMediator>();
            var mapper = Substitute.For<IMapper>();
            settingsFacade.DefaultCurrency.Returns("USD");

            // Act
            var viewModel = new SettingsViewModel(settingsFacade, mediator, mapper);

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
            var mapper = Substitute.For<IMapper>();

            // Act
            var viewModel = new SettingsViewModel(settingsFacade, mediator, mapper);

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
            var mapper = Substitute.For<IMapper>();
            settingsFacade.DefaultAccount.Returns("Acc2");
            var accountViewModels = new ObservableCollection<AccountViewModel>
            {
                new AccountViewModel {Name = "Acc1"},
                new AccountViewModel{ Name = "Acc2"}
            };
            mapper.Map<ObservableCollection<AccountViewModel>>(Arg.Any<List<Account>>()).Returns(accountViewModels);

            // Act
            var viewModel = new SettingsViewModel(settingsFacade, mediator, mapper);

            // Assert
            viewModel.AvailableAccounts.Should().NotBeNull();
            viewModel.SelectedAccount.Name.Should().Be("Acc2");
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
            var mapper = Substitute.For<IMapper>();
            var viewModel = new SettingsViewModel(settingsFacade, mediator, mapper);

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
            var mapper = Substitute.For<IMapper>();
            var viewModel = new SettingsViewModel(settingsFacade, mediator, mapper);
            var accountViewModels = new ObservableCollection<AccountViewModel>
            {
                new AccountViewModel {Name = "Acc1"},
                new AccountViewModel{ Name = "Acc2"}
            };
            mapper.Map<ObservableCollection<AccountViewModel>>(Arg.Any<List<Account>>()).Returns(accountViewModels);

            // Act
            var newAccount = new AccountViewModel { Name = "Acc1" };
            viewModel.SelectedAccount = newAccount;

            // Assert
            settingsFacade.DefaultAccount.Should().Be(newAccount.Name);
        }
    }
}
