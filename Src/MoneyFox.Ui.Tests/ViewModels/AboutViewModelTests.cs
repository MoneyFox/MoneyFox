namespace MoneyFox.Ui.Tests.ViewModels;

using System.Diagnostics.CodeAnalysis;
using Core.Common.Interfaces;
using Core.Interfaces;
using FluentAssertions;
using NSubstitute;
using Views.About;
using Xunit;

[ExcludeFromCodeCoverage]
public class AboutViewModelTests
{
    private AboutViewModel aboutViewModel;

    private readonly IAppInformation appInformation;
    private readonly IBrowserAdapter browserAdapter;
    private readonly IEmailAdapter emailAdapter;
    private readonly IStoreOperations storeOperations;
    private readonly IToastService toastService;

    public AboutViewModelTests()
    {
        appInformation = Substitute.For<IAppInformation>();
        browserAdapter = Substitute.For<IBrowserAdapter>();
        emailAdapter = Substitute.For<IEmailAdapter>();
        storeOperations = Substitute.For<IStoreOperations>();
        toastService = Substitute.For<IToastService>();

        aboutViewModel = new(
            appInformation: appInformation,
            emailAdapter: emailAdapter,
            browserAdapter: browserAdapter,
            storeOperations: storeOperations,
            toastService);

    }

    [Fact]
    public void Version_NoParams_ReturnCorrectMail()
    {
        // Arrange
        _ = appInformation.GetVersion.Returns("42");

        // Assert
        aboutViewModel.Version.Should()
            .Be("42");
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

    [Fact]
    public void RateApp_NoParams_CommandCalled()
    {
        // Act
        aboutViewModel.RateAppCommand.Execute(null);

        // Assert
        storeOperations.Received(1).RateApp();
    }
}
