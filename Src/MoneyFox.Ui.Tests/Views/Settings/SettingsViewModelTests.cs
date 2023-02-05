namespace MoneyFox.Ui.Tests.Views.Settings;

using Core.Common.Settings;
using Domain;
using FluentAssertions;
using NSubstitute;
using Ui.Views.Settings;
using Xunit;

public class SettingsViewModelTests
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
