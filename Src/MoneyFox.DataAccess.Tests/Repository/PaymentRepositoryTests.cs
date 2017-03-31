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
    public class PaymentRepositoryTests
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
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));
            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            try
            {
                paymentRepository.Save(testPayment);
                PaymentRepository.IsCacheMarkedForReload = true;
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
                PaymentRepository.IsCacheMarkedForReload = true;
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
                PaymentRepository.IsCacheMarkedForReload = true;

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
                PaymentRepository.IsCacheMarkedForReload = true;

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
            PaymentRepository.IsCacheMarkedForReload = true;
            paymentRepository.FindById(testPayment.Id).ShouldNotBeNull();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }

        [TestMethod]
        public void FindById_PaymentDeleted()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
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

            var category = new Fixture().Create<CategoryViewModel>();
            category.Id = 0;
            categoryRepository.Save(category);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.Category = category;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var payment = paymentRepository.FindById(testPayment.Id);

            payment.ShouldNotBeNull();
            payment.Category.ShouldNotBeNull();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();

            categoryRepository.FindById(testPayment.Category.Id).ShouldNotBeNull();
        }

        [TestMethod]
        public void Save_WithCategory()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testCategory = new Fixture().Create<CategoryViewModel>();
            testCategory.Id = 0;

            categoryRepository.Save(testCategory);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.Category = testCategory;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
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
        public void SaveAndUpdate_WithCategory_NoDuplicates()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var category = new Fixture().Create<CategoryViewModel>();
            category.Id = 0;

            categoryRepository.Save(category);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            testPayment.Category = category;

            paymentRepository.Save(testPayment);
            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            categoryRepository.GetList(x => x.Name == testPayment.Category.Name).Count().ShouldBe(1);
        }

        [TestMethod]
        public void Save_WithCategory_FkSet()
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
            PaymentRepository.IsCacheMarkedForReload = true;
            paymentRepository.FindById(testPayment.Id).CategoryId.Value.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Save_WithChargedAccount()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var account = new Fixture().Create<AccountViewModel>();
            account.Id = 0;
            accountRepository.Save(account);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.ChargedAccount = account;
            testPayment.TargetAccount = null;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.ChargedAccount.ShouldNotBeNull();
            selected.ChargedAccount.Id.ShouldBe(selected.ChargedAccountId);
            selected.ChargedAccount.Id.ShouldBe(testPayment.ChargedAccount.Id);

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }
        
        [TestMethod]
        public void SaveAndUpdate_WithChargedAccount_NoDuplicates()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var account = new Fixture().Create<AccountViewModel>();
            account.Id = 0;

            accountRepository.Save(account);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            testPayment.ChargedAccount = account;

            paymentRepository.Save(testPayment);
            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            accountRepository.GetList(x => x.Name == testPayment.ChargedAccount.Name).Count().ShouldBe(1);
        }

        [TestMethod]
        public void Save_WithChargedAccount_FkSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var account = fixture.Create<AccountViewModel>();
            accountRepository.Save(account);
            PaymentRepository.IsCacheMarkedForReload = true;
            account.Id.ShouldBeGreaterThan(0);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.ChargedAccount = account;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            paymentRepository.FindById(testPayment.Id).ChargedAccountId.ShouldBe(account.Id);
        }

        [TestMethod]
        public void Save_WithTargetAccount()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var account = new Fixture().Create<AccountViewModel>();
            account.Id = 0;
            accountRepository.Save(account);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.ChargedAccount = account;
            testPayment.TargetAccount = null;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.ChargedAccount.ShouldNotBeNull();
            selected.ChargedAccount.Id.ShouldBe(selected.ChargedAccountId);
            selected.ChargedAccount.Id.ShouldBe(testPayment.ChargedAccount.Id);

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }

        [TestMethod]
        public void SaveAndUpdate_WithTargetAccount_NoDuplicates()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var account = new Fixture().Create<AccountViewModel>();
            account.Id = 0;

            accountRepository.Save(account);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            testPayment.TargetAccount = account;

            paymentRepository.Save(testPayment);
            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            accountRepository.GetList(x => x.Name == testPayment.TargetAccount.Name).Count().ShouldBe(1);
        }

        [TestMethod]
        public void Save_WithTargetAccount_FkSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var account = fixture.Create<AccountViewModel>();

            accountRepository.Save(account);
            PaymentRepository.IsCacheMarkedForReload = true;
            account.Id.ShouldBeGreaterThan(0);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.TargetAccount = account;

            paymentRepository.Save(testPayment);
            paymentRepository.FindById(testPayment.Id).TargetAccountId.ShouldBe(account.Id);
        }

        [TestMethod]
        public void FindById_WithRecurringPayment()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.RecurringPaymentId = 0;
            testPayment.RecurringPayment.Id = 0;
            testPayment.IsRecurring = true;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.RecurringPayment.ShouldNotBeNull();
            selected.RecurringPayment.Id.ShouldBe(selected.RecurringPaymentId);
            selected.RecurringPayment.Id.ShouldBe(testPayment.RecurringPayment.Id);

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }

        [TestMethod]
        public void FindById_WithRecurringPayment_RecPaymentDependenciesLoaded()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var account = fixture.Create<AccountViewModel>();
            account.Id = 0;
            accountRepository.Save(account);

            var testPayment = fixture.Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.RecurringPaymentId = 0;
            testPayment.RecurringPayment.Id = 0;
            testPayment.IsRecurring = true;
            testPayment.ChargedAccount = account;
            testPayment.RecurringPayment.ChargedAccount = account;


            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.RecurringPayment.ShouldNotBeNull();
            selected.RecurringPayment.Id.ShouldBe(selected.RecurringPaymentId);
            selected.RecurringPayment.Id.ShouldBe(testPayment.RecurringPayment.Id);

            selected.RecurringPayment.ChargedAccount.ShouldNotBeNull();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }
        
        [TestMethod]
        public void GetList_WithRecurringPayment_RecPaymentDependenciesLoaded()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var accountRepository =
                new AccountRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var account = fixture.Create<AccountViewModel>();
            account.Id = 0;
            accountRepository.Save(account);

            var testPayment = fixture.Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.RecurringPaymentId = 0;
            testPayment.RecurringPayment.Id = 0;
            testPayment.IsRecurring = true;
            testPayment.ChargedAccount = account;
            testPayment.RecurringPayment.ChargedAccount = account;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.GetList(x => x.Id == testPayment.Id).First();

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.RecurringPayment.ShouldNotBeNull();
            selected.RecurringPayment.Id.ShouldBe(selected.RecurringPaymentId);
            selected.RecurringPayment.Id.ShouldBe(testPayment.RecurringPayment.Id);

            selected.RecurringPayment.ChargedAccount.ShouldNotBeNull();

            paymentRepository.Delete(testPayment);
            paymentRepository.FindById(testPayment.Id).ShouldBeNull();
        }

        [TestMethod]
        public void Save_WithRecurringPayment_IdSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.IsRecurring = true;
            testPayment.RecurringPayment.Id = 0;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.RecurringPayment.ShouldNotBeNull();
            selected.RecurringPayment.ShouldBeInstanceOf<RecurringPaymentViewModel>();

            selected.RecurringPayment.Id.ShouldBeGreaterThan(0);
            selected.RecurringPaymentId.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Save_WithRecurringPayment_IdInCacheSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.IsRecurring = true;
            testPayment.RecurringPayment.Id = 0;

            paymentRepository.Save(testPayment);
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();

            selected.RecurringPayment.ShouldNotBeNull();
            selected.RecurringPayment.ShouldBeInstanceOf<RecurringPaymentViewModel>();

            selected.RecurringPayment.Id.ShouldBeGreaterThan(0);
            selected.RecurringPaymentId.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void SaveAndUpdate_WithRecurringPayment_NoDuplicates()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                new MvxWpfFileStore(FILE_ROOT)));

            var recurringPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var recurringPayment = new Fixture().Create<RecurringPaymentViewModel>();
            recurringPayment.Id = 0;

            recurringPaymentRepository.Save(recurringPayment);
            PaymentRepository.IsCacheMarkedForReload = true;

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;

            testPayment.RecurringPayment = recurringPayment;

            paymentRepository.Save(testPayment);
            paymentRepository.Save(testPayment);
            var selected = paymentRepository.FindById(testPayment.Id);

            recurringPaymentRepository.GetList(x => x.Note == testPayment.RecurringPayment.Note).Count().ShouldBe(1);
        }

        [TestMethod]
        public void Save_WithRecurringPayment_FkSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var recurringPaymentRepository =
                new RecurringPaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var recurringPayment = fixture.Create<RecurringPaymentViewModel>();

            recurringPaymentRepository.Save(recurringPayment);
            recurringPayment.Id.ShouldBeGreaterThan(0);

            var testPayment = new Fixture().Create<PaymentViewModel>();
            testPayment.Id = 0;
            testPayment.RecurringPayment = recurringPayment;

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            paymentRepository.FindById(testPayment.Id).RecurringPaymentId.ShouldBe(recurringPayment.Id);
        }

        [TestMethod]
        public void Save_WithNoChildrenSet()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var testPayment = new PaymentViewModel
            {
                Amount = 40,
                ChargedAccount = new AccountViewModel()
            };

            paymentRepository.Save(testPayment);
            PaymentRepository.IsCacheMarkedForReload = true;
            var selected = paymentRepository.FindById(testPayment.Id);

            selected.ShouldNotBeNull();
            selected.ShouldBeInstanceOf<PaymentViewModel>();
        }

        [TestMethod]
        public void GetList_WithChildren()
        {
            var paymentRepository =
                new PaymentRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var categoryRepository =
                new CategoryRepository(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWpfFileStore(FILE_ROOT)));

            var fixture = new Fixture();

            var category = fixture.Create<CategoryViewModel>();
            category.Id = 0;
            categoryRepository.Save(category);

            var payment = fixture.Create<PaymentViewModel>();
            payment.Id = 0;
            payment.Category = category;

            paymentRepository.Save(payment);
            PaymentRepository.IsCacheMarkedForReload = true;

            var selected = paymentRepository.GetList(x => x.Id == payment.Id).First();
            selected.ShouldNotBeNull();
            selected.Category.ShouldNotBeNull();
        }
    }
}