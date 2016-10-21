using System;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.DataModels;
using MvvmCross.Plugins.File.Wpf;
using MvvmCross.Plugins.Sqlite.Wpf;
using Ploeh.AutoFixture;
using XunitShouldExtension;

namespace MoneyFox.DataAccess.Tests
{
    [TestClass]
    public class PaymentRepositoryTests
    {
        private readonly string dbDefaultPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

        private const string FILE_ROOT = @"C:\Temp";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();
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
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            try
            {
                paymentRepository.Save(testPayment);
                testPayment.Id.ShouldBeGreaterThan(0);
            }
            finally
            {
                paymentRepository.Delete(testPayment);
            }
        }

        [TestMethod]
        public void Save_ExistingEntryUpdated()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            try
            {
                paymentRepository.Save(testPayment);
                paymentRepository.FindById(testPayment.Id).ShouldNotBeNull();

                const string updatedNote = "FOOOOOOOOOO";
                testPayment.Note = updatedNote;

                paymentRepository.Save(testPayment);
                paymentRepository.FindById(testPayment.Id).Note.ShouldBe(updatedNote);
            }
            finally
            {
                paymentRepository.Delete(testPayment);
            }
        }

        [TestMethod]
        public void GetList_WithoutFilter()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            try
            {
                paymentRepository.Save(testPayment);

                var selectedAccount = paymentRepository.GetList().First();

                selectedAccount.Id.ShouldBe(testPayment.Id);
                selectedAccount.Amount.ShouldBe(testPayment.Amount);
            }
            finally
            {
                paymentRepository.Delete(testPayment);
            }
        }

        [TestMethod]
        public void GetList_WithFilter()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            try
            {
                paymentRepository.Save(testPayment);

                paymentRepository.GetList(x => x.Id == testPayment.Id).First().Id.ShouldBe(testPayment.Id);
                paymentRepository.GetList(x => x.Id == 99).FirstOrDefault().ShouldBeNull();
            }
            finally
            {
                paymentRepository.Delete(testPayment);
            }
        }

        [TestMethod]
        public void Delete_PaymentDeleted()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            paymentRepository.Save(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldNotBeNull();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }

        [TestMethod]
        public void FindById_AccountDeleted()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            paymentRepository.Save(testPayment);
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }


        [TestMethod]
        public void Save_WithChildren()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.Category.Id = 0;

            paymentRepository.Save(testPayment);
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.Category.ShouldNotBeNull();
            selected.Category.Id.ShouldBe(selected.CategoryId.Value);
            selected.Category.Id.ShouldBe(testPayment.Category.Id);

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }

        [TestMethod]
        public void Save_WithChildren_IdSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.Category.Id = 0;
            testPayment.TargetAccount.Id = 0;
            testPayment.ChargedAccount.Id = 0;

            paymentRepository.Save(testPayment);
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.Category.ShouldNotBeNull();
            selected.Category.ShouldBeInstanceOf<CategoryViewModel>();

            selected.Category.Id.ShouldBeGreaterThan(0);
        }
        
        [TestMethod]
        public void SaveAndUpdate_WithChildren_NoDuplicates()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var category = fixture.Create<CategoryViewModel>();

            categoryRepository.Save(category);
            category.Id.ShouldBeGreaterThan(0);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.Category = category;

            paymentRepository.Save(testPayment);
            paymentRepository.Save(testPayment);
            var selected = paymentRepository.FindById(testPayment.Id);

            categoryRepository.GetList(x => x.Name == category.Name).Count().ShouldBe(1);
        }

        [TestMethod]
        public void Save_WithChildren_FkSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var category = fixture.Create<CategoryViewModel>();

            categoryRepository.Save(category);
            category.Id.ShouldBeGreaterThan(0);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.Category = category;

            paymentRepository.Save(testPayment);
            var selected = paymentRepository.FindById(testPayment.Id);

            categoryRepository.GetList(x => x.Name == category.Name).Count().ShouldBe(1);
        }

        [TestMethod]
        public void Delete_WithChildren_PaymentDeleted()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.Category.Id = 0;

            paymentRepository.Save(testPayment);
            var payment = paymentRepository.FindById(testPayment.Id);

            payment.ShouldNotBeNull();
            payment.Category.ShouldNotBeNull();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();

            categoryRepository.FindById(testPayment.Category.Id).ShouldNotBeNull();
        }
    }
}