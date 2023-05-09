namespace MoneyFox.Ui.Tests.Views.Settings;

using System.Globalization;
using Core.Common.Settings;
using Domain;
using FluentAssertions;
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

            // Act
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
        }

        [Fact]
        public void SetCurrencyFromSettings()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            settingsFacade.DefaultCurrency.Returns("USD");

            // Act
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
            viewModel.SelectedCurrency.AlphaIsoCode.Should().Be("USD");
        }

        [Fact]
        public void SetCurrencyFromRegion_WhenSettingNotSetYet()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();

            // Act
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade);

            // Assert
            viewModel.AvailableCurrencies.Should().NotBeNull();
            viewModel.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
        }
    }

    public sealed class CurrencySelected
    {
        [Fact]
        public void UpdateSettingsOnSet()
        {
            // Arrange
            var settingsFacade = Substitute.For<ISettingsFacade>();
            var viewModel = new SettingsViewModel(settingsFacade: settingsFacade);

            // Act
            var newCurrency = new CurrencyViewModel(Currencies.CHF.AlphaIsoCode);
            viewModel.SelectedCurrency = newCurrency;

            // Assert
            settingsFacade.DefaultCurrency.Should().Be(newCurrency.AlphaIsoCode);
        }
    }
}
