using System;
using System.Collections.Generic;
using System.Linq;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Resources;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Repositories
{
    [TestClass]
    public class CategoryRepositoryTests : MvxIoCSupportingTest
    {
        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
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
        public void Save_EmptyString_CorrectNameAssigned()
        {
            var categoryList = new List<Category>();

            var categoryDataAccessMock = new Mock<IDataAccess<Category>>();
            categoryDataAccessMock.Setup(x => x.SaveItem(It.IsAny<Category>()))
                .Callback((Category cat) => categoryList.Add(cat));
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<Category>());

            var repository = new CategoryRepository(categoryDataAccessMock.Object);

            var category = new Category
            {
                Name = ""
            };

            repository.Save(category);

            categoryList[0].ShouldBeSameAs(category);
            categoryList[0].Name.ShouldBe(Strings.NoNamePlaceholderLabel);
        }

        [TestMethod]
        public void Save_InputName_CorrectNameAssigned()
        {
            const string name = "Ausgang";
            var categoryList = new List<Category>();

            var categoryDataAccessMock = new Mock<IDataAccess<Category>>();
            categoryDataAccessMock.Setup(x => x.SaveItem(It.IsAny<Category>()))
                .Callback((Category cat) => categoryList.Add(cat));
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<Category>());

            var repository = new CategoryRepository(categoryDataAccessMock.Object);

            var category = new Category
            {
                Name = name
            };

            repository.Save(category);

            categoryList[0].ShouldBe(category);
            categoryList[0].Name.ShouldBe(name);
        }

        [TestMethod]
        public void CategoryRepository_Delete()
        {
            var categoryList = new List<Category>();

            var categoryDataAccessMock = new Mock<IDataAccess<Category>>();
            categoryDataAccessMock.Setup(x => x.SaveItem(It.IsAny<Category>()))
                .Callback((Category cat) => categoryList.Add(cat));
            categoryDataAccessMock.Setup(x => x.DeleteItem(It.IsAny<Category>()))
                .Callback((Category cat) => categoryList.Remove(cat));
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<Category>());

            var repository = new CategoryRepository(categoryDataAccessMock.Object);

            var category = new Category
            {
                Name = "Ausgang"
            };

            repository.Save(category);

            categoryList[0].ShouldBeSameAs(category);

            repository.Delete(category);

            categoryList.Any().ShouldBeFalse();
            repository.GetList().Any().ShouldBeFalse();
        }

        [TestMethod]
        public void CategoryRepository_AccessCache()
        {
            new CategoryRepository(new Mock<IDataAccess<Category>>().Object).GetList().ShouldNotBeNull();
        }

        [TestMethod]
        public void CategoryRepository_AddMultipleToCache()
        {
            var categoryDataAccessMock = new Mock<IDataAccess<Category>>();
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<Category>());


            var repository = new CategoryRepository(categoryDataAccessMock.Object);
            var category = new Category
            {
                Name = "Ausgang"
            };

            var secondCategory = new Category
            {
                Name = "Lebensmittel"
            };

            repository.Save(category);
            repository.Save(secondCategory);

            repository.GetList().ToList().Count.ShouldBe(2);
            repository.GetList().ToList()[0].ShouldBeSameAs(category);
            repository.GetList().ToList()[1].ShouldBeSameAs(secondCategory);
        }

        [TestMethod]
        public void Load_CategoryDataAccess_DataInitialized()
        {
            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>
            {
                new Category {Id = 10},
                new Category {Id = 15}
            });

            var categoryRepository = new CategoryRepository(dataAccessSetup.Object);
            categoryRepository.Load();

            categoryRepository.GetList(x => x.Id == 10).Any().ShouldBeTrue();
            categoryRepository.GetList(x => x.Id == 15).Any().ShouldBeTrue();
        }

        [TestMethod]
        public void Delete_Failure_ReturnFalse()
        {
            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<Category>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>());


            new CategoryRepository(dataAccessSetup.Object).Delete(new Category()).ShouldBeFalse();
        }

        [TestMethod]
        public void Save_Failure_ReturnFalse()
        {
            var dataAccessSetup = new Mock<IDataAccess<Category>>();
            dataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Category>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Category>());


            new CategoryRepository(dataAccessSetup.Object).Save(new Category()).ShouldBeFalse();
        }

        [TestMethod]
        public void FindById_ReturnsCategory() {
            var categoryDataAccessMock = new Mock<IDataAccess<Category>>();
            var testCategory = new Category { Id = 100, Name = "Test Category" };

            categoryDataAccessMock.Setup(x => x.LoadList(null))
                .Returns(new List<Category> { testCategory });

            Assert.AreEqual(testCategory, new CategoryRepository(categoryDataAccessMock.Object).FindById(100));
        }
    }
}