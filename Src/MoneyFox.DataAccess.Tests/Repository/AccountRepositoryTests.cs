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
    public class AccountRepositoryTests
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
            var accountRepo =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testAccount = new Fixture().Create<AccountViewModel>();
            testAccount.Id = 0;

            try
            {
                accountRepo.Save(testAccount);
                testAccount.Id.ShouldBeGreaterThan(0);
            } finally
            {
                accountRepo.Delete(testAccount);
            }
        }

        [TestMethod]
        public void Save_ExistingEntryUpdated()
        {
            var accountRepo =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testAccount = new Fixture().Create<AccountViewModel>();
            testAccount.Id = 0;

            try
            {
                accountRepo.Save(testAccount);
                accountRepo.FindById(testAccount.Id).ShouldNotBeNull();

                const string updatedName = "FOOOOOOOOOO";
                testAccount.Name = updatedName;

                accountRepo.Save(testAccount);
                accountRepo.FindById(testAccount.Id).Name.ShouldBe(updatedName);
            }
            finally
            {
                accountRepo.Delete(testAccount);
            }
        }

        [TestMethod]
        public void GetList_WithoutFilter()
        {
            var accountRepo =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testAccount = new Fixture().Create<AccountViewModel>();
            testAccount.Id = 0;

            try
            {
                accountRepo.Save(testAccount);
                
                var selectedAccount = accountRepo.GetList().First();

                selectedAccount.Id.ShouldBe(testAccount.Id);
                selectedAccount.Name.ShouldBe(testAccount.Name);
            }
            finally
            {
                accountRepo.Delete(testAccount);
            }
        }

        [TestMethod]
        public void GetList_WithFilter()
        {
            var accountRepo =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testAccount = new Fixture().Create<AccountViewModel>();
            testAccount.Id = 0;

            try
            {
                accountRepo.Save(testAccount);

                accountRepo.GetList(x => x.Id == testAccount.Id).First().Id.ShouldBe(testAccount.Id);
                accountRepo.GetList(x => x.Id == 99).FirstOrDefault().ShouldBeNull();
            } finally
            {
                accountRepo.Delete(testAccount);
            }
        }

        [TestMethod]
        public void Delete_AccountDeleted()
        {
            var accountRepo =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testAccount = new Fixture().Create<AccountViewModel>();
            testAccount.Id = 0;

            accountRepo.Save(testAccount);
            accountRepo.FindById(testAccount.Id).ShouldNotBeNull();

            accountRepo.Delete(testAccount);
            accountRepo.FindById(testAccount.Id).ShouldBeNull();
        }
        
        [TestMethod]
        public void FindById_AccountDeleted()
        {
            var accountRepo =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testAccount = new Fixture().Create<AccountViewModel>();
            testAccount.Id = 0;

            accountRepo.Save(testAccount);
            var selected = accountRepo.FindById(testAccount.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<AccountViewModel>();

            accountRepo.Delete(testAccount);
            accountRepo.FindById(testAccount.Id).ShouldBeNull();
        }
    }
}
