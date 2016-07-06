using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using SQLite.Net;
using System;

namespace MoneyFox.Shared.Repositories {

    public interface IUnitOfWork: IDisposable {
        IAccountRepository AccountRepository { get; }

        IPaymentRepository PaymentRepository { get; }
        IRepository<RecurringPayment> RecurringPaymentRepository { get; }

        ICategoryRepository CategoryRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork {
        private readonly SQLiteConnection sqliteConnection;

        public UnitOfWork(IDatabaseManager dbManager) {
            sqliteConnection = dbManager.GetConnection();
        }

        private IAccountRepository accountRepository;
        private ICategoryRepository categoryRepository;
        private IPaymentRepository paymentRepository;
        private IRepository<RecurringPayment> recurringPaymentRepository;

        public IAccountRepository AccountRepository
            => accountRepository ?? (accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnection)));
        public IPaymentRepository PaymentRepository
         => paymentRepository ?? (paymentRepository = new PaymentRepository(new PaymentDataAccess(sqliteConnection)));


        public IRepository<RecurringPayment> RecurringPaymentRepository
        => recurringPaymentRepository 
            ?? (recurringPaymentRepository = new RecurringPaymentRepository(new RecurringPaymentDataAccess(sqliteConnection)));

        public ICategoryRepository CategoryRepository
         => categoryRepository ?? (categoryRepository = new CategoryRepository(new CategoryDataAccess(sqliteConnection)));

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    sqliteConnection.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}