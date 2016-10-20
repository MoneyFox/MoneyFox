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
    }
}