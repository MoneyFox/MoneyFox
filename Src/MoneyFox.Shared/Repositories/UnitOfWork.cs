using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using SQLite.Net;
using System;

namespace MoneyFox.Shared.Repositories {

    public class UnitOfWork : IDisposable {
        private readonly SQLiteConnection sqliteConnection;

        public UnitOfWork(IDatabaseManager dbManager) {
            sqliteConnection = dbManager.GetConnection();
        }

        private IAccountRepository accountRepository;
        private ICategoryRepository categoryRepository;

        public IAccountRepository AccountRepository
            => accountRepository ?? (accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnection)));

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