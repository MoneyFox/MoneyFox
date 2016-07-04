using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.Tests.Mocks;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Repositories {
    [TestClass]
    public class CategoryRepositoryTests : MvxIoCSupportingTest {
        private DateTime localDateSetting;

        public static IEnumerable NamePlaceholder {
            get {
                yield return new object[] {"Ausgang", "Ausgang"};
                yield return new object[] {"", Strings.NoNamePlaceholderLabel};
            }
        }

        [TestInitialize]
        public void Init() {
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
        public void Save_EmptyString_CorrectNameAssigned() {
            var categoryDataAccessMock = new CategoryDataAccessMock();
            var repository = new CategoryRepository(categoryDataAccessMock,
                new Mock<INotificationService>().Object);

            var category = new Category {
                Name = ""
            };

            repository.Save(category);

            categoryDataAccessMock.CategoryTestList[0].ShouldBeSameAs(category);
            categoryDataAccessMock.CategoryTestList[0].Name.ShouldBe(Strings.NoNamePlaceholderLabel);
        }

        [TestMethod]
        public void Save_InputName_CorrectNameAssigned() {
            const string name = "Ausgang";
            var categoryDataAccessMock = new CategoryDataAccessMock();
            var repository = new CategoryRepository(categoryDataAccessMock,
                new Mock<INotificationService>().Object);

            var category = new Category {
                Name = name
            };

            repository.Save(category);

            categoryDataAccessMock.CategoryTestList[0].ShouldBeSameAs(category);
            categoryDataAccessMock.CategoryTestList[0].Name.ShouldBe(name);
        }

        [TestMethod]
        public void CategoryRepository_Delete() {
            var categoryDataAccessMock = new CategoryDataAccessMock();
            var repository = new CategoryRepository(categoryDataAccessMock,
                new Mock<INotificationService>().Object);

            var category = new Category {
                Name = "Ausgang"
            };

            repository.Save(category);

            categoryDataAccessMock.CategoryTestList[0].ShouldBeSameAs(category);

            repository.Delete(category);

            categoryDataAccessMock.CategoryTestList.Any().ShouldBeFalse();
            repository.Data.Any().ShouldBeFalse();
        }

        [TestMethod]
        public void CategoryRepository_AccessCache() {
            new CategoryRepository(new CategoryDataAccessMock(), new Mock<INotificationService>().Object)
                .Data.ShouldNotBeNull();
        }

        [TestMethod]
        public void CategoryRepository_AddMultipleToCache() {
            var repository = new CategoryRepository(new CategoryDataAccessMock(),
                new Mock<INotificationService>().Object);
            var category = new Category {
                Name = "Ausgang"
            };

            var secondCategory = new Category {
                Name = "Lebensmittel"
            };

            repository.Save(category);
            repository.Save(secondCategory);

            repository.Data.Count.ShouldBe(2);
            repository.Data[0].ShouldBeSameAs(category);
            repository.Data[1].ShouldBeSameAs(secondCategory);
        }

        [TestMethod]
        public void Load_CategoryDataAccess_DataInitialized() {
            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category> {
                new Category {Id = 10},
                new Category {Id = 15}
            });

            var categoryRepository = new CategoryRepository(dataAccessSetup.Object,
                new Mock<INotificationService>().Object);
            categoryRepository.Load();

            categoryRepository.Data.Any(x => x.Id == 10).ShouldBeTrue();
            categoryRepository.Data.Any(x => x.Id == 15).ShouldBeTrue();
        }

        [TestMethod]
        public void Save_NotifyUserOfFailure() {
            var isNotificationServiceCalled = false;

            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Category>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>());

            var notificationServiceSetup = new Mock<INotificationService>();
            notificationServiceSetup.Setup(x => x.SendBasicNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string x, string y) => isNotificationServiceCalled = true);

            new CategoryRepository(dataAccessSetup.Object,
                notificationServiceSetup.Object).Save(new Category());

            isNotificationServiceCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void Delete_NotifyUserOfFailure() {
            var isNotificationServiceCalled = false;

            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<Category>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>());

            var notificationServiceSetup = new Mock<INotificationService>();
            notificationServiceSetup.Setup(x => x.SendBasicNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string x, string y) => isNotificationServiceCalled = true);

            new CategoryRepository(dataAccessSetup.Object,
                notificationServiceSetup.Object).Delete(new Category());

            isNotificationServiceCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void CategoryRepository_FindById_ReturnsCategory()
        {
            var categoryRepository = new Mock<IRepository<Category>>();
            var testCategory = new Category() {Id = 100, Name = "Test Category"};
            categoryRepository.SetupAllProperties();
            categoryRepository.Setup(x => x.FindById(It.IsAny<int>()))
                .Returns((int categoryId) => categoryRepository.Object.Data.FirstOrDefault(c => c.Id == categoryId));
            categoryRepository.Object.Data = new ObservableCollection<Category>();
            categoryRepository.Object.Data.Add(testCategory);

            Assert.AreEqual(testCategory, categoryRepository.Object.FindById(100));
        }
    }
}