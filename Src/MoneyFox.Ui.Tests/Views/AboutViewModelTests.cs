namespace MoneyFox.Ui.Tests.Views;

using Core.Common.Interfaces;
using Core.Interfaces;
using NSubstitute;
using Ui.Views.About;
using Xunit;

public class AboutViewModelTests
{
    private readonly AboutViewModel aboutViewModel;
    private readonly IBrowserAdapter browserAdapter;

    public AboutViewModelTests()
    {
        browserAdapter = Substitute.For<IBrowserAdapter>();
        var emailAdapter = Substitute.For<IEmailAdapter>();
        var toastService = Substitute.For<IToastService>();
        aboutViewModel = new(emailAdapter: emailAdapter, browserAdapter: browserAdapter, toastService: toastService);
    }

    [Fact]
    public async Task GoToWebsite_NoParams_Called()
    {
        // Arrange
        _ = browserAdapter.OpenWebsiteAsync(Arg.Any<Uri>()).Returns(Task.CompletedTask);

        // Act
        await aboutViewModel.GoToWebsiteCommand.ExecuteAsync(null);

        // Assert
        await browserAdapter.Received(1).OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri("https://www.apply-solutions.ch")));
    }

    [Fact]
    public async Task GoToRepository_NoParams_CommandCalled()
    {
        // Arrange
        _ = browserAdapter.OpenWebsiteAsync(Arg.Any<Uri>()).Returns(Task.CompletedTask);

        // Act
        await aboutViewModel.GoToRepositoryCommand.ExecuteAsync(null);

        // Assert
        await browserAdapter.Received(1).OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri("https://github.com/MoneyFox/MoneyFox")));
    }
}
