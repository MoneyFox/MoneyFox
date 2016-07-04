using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using SQLite.Net;
using System;

namespace MoneyFox.Shared.Repositories {

    public class UnitOfWork : IDisposable {
        private readonly SQLiteConnection sqliteConnection;

        public UnitOfWork(IDatabaseManager dbManager) {
            sqliteConnection = dbManager.GetConnection();
        }

        private IRepository<Account> accountRepository;
        private IRepository<Category> categoryRepository;

        public IRepository<Account> AccountRepository
            => accountRepository ?? (accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnection)));

        public IRepository<Category> Categoryepository
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