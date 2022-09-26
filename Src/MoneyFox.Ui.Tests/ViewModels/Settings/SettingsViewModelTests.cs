namespace MoneyFox.Ui.Tests.ViewModels.Settings;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using MoneyFox.Core.Common.Facades;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Ui.ViewModels.Settings;
using NSubstitute;
using Xunit;

[ExcludeFromCodeCoverage]
public class SettingsViewModelTests
{
    [Fact]
    public void CollectionNotNullAfterCtor()
    {
        // Arrange
        ISettingsFacade settingsFacade = Substitute.For<ISettingsFacade>();
        IDialogService dialogService = Substitute.For<IDialogService>();

        // Act
        var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, dialogService: dialogService);

        // Assert
        viewModel.AvailableCultures.Should().NotBeNull();
    }

    [Fact]
    public void UpdateSettingsOnSet()
    {
        // Arrange
        ISettingsFacade settingsFacade = Substitute.For<ISettingsFacade>();
        IDialogService dialogService = Substitute.For<IDialogService>();
        var viewModel = new SettingsViewModel(settingsFacade: settingsFacade, dialogService: dialogService);

        // Act
        CultureInfo newCulture = new("de-CH");
        viewModel.SelectedCulture = newCulture;

        // Assert
        settingsFacade.Received(1).DefaultCulture = newCulture.Name;
    }
}

