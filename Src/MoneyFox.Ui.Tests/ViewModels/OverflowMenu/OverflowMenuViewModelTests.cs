namespace MoneyFox.Tests.Presentation.ViewModels.OverflowMenu
{

    using System.Threading.Tasks;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.ViewModels.OverflowMenu;
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
            var navigationService = Substitute.For<INavigationService>();

            // Arrange
            var overflowItem = new OverflowItemViewModel { Name = "Some Name", Type = OverflowMenuItemType.Categories };

            // Act
            var viewModel = new OverflowMenuViewModel(navigationService);
            await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

            // Assert
            await navigationService.Received(1).NavigateTo<CategoryListPage>();
        }

        [Fact]
        public async Task ShouldNavigateToBackupPage_WhenCommandIsCalledWithBackupItem()
        {
            // Capture
            var navigationService = Substitute.For<INavigationService>();

            // Arrange
            var overflowItem = new OverflowItemViewModel { Name = "Some Name", Type = OverflowMenuItemType.Backup };

            // Act
            var viewModel = new OverflowMenuViewModel(navigationService);
            await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

            // Assert
            await navigationService.Received(1).NavigateTo<BackupPage>();
        }

        [Fact]
        public async Task ShouldNavigateToSettingsPage_WhenCommandIsCalledWithSettingsMenuItem()
        {
            // Capture
            var navigationService = Substitute.For<INavigationService>();

            // Arrange
            var overflowItem = new OverflowItemViewModel { Name = "Some Name", Type = OverflowMenuItemType.Settings };

            // Act
            var viewModel = new OverflowMenuViewModel(navigationService);
            await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

            // Assert
            await navigationService.Received(1).NavigateTo<SettingsPage>();
        }

        [Fact]
        public async Task ShouldNavigateToAboutPage_WhenCommandIsCalledWithAboutMenuItem()
        {
            // Capture
            var navigationService = Substitute.For<INavigationService>();

            // Arrange
            var overflowItem = new OverflowItemViewModel { Name = "Some Name", Type = OverflowMenuItemType.About };

            // Act
            var viewModel = new OverflowMenuViewModel(navigationService);
            await viewModel.GoToSelectedItemCommand.ExecuteAsync(overflowItem);

            // Assert
            await navigationService.Received(1).NavigateTo<AboutPage>();
        }
    }

}
