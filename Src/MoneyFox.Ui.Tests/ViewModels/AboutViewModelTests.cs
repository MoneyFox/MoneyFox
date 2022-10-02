namespace MoneyFox.Ui.Tests.ViewModels;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Ui.Views.About;
using NSubstitute;
using Xunit;

[ExcludeFromCodeCoverage]
public class AboutViewModelTests
{
    [Fact]
    public void SupportMail_NoParams_ReturnCorrectMail()
    {
        new AboutViewModel(
                appInformation: Substitute.For<IAppInformation>(),
                emailAdapter: Substitute.For<IEmailAdapter>(),
                browserAdapter: Substitute.For<IBrowserAdapter>(),
                storeOperations: Substitute.For<IStoreOperations>()).SupportMail.Should()
            .Be("mobile.support@apply-solutions.ch");
    }

    [Fact]
    public void Website_NoParams_ReturnCorrectMail()
    {
        new AboutViewModel(
                appInformation: Substitute.For<IAppInformation>(),
                emailAdapter: Substitute.For<IEmailAdapter>(),
                browserAdapter: Substitute.For<IBrowserAdapter>(),
                storeOperations: Substitute.For<IStoreOperations>()).Website.Should()
            .Be("https://www.apply-solutions.ch");
    }

    [Fact]
    public void Version_NoParams_ReturnCorrectMail()
    {
        IAppInformation appinfos = Substitute.For<IAppInformation>();
        _ = appinfos.GetVersion.Returns("42");
        new AboutViewModel(
                appInformation: appinfos,
                emailAdapter: Substitute.For<IEmailAdapter>(),
                browserAdapter: Substitute.For<IBrowserAdapter>(),
                storeOperations: Substitute.For<IStoreOperations>()).Version.Should()
            .Be("42");
    }

    [Fact]
    public async Task GoToWebsite_NoParams_Called()
    {
        IBrowserAdapter webBrowserTaskSetup = Substitute.For<IBrowserAdapter>();
        _ = webBrowserTaskSetup.OpenWebsiteAsync(Arg.Any<Uri>()).Returns(Task.CompletedTask);
        new AboutViewModel(
            appInformation: Substitute.For<IAppInformation>(),
            emailAdapter: Substitute.For<IEmailAdapter>(),
            browserAdapter: webBrowserTaskSetup,

            // ReSharper disable once MethodHasAsyncOverload
            storeOperations: Substitute.For<IStoreOperations>()).GoToWebsiteCommand.Execute(null);

        await webBrowserTaskSetup.Received(1).OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri("https://www.apply-solutions.ch")));
    }

    [Fact]
    public async Task GoToRepository_NoParams_CommandCalled()
    {
        IBrowserAdapter webbrowserTaskSetup = Substitute.For<IBrowserAdapter>();
        _ = webbrowserTaskSetup.OpenWebsiteAsync(Arg.Any<Uri>()).Returns(Task.CompletedTask);
        new AboutViewModel(
            appInformation: Substitute.For<IAppInformation>(),
            emailAdapter: Substitute.For<IEmailAdapter>(),
            browserAdapter: webbrowserTaskSetup,
            storeOperations: Substitute.For<IStoreOperations>()).GoToRepositoryCommand.Execute(null);

        await webbrowserTaskSetup.Received(1).OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri("https://github.com/MoneyFox/MoneyFox")));
    }

    [Fact]
    public void RateApp_NoParams_CommandCalled()
    {
        IStoreOperations storeFeaturesSetup = Substitute.For<IStoreOperations>();
        new AboutViewModel(
            appInformation: Substitute.For<IAppInformation>(),
            emailAdapter: Substitute.For<IEmailAdapter>(),
            browserAdapter: Substitute.For<IBrowserAdapter>(),
            storeOperations: storeFeaturesSetup).RateAppCommand.Execute(null);

        storeFeaturesSetup.Received(1).RateApp();
    }
}

