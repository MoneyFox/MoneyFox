using FluentAssertions;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Ui.Shared.ViewModels.About;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Ui.Shared.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AboutViewModelTests
    {
        [Fact]
        public void SupportMail_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(Substitute.For<IAppInformation>(),
                               Substitute.For<IEmailAdapter>(),
                               Substitute.For<IBrowserAdapter>(),
                               Substitute.For<IStoreOperations>())
               .SupportMail
               .Should().Be(AppConstants.SupportMail);
        }

        [Fact]
        public void Website_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(Substitute.For<IAppInformation>(),
                               Substitute.For<IEmailAdapter>(),
                               Substitute.For<IBrowserAdapter>(),
                               Substitute.For<IStoreOperations>())
               .Website
               .Should().Be(AppConstants.WebsiteUrl);
        }

        [Fact]
        public void Version_NoParams_ReturnCorrectMail()
        {
            var appinfos = Substitute.For<IAppInformation>();
            appinfos.GetVersion.Returns("42");

            new AboutViewModel(appinfos,
                               Substitute.For<IEmailAdapter>(),
                               Substitute.For<IBrowserAdapter>(),
                               Substitute.For<IStoreOperations>())
               .Version
               .Should().Be("42");
        }

        [Fact]
        public async Task GoToWebsite_NoParams_Called()
        {
            var webbrowserTaskSetup = Substitute.For<IBrowserAdapter>();
            webbrowserTaskSetup.OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri(AppConstants.WebsiteUrl)))
                               .Returns(Task.CompletedTask);

            new AboutViewModel(Substitute.For<IAppInformation>(),
                               Substitute.For<IEmailAdapter>(),
                               webbrowserTaskSetup,
                               Substitute.For<IStoreOperations>())
                 .GoToWebsiteCommand
                 .Execute(null);

            await webbrowserTaskSetup.Received(1).OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri(AppConstants.WebsiteUrl)));
        }

        [Fact]
        public async Task GoToRepository_NoParams_CommandCalled()
        {
            var webbrowserTaskSetup = Substitute.For<IBrowserAdapter>();
            webbrowserTaskSetup.OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri(AppConstants.GitHubRepositoryUrl)))
                               .Returns(Task.CompletedTask);

            new AboutViewModel(Substitute.For<IAppInformation>(),
                               Substitute.For<IEmailAdapter>(),
                               webbrowserTaskSetup,
                               Substitute.For<IStoreOperations>())
                 .GoToRepositoryCommand
                 .Execute(null);

            await webbrowserTaskSetup.Received(1).OpenWebsiteAsync(Arg.Is<Uri>(s => s == new Uri(AppConstants.GitHubRepositoryUrl)));
        }

        [Fact]
        public void RateApp_NoParams_CommandCalled()
        {
            var storeFeaturesSetup = Substitute.For<IStoreOperations>();

            new AboutViewModel(Substitute.For<IAppInformation>(),
                               Substitute.For<IEmailAdapter>(),
                               Substitute.For<IBrowserAdapter>(),
                               storeFeaturesSetup)
               .RateAppCommand
               .Execute(null);

            storeFeaturesSetup.Received(1).RateApp();
        }
    }
}
