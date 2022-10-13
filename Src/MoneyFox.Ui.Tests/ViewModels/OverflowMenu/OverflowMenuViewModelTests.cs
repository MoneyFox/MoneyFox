namespace MoneyFox.Ui.Tests.ViewModels.OverflowMenu;

using System.Threading.Tasks;
using MoneyFox.Core.Interfaces;
using MoneyFox.Ui.Views.OverflowMenu;
using NSubstitute;
using Views.About;
using Views.Backup;
using Views.Categories;
using Views.Settings;
using Xunit;

public class OverflowMenuViewModelTests
{
    [Fact]
    public async Task ShouldNavigateToCategoryPage_WhenCommandIsCalledWithCategoryItem()
    {
        // Capture
        INavigationService navigationService = Substitute.For<INavigationService>();

        // Arrange
        OverflowItemViewModel overflowItem = new()
        { Name = "Some Name", Type = OverflowMenuItemType.Categories };

        // Act
        OverflowMenuViewModel viewModel = new(navigationService);
        await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

        // Assert
        await navigationService.Received(1).NavigateToAsync<CategoryListPage>();
    }

    [Fact]
    public async Task ShouldNavigateToBackupPage_WhenCommandIsCalledWithBackupItem()
    {
        // Capture
        INavigationService navigationService = Substitute.For<INavigationService>();

        // Arrange
        OverflowItemViewModel overflowItem = new()
        { Name = "Some Name", Type = OverflowMenuItemType.Backup };

        // Act
        OverflowMenuViewModel viewModel = new(navigationService);
        await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

        // Assert
        await navigationService.Received(1).NavigateToAsync<BackupPage>();
    }

    [Fact]
    public async Task ShouldNavigateToSettingsPage_WhenCommandIsCalledWithSettingsMenuItem()
    {
        // Capture
        INavigationService navigationService = Substitute.For<INavigationService>();

        // Arrange
        OverflowItemViewModel overflowItem = new()
        { Name = "Some Name", Type = OverflowMenuItemType.Settings };

        // Act
        OverflowMenuViewModel viewModel = new(navigationService);
        await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

        // Assert
        await navigationService.Received(1).NavigateToAsync<SettingsPage>();
    }

    [Fact]
    public async Task ShouldNavigateToAboutPage_WhenCommandIsCalledWithAboutMenuItem()
    {
        // Capture
        INavigationService navigationService = Substitute.For<INavigationService>();

        // Arrange
        OverflowItemViewModel overflowItem = new()
        { Name = "Some Name", Type = OverflowMenuItemType.About };

        // Act
        OverflowMenuViewModel viewModel = new(navigationService);
        await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

        // Assert
        await navigationService.Received(1).NavigateToAsync<AboutPage>();
    }
}

