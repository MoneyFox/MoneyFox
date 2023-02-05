namespace MoneyFox.Ui.Tests.Views.Settings;

using System.Globalization;
using FluentAssertions;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Settings;
using NSubstitute;
using Ui.Views.Settings;
using Xunit;

public class SettingsViewModelTests
{
    [Fact]
    public void CollectionNotNullAfterCtor()
    {
        // Arrange
        var settingsFacade = Substitute.For<ISettingsFacade>();
        var dialogService = Substitute.For<IDialogService>();

        // Act
        var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, dialogService: dialogService);

        // Assert
        viewModel.AvailableCurrencies.Should().NotBeNull();
    }

    [Fact]
    public void UpdateSettingsOnSet()
    {
        // Arrange
        var settingsFacade = Substitute.For<ISettingsFacade>();
        var dialogService = Substitute.For<IDialogService>();
        var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, dialogService: dialogService);

        // Act
        CultureInfo newCulture = new("de-CH");
        viewModel.SelectedCurrency = newCulture;

        // Assert
        settingsFacade.Received(1).DefaultCulture = newCulture.Name;
    }
}
