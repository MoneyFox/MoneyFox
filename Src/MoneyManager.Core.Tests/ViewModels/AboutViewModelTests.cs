using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;
using MoneyManager.TestFoundation;
using Moq;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.WebBrowser;
using Xunit;

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
            composeMailSetup.Setup(x => x.ComposeEmail(It.Is<string>(s => s == Strings.SupportMail),
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
                .SupportMail.ShouldBe(Strings.SupportMail);
        }

        [Fact]
        public void GoToWebsite_NoParams_Called()
        {
            var commandCalled = false;

            var webbrowserTaskSetup = new Mock<IMvxWebBrowserTask>();
            webbrowserTaskSetup.Setup(x => x.ShowWebPage(It.Is<string>(s => s == Strings.WebsiteUrl))).Callback(() => commandCalled = true);

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
            webbrowserTaskSetup.Setup(x => x.ShowWebPage(It.Is<string>(s => s == Strings.GitHubRepositoryUrl))).Callback(() => commandCalled = true);

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