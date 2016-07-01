using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class CategoryDialogViewModelTests : MvxIoCSupportingTest
    {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();

            // We setup the static setting classes here for the general usage in the app
            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);

            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);
            Mvx.RegisterType(() => settingsMockSetup.Object);
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            Mvx.RegisterSingleton(() => new Mock<IMvxMessenger>().Object);
            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>());
            dataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Category>())).Returns(true);

            Mock<IRepository<Category>> categoryRepoMock = new Mock<IRepository<Category>>(dataAccessSetup.Object,
                new Mock<INotificationService>().Object);
            categoryRepoMock.Setup(x => x.Save(It.IsAny<Category>())).Returns(true);
            CategoryDialogViewModel viewModel = new CategoryDialogViewModel(categoryRepoMock.Object, new Mock<IDialogService>().Object);

            viewModel.DoneCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }

    }
}
