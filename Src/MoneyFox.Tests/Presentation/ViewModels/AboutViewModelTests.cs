namespace MoneyFox.Tests.Presentation.ViewModels
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.ViewModels.About;
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
            var appinfos = Substitute.For<IAppInformation>();
            appinfos.GetVersion.Returns("42");
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
            var webBrowserTaskSetup = Substitute.For<IBrowserAdapter>();
            webBrowserTaskSetup.OpenWebsiteAsync(Arg.Any<Uri>()).Returns(Task.CompletedTask);
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
            var webbrowserTaskSetup = Substitute.For<IBrowserAdapter>();
            webbrowserTaskSetup.OpenWebsiteAsync(Arg.Any<Uri>()).Returns(Task.CompletedTask);
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
            var storeFeaturesSetup = Substitute.For<IStoreOperations>();
            new AboutViewModel(
                appInformation: Substitute.For<IAppInformation>(),
                emailAdapter: Substitute.For<IEmailAdapter>(),
                browserAdapter: Substitute.For<IBrowserAdapter>(),
                storeOperations: storeFeaturesSetup).RateAppCommand.Execute(null);

            storeFeaturesSetup.Received(1).RateApp();
        }
    }

}
