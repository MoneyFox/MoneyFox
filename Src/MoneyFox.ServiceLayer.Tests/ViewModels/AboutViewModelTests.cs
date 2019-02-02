using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public void SendMail_NoParams_CommandCalled()
        {
            var commandCalled = false;

            var composeMailSetup = new Mock<IEmailAdapter>();
            composeMailSetup.Setup(x => x.SendEmail(It.Is<string>(s => s == Strings.FeedbackSubject),
                It.IsAny<string>(),
                It.IsAny<List<string>>()))
                .Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                               composeMailSetup.Object,
                               new Mock<IBrowserAdapter>().Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .SendMailCommand.Execute();

            commandCalled.ShouldBeTrue();
        }

        [Fact]
        public void SupportMail_NoParams_ReturnCorrectMail()
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
        public void GoToWebsite_NoParams_Called()
        {
            var commandCalled = false;

            var webbrowserTaskSetup = new Mock<IBrowserAdapter>();
            webbrowserTaskSetup.Setup(x => x.OpenWebsite(It.Is<Uri>(s => s == new Uri(AppConstants.WEBSITE_URL))))
                .Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               webbrowserTaskSetup.Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .GoToWebsiteCommand.Execute();

            commandCalled.ShouldBeTrue();
        }

        [Fact]
        public void GoToRepository_NoParams_CommandCalled()
        {
            var commandCalled = false;

            var webbrowserTaskSetup = new Mock<IBrowserAdapter>();
            webbrowserTaskSetup.Setup(
                x => x.OpenWebsite(It.Is<Uri>(s => s == new Uri(AppConstants.GIT_HUB_REPOSITORY_URL))))
                .Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               webbrowserTaskSetup.Object,
                               new Mock<IStoreOperations>().Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .GoToRepositoryCommand.Execute();

            commandCalled.ShouldBeTrue();
        }

        [Fact]
        public void RateApp_NoParams_CommandCalled()
        {
            var commandCalled = false;

            var storeFeaturesSetup = new Mock<IStoreOperations>();
            storeFeaturesSetup.Setup(x => x.RateApp()).Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                               new Mock<IEmailAdapter>().Object,
                               new Mock<IBrowserAdapter>().Object,
                               storeFeaturesSetup.Object,
                               new Mock<IMvxLogProvider>().Object,
                               new Mock<IMvxNavigationService>().Object)
                .RateAppCommand.Execute();

            commandCalled.ShouldBeTrue();
        }
    }
}