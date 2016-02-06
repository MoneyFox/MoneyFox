using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.WebBrowser;
using MvvmCross.Test.Core;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class AboutViewModelTests : MvxIoCSupportingTest
    {
        public AboutViewModelTests()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [Fact]
        public void SendMail_NoParams_CommandCalled()
        {
            var commandCalled = false;

            var composeMailSetup = new Mock<IMvxComposeEmailTask>();
            composeMailSetup.Setup(x => x.ComposeEmail(It.Is<string>(s => s == Constants.SUPPORT_MAIL),
                It.IsAny<string>(),
                It.Is<string>(s => s == Strings.FeedbackSubject),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                null))
                .Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                composeMailSetup.Object,
                new Mock<IMvxWebBrowserTask>().Object,
                new Mock<IStoreFeatures>().Object)
                .SendMailCommand.Execute();

            commandCalled.ShouldBeTrue();
        }

        [Fact]
        public void SupportMail_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(new Mock<IAppInformation>().Object,
                new Mock<IMvxComposeEmailTask>().Object,
                new Mock<IMvxWebBrowserTask>().Object,
                new Mock<IStoreFeatures>().Object)
                .SupportMail.ShouldBe(Constants.SUPPORT_MAIL);
        }

        [Fact]
        public void Website_NoParams_ReturnCorrectMail()
        {
            new AboutViewModel(new Mock<IAppInformation>().Object,
                new Mock<IMvxComposeEmailTask>().Object,
                new Mock<IMvxWebBrowserTask>().Object,
                new Mock<IStoreFeatures>().Object)
                .Website.ShouldBe(Constants.WEBSITE_URL);
        }

        [Fact]
        public void Version_NoParams_ReturnCorrectMail()
        {
            var appinfos = new Mock<IAppInformation>();
            appinfos.Setup(x => x.Version).Returns("42");

            new AboutViewModel(appinfos.Object,
                new Mock<IMvxComposeEmailTask>().Object,
                new Mock<IMvxWebBrowserTask>().Object,
                new Mock<IStoreFeatures>().Object)
                .Version.ShouldBe("42");
        }

        [Fact]
        public void GoToWebsite_NoParams_Called()
        {
            var commandCalled = false;

            var webbrowserTaskSetup = new Mock<IMvxWebBrowserTask>();
            webbrowserTaskSetup.Setup(x => x.ShowWebPage(It.Is<string>(s => s == Constants.WEBSITE_URL)))
                .Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                new Mock<IMvxComposeEmailTask>().Object,
                webbrowserTaskSetup.Object,
                new Mock<IStoreFeatures>().Object)
                .GoToWebsiteCommand.Execute();

            commandCalled.ShouldBeTrue();
        }

        [Fact]
        public void GoToRepository_NoParams_CommandCalled()
        {
            var commandCalled = false;

            var webbrowserTaskSetup = new Mock<IMvxWebBrowserTask>();
            webbrowserTaskSetup.Setup(x => x.ShowWebPage(It.Is<string>(s => s == Constants.GIT_HUB_REPOSITORY_URL)))
                .Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                new Mock<IMvxComposeEmailTask>().Object,
                webbrowserTaskSetup.Object,
                new Mock<IStoreFeatures>().Object)
                .GoToRepositoryCommand.Execute();

            commandCalled.ShouldBeTrue();
        }

        [Fact]
        public void RateApp_NoParams_CommandCalled()
        {
            var commandCalled = false;

            var storeFeaturesSetup = new Mock<IStoreFeatures>();
            storeFeaturesSetup.Setup(x => x.RateApp()).Callback(() => commandCalled = true);

            new AboutViewModel(new Mock<IAppInformation>().Object,
                new Mock<IMvxComposeEmailTask>().Object,
                new Mock<IMvxWebBrowserTask>().Object,
                storeFeaturesSetup.Object)
                .RateAppCommand.Execute();

            commandCalled.ShouldBeTrue();
        }
    }
}