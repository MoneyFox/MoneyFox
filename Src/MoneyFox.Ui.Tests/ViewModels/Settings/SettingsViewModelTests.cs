namespace MoneyFox.Ui.Tests.ViewModels.Settings;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Core.Common.Facades;
using Core.Common.Interfaces;
using FluentAssertions;
using NSubstitute;
using Views.Settings;
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
        viewModel.AvailableCultures.Should().NotBeNull();
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
        viewModel.SelectedCulture = newCulture;

        // Assert
        settingsFacade.Received(1).DefaultCulture = newCulture.Name;
    }
}
