using FluentAssertions;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Ui.Shared.ViewModels.Settings;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace MoneyFox.Ui.Shared.Tests.ViewModels.Settings
{
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
            var viewModel = new SettingsViewModel(settingsFacade, dialogService);

            // Assert
            viewModel.AvailableCultures.Should().NotBeNull();
        }

        [Fact]
        public void UpdateSettingsOnSet()
        {
            // Arrange
            ISettingsFacade settingsFacade = Substitute.For<ISettingsFacade>();
            IDialogService dialogService = Substitute.For<IDialogService>();
            var viewModel = new SettingsViewModel(settingsFacade, dialogService);

            // Act
            var newCulture = new CultureInfo("de-CH");
            viewModel.SelectedCulture = newCulture;

            // Assert
            settingsFacade.Received(1).DefaultCulture = newCulture.Name;
        }
    }
}
