using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseManager dbManager;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dbManager">Instanced <see cref="IDatabaseManager"/> data Access</param>
        public AccountRepository(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public IEnumerable<AccountViewModel> GetList(Expression<Func<AccountViewModel, bool>> filter = null)
        {
            using (var db = dbManager.GetConnection())
            {
                var query = db.Table<Account>().AsQueryable().ProjectTo<AccountViewModel>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return query.OrderBy(x => x.Name).ToList();
            }
        }

        public AccountViewModel FindById(int id)
        {
            using (var db = dbManager.GetConnection())
            {
                return Mapper.Map<AccountViewModel>(db.Table<Account>().FirstOrDefault(x => x.Id == id));
            }
        }

        /// <summary>
        ///     Save a new AccountViewModel or update an existing one.
        /// </summary>
        public bool Save(AccountViewModel accountVmToSave)
        {
            using (var db = dbManager.GetConnection())
            {
                if (string.IsNullOrWhiteSpace(accountVmToSave.Name))
                {
                    accountVmToSave.Name = Strings.NoNamePlaceholderLabel;
                }

                var account = Mapper.Map<Account>(accountVmToSave);

                if (account.Id == 0)
                {
                    var rows = db.Insert(account);
                    accountVmToSave.Id = db.Table<Account>().OrderByDescending(x => x.Id).First().Id;
                    return rows == 1;
                }
                return db.Update(account) == 1;
            }
        }

        /// <summary>
        ///     Deletes the passed AccountViewModel and removes it from cache
        /// </summary>
        public bool Delete(AccountViewModel accountToDelete)
        {
            using (var db = dbManager.GetConnection())
            {
                var itemToDelete = db.Table<Account>().Single(x => x.Id == accountToDelete.Id);
                return db.Delete(itemToDelete) == 1;
            }
        }
    }
}