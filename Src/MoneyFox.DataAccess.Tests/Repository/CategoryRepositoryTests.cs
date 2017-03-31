using System;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Tests;
using MvvmCross.Plugins.File.Wpf;
using MvvmCross.Plugins.Sqlite.Wpf;
using Ploeh.AutoFixture;

namespace MoneyFox.DataAccess.Tests.Repository
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        private readonly string dbDefaultPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

        private const string FILE_ROOT = @"C:\Temp";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            MapperConfiguration.Setup();
        }

        [TestInitialize]
        public void Init()
        {
            var dbPath = Path.Combine(dbDefaultPath, DatabaseConstants.DB_NAME);
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        [TestMethod]
        public void Save_IdSet()
        {
            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testCategory = new Fixture().Create<CategoryViewModel>();
            testCategory.Id = 0;

            try
            {
                categoryRepository.Save(testCategory);
                testCategory.Id.ShouldBeGreaterThan(0);
            }
            finally
            {
                categoryRepository.Delete(testCategory);
            }
        }

        [TestMethod]
        public void Save_ExistingEntryUpdated()
        {
            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testCategory = new Fixture().Create<CategoryViewModel>();
            testCategory.Id = 0;

            try
            {
                categoryRepository.Save(testCategory);
                categoryRepository.FindById(testCategory.Id).ShouldNotBeNull();

                const string updatedName = "FOOOOOOOOOO";
                testCategory.Name = updatedName;

                categoryRepository.Save(testCategory);
                categoryRepository.FindById(testCategory.Id).Name.ShouldBe(updatedName);
            }
            finally
            {
                categoryRepository.Delete(testCategory);
            }
        }

        [TestMethod]
        public void GetList_WithoutFilter()
        {
            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testCategory = new Fixture().Create<CategoryViewModel>();
            testCategory.Id = 0;

            try
            {
                categoryRepository.Save(testCategory);

                var selectedAccount = categoryRepository.GetList().First();

                selectedAccount.Id.ShouldBe(testCategory.Id);
                selectedAccount.Name.ShouldBe(testCategory.Name);
            }
            finally
            {
                categoryRepository.Delete(testCategory);
            }
        }

        [TestMethod]
        public void GetList_WithFilter()
        {
            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testCategory = new Fixture().Create<CategoryViewModel>();
            testCategory.Id = 0;

            try
            {
                categoryRepository.Save(testCategory);

                categoryRepository.GetList(x => x.Id == testCategory.Id).First().Id.ShouldBe(testCategory.Id);
                categoryRepository.GetList(x => x.Id == 99).FirstOrDefault().ShouldBeNull();
            }
            finally
            {
                categoryRepository.Delete(testCategory);
            }
        }

        [TestMethod]
        public void Delete_CategoryDeleted()
        {
            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testCategory = new Fixture().Create<CategoryViewModel>();
            testCategory.Id = 0;

            categoryRepository.Save(testCategory);
            categoryRepository.FindById(testCategory.Id).ShouldNotBeNull();

            categoryRepository.Delete(testCategory);
            categoryRepository.FindById(testCategory.Id).ShouldBeNull();
        }

        [TestMethod]
        public void FindById_CategoryDeleted()
        {
            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testCategory = new Fixture().Create<CategoryViewModel>();
            testCategory.Id = 0;

            categoryRepository.Save(testCategory);
            var selected = categoryRepository.FindById(testCategory.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<CategoryViewModel>();

            categoryRepository.Delete(testCategory);
            categoryRepository.FindById(testCategory.Id).ShouldBeNull();
        }
    }
}