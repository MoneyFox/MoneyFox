using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Resources;
using Moq;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Repositories
{
    [TestClass]
    public class CategoryRepositoryTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            Setup();
        }

        [TestMethod]
        public void Save_EmptyString_CorrectNameAssigned()
        {
            var categoryList = new List<CategoryViewModel>();

            var categoryDataAccessMock = new Mock<IDataAccess<CategoryViewModel>>();
            categoryDataAccessMock.Setup(x => x.SaveItem(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => categoryList.Add(cat));
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<CategoryViewModel>());

            var repository = new CategoryRepository(categoryDataAccessMock.Object);

            var category = new CategoryViewModel
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
            var categoryList = new List<CategoryViewModel>();

            var categoryDataAccessMock = new Mock<IDataAccess<CategoryViewModel>>();
            categoryDataAccessMock.Setup(x => x.SaveItem(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => categoryList.Add(cat));
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<CategoryViewModel>());

            var repository = new CategoryRepository(categoryDataAccessMock.Object);

            var category = new CategoryViewModel
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
            var categoryList = new List<CategoryViewModel>();

            var categoryDataAccessMock = new Mock<IDataAccess<CategoryViewModel>>();
            categoryDataAccessMock.Setup(x => x.SaveItem(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => categoryList.Add(cat));
            categoryDataAccessMock.Setup(x => x.DeleteItem(It.IsAny<CategoryViewModel>()))
                .Callback((CategoryViewModel cat) => categoryList.Remove(cat));
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<CategoryViewModel>());

            var repository = new CategoryRepository(categoryDataAccessMock.Object);

            var category = new CategoryViewModel
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
        public void CategoryRepository_AddMultipleToCache()
        {
            var categoryDataAccessMock = new Mock<IDataAccess<CategoryViewModel>>();
            categoryDataAccessMock.Setup(x => x.LoadList(null)).Returns(new List<CategoryViewModel>());


            var repository = new CategoryRepository(categoryDataAccessMock.Object);
            var category = new CategoryViewModel
            {
                Name = "Ausgang"
            };

            var secondCategory = new CategoryViewModel
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
            var dataAccessSetup = new Mock<IDataAccess<CategoryViewModel>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel {Id = 10},
                new CategoryViewModel {Id = 15}
            });

            var categoryRepository = new CategoryRepository(dataAccessSetup.Object);
            categoryRepository.Load();

            categoryRepository.GetList(x => x.Id == 10).Any().ShouldBeTrue();
            categoryRepository.GetList(x => x.Id == 15).Any().ShouldBeTrue();
        }

        [TestMethod]
        public void Delete_Failure_ReturnFalse()
        {
            var dataAccessSetup = new Mock<IDataAccess<CategoryViewModel>>();
            dataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<CategoryViewModel>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<CategoryViewModel>());


            new CategoryRepository(dataAccessSetup.Object).Delete(new CategoryViewModel()).ShouldBeFalse();
        }

        [TestMethod]
        public void Save_Failure_ReturnFalse()
        {
            var dataAccessSetup = new Mock<IDataAccess<CategoryViewModel>>();
            dataAccessSetup.Setup(x => x.SaveItem(It.IsAny<CategoryViewModel>())).Returns(false);
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<CategoryViewModel>());


            new CategoryRepository(dataAccessSetup.Object).Save(new CategoryViewModel()).ShouldBeFalse();
        }

        [TestMethod]
        public void FindById_ReturnsCategory() {
            var categoryDataAccessMock = new Mock<IDataAccess<CategoryViewModel>>();
            var testCategory = new CategoryViewModel { Id = 100, Name = "Test CategoryViewModel" };

            categoryDataAccessMock.Setup(x => x.LoadList(null))
                .Returns(new List<CategoryViewModel> { testCategory });

            Assert.AreEqual(testCategory, new CategoryRepository(categoryDataAccessMock.Object).FindById(100));
        }
    }
}