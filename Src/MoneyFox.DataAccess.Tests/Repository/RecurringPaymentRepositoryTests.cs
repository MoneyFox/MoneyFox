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
    public class RecurringPaymentRepositoryTests
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
            var recurringPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testRecurringPayment = new Fixture().Create<RecurringPaymentViewModel>();
            testRecurringPayment.Id = 0;

            try
            {
                recurringPaymentRepository.Save(testRecurringPayment);
                testRecurringPayment.Id.ShouldBeGreaterThan(0);
            }
            finally
            {
                recurringPaymentRepository.Delete(testRecurringPayment);
            }
        }

        [TestMethod]
        public void Save_ExistingEntryUpdated()
        {
            var recPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testRecurringPayment = new Fixture().Create<RecurringPaymentViewModel>();
            testRecurringPayment.Id = 0;

            try
            {
                recPaymentRepository.Save(testRecurringPayment);
                recPaymentRepository.FindById(testRecurringPayment.Id).ShouldNotBeNull();

                const string updatedNote = "FOOOOOOOOOO";
                testRecurringPayment.Note = updatedNote;

                recPaymentRepository.Save(testRecurringPayment);
                recPaymentRepository.FindById(testRecurringPayment.Id).Note.ShouldBe(updatedNote);
            }
            finally
            {
                recPaymentRepository.Delete(testRecurringPayment);
            }
        }

        [TestMethod]
        public void GetList_WithoutFilter()
        {
            var recurringPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testRecurringPayment = new Fixture().Create<RecurringPaymentViewModel>();
            testRecurringPayment.Id = 0;

            try
            {
                recurringPaymentRepository.Save(testRecurringPayment);

                var selectedAccount = recurringPaymentRepository.GetList().First();

                selectedAccount.Id.ShouldBe(testRecurringPayment.Id);
                selectedAccount.Amount.ShouldBe(testRecurringPayment.Amount);
            }
            finally
            {
                recurringPaymentRepository.Delete(testRecurringPayment);
            }
        }

        [TestMethod]
        public void GetList_WithFilter()
        {
            var recurringPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testRecurringPayment = new Fixture().Create<RecurringPaymentViewModel>();
            testRecurringPayment.Id = 0;

            try
            {
                recurringPaymentRepository.Save(testRecurringPayment);

                recurringPaymentRepository.GetList(x => x.Id == testRecurringPayment.Id).First().Id.ShouldBe(testRecurringPayment.Id);
                recurringPaymentRepository.GetList(x => x.Id == 99).FirstOrDefault().ShouldBeNull();
            }
            finally
            {
                recurringPaymentRepository.Delete(testRecurringPayment);
            }
        }

        [TestMethod]
        public void Delete_PaymentDeleted()
        {
            var recurringPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testRecurringPayment = new Fixture().Create<RecurringPaymentViewModel>();
            testRecurringPayment.Id = 0;

            recurringPaymentRepository.Save(testRecurringPayment);
            recurringPaymentRepository.FindById(testRecurringPayment.Id).ShouldNotBeNull();

            recurringPaymentRepository.Delete(testRecurringPayment);
            recurringPaymentRepository.FindById(testRecurringPayment.Id).ShouldBeNull();
        }

        [TestMethod]
        public void FindById_RecurringPaymentDeleted()
        {
            var recurringPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testRecurringPayment = new Fixture().Create<RecurringPaymentViewModel>();
            testRecurringPayment.Id = 0;

            recurringPaymentRepository.Save(testRecurringPayment);
            var selected = recurringPaymentRepository.FindById(testRecurringPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<RecurringPaymentViewModel>();

            recurringPaymentRepository.Delete(testRecurringPayment);
            recurringPaymentRepository.FindById(testRecurringPayment.Id).ShouldBeNull();
        }
    }
}