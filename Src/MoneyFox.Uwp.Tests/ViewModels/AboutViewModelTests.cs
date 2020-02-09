using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Uwp.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AboutViewModelTests
    {
        [Fact]
        public void SupportMail_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object)
               .SupportMail.ShouldEqual(AppConstants.SupportMail);
        }

        [Fact]
        public void Website_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object)
               .Website.ShouldEqual(AppConstants.WebsiteUrl);
        }

        [Fact]
        public void Version_NoParams_ReturnCorrectMail()
        {
            var appinfos = new Mock<IAppInformation>();
            appinfos.Setup(x => x.GetVersion()).Returns("42");

            new AboutViewModel(appinfos.Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object)
               .Version.ShouldEqual("42");
        }

        [Fact]
        public async Task GoToWebsite_NoParams_Called()
        {
            var webbrowserTaskSetup = new Mock<IBrowserAdapter>();
            webbrowserTaskSetup.Setup(x => x.OpenWebsiteAsync(It.Is<Uri>(s => s == new Uri(AppConstants.WebsiteUrl))))
                               .Returns(Task.CompletedTask);

            await new AboutViewModel(new Mock<IAppInformation>().Object,
                                     new Mock<IEmailAdapter>().Object,
                                     webbrowserTaskSetup.Object,
                                     new Mock<IStoreOperations>().Object)
                 .GoToWebsiteCommand.ExecuteAsync();

            webbrowserTaskSetup.Verify(x => x.OpenWebsiteAsync(It.IsAny<Uri>()), Times.Once);
        }

        [Fact]
        public async Task GoToRepository_NoParams_CommandCalled()
        {
            var webbrowserTaskSetup = new Mock<IBrowserAdapter>();
            webbrowserTaskSetup.Setup(
                                      x => x.OpenWebsiteAsync(It.Is<Uri>(s => s == new Uri(AppConstants.GitHubRepositoryUrl))))
                               .Returns(Task.CompletedTask);

            await new AboutViewModel(new Mock<IAppInformation>().Object,
                                     new Mock<IEmailAdapter>().Object,
                                     webbrowserTaskSetup.Object,
                                     new Mock<IStoreOperations>().Object)
                 .GoToRepositoryCommand.ExecuteAsync();

            webbrowserTaskSetup.Verify(x => x.OpenWebsiteAsync(It.IsAny<Uri>()), Times.Once());
        }

        [Fact]
        public void RateApp_NoParams_CommandCalled()
        {
            var storeFeaturesSetup = new Mock<IStoreOperations>();
            storeFeaturesSetup.Setup(x => x.RateApp());

            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               storeFeaturesSetup.Object)
               .RateAppCommand.Execute(null);

            storeFeaturesSetup.Verify(x => x.RateApp(), Times.Once());
        }
    }
}
