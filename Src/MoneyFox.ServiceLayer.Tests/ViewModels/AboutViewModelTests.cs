using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class AboutViewModelTests : MvxIoCSupportingTest
    {
        [Fact]
        public async Task SendMail_NoParams_CommandCalled()
        {
            var composeMailSetup = new Mock<IEmailAdapter>();
            composeMailSetup.Setup(x => x.SendEmail(It.Is<string>(s => s == Strings.FeedbackSubject),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>()))
                .Returns(Task.CompletedTask);

            await new AboutViewModel(new Mock<IAppInformation>().Object,
                               composeMailSetup.Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .SendMailCommand.ExecuteAsync();

            composeMailSetup.Verify(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public async Task SupportMail_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .SupportMail.ShouldEqual(AppConstants.SUPPORT_MAIL);
        }

        [Fact]
        public void Website_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .Website.ShouldEqual(AppConstants.WEBSITE_URL);
        }

        [Fact]
        public void Version_NoParams_ReturnCorrectMail()
        {
            var appinfos = new Mock<IAppInformation>();
            appinfos.Setup(x => x.GetVersion()).Returns("42");

            new AboutViewModel(appinfos.Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .Version.ShouldEqual("42");
        }

        [Fact]
        public async Task GoToWebsite_NoParams_Called()
        {
            var webbrowserTaskSetup = new Mock<IBrowserAdapter>();
            webbrowserTaskSetup.Setup(x => x.OpenWebsite(It.Is<Uri>(s => s == new Uri(AppConstants.WEBSITE_URL))))
                .Returns(Task.CompletedTask);

            await new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               webbrowserTaskSetup.Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .GoToWebsiteCommand.ExecuteAsync();

            webbrowserTaskSetup.Verify(x => x.OpenWebsite(It.IsAny<Uri>()), Times.Once);
        }

        [Fact]
        public async Task GoToRepository_NoParams_CommandCalled()
        {
            var webbrowserTaskSetup = new Mock<IBrowserAdapter>();
            webbrowserTaskSetup.Setup(
                    x => x.OpenWebsite(It.Is<Uri>(s => s == new Uri(AppConstants.GIT_HUB_REPOSITORY_URL))))
                .Returns(Task.CompletedTask);

            await new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               webbrowserTaskSetup.Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .GoToRepositoryCommand.ExecuteAsync();

            webbrowserTaskSetup.Verify(x => x.OpenWebsite(It.IsAny<Uri>()), Times.Once());
        }

        [Fact]
        public void RateApp_NoParams_CommandCalled()
        {
            var storeFeaturesSetup = new Mock<IStoreOperations>();
            storeFeaturesSetup.Setup(x => x.RateApp());

            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               storeFeaturesSetup.Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .RateAppCommand.Execute();

            storeFeaturesSetup.Verify(x => x.RateApp(), Times.Once());
        }
    }
}