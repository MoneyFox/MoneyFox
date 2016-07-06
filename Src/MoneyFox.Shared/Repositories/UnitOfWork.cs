using System;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using SQLite.Net;

namespace MoneyFox.Shared.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }

        IPaymentRepository PaymentRepository { get; }
        IRepository<RecurringPayment> RecurringPaymentRepository { get; }

        ICategoryRepository CategoryRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly SQLiteConnection sqliteConnection;

        private IAccountRepository accountRepository;
        private ICategoryRepository categoryRepository;

        private bool disposed;
        private IPaymentRepository paymentRepository;
        private IRepository<RecurringPayment> recurringPaymentRepository;

        public UnitOfWork(IDatabaseManager dbManager)
        {
            sqliteConnection = dbManager.GetConnection();
        }

        public IAccountRepository AccountRepository
            => accountRepository ?? (accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnection)))
            ;

        public IPaymentRepository PaymentRepository
            => paymentRepository ?? (paymentRepository = new PaymentRepository(new PaymentDataAccess(sqliteConnection)))
            ;


        public IRepository<RecurringPayment> RecurringPaymentRepository
            => recurringPaymentRepository
               ??
               (recurringPaymentRepository =
                   new RecurringPaymentRepository(new RecurringPaymentDataAccess(sqliteConnection)));

        public ICategoryRepository CategoryRepository
            =>
                categoryRepository ??
                (categoryRepository = new CategoryRepository(new CategoryDataAccess(sqliteConnection)));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    sqliteConnection.Dispose();
                }
            }
            disposed = true;
        }
    }
}