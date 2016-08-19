using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using PropertyChanged;

namespace MoneyFox.Shared.Repositories
{
    [ImplementPropertyChanged]
    public class AccountRepository : IAccountRepository {
        private readonly IDataAccess<Account> dataAccess;

        private List<Account> data;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced account data Access</param>
        public AccountRepository(IDataAccess<Account> dataAccess)
        {
            this.dataAccess = dataAccess;

            Load();
        }

        public IEnumerable<Account> GetList(Expression<Func<Account, bool>> filter = null) {
            if (data == null) {
                data = dataAccess.LoadList();
            }

            if (filter != null) {
                return data.Where(filter.Compile());
            }

            return data;
        }

        public Account FindById(int id) => data.FirstOrDefault(a => a.Id == id);

        /// <summary>
        ///     Save a new account or update an existing one.
        /// </summary>
        /// <param name="account">accountToDelete to save</param>
        public bool Save(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.Name))
            {
                account.Name = Strings.NoNamePlaceholderLabel;
            }

            if (account.Id == 0)
            {
                data.Add(account);
                data = new List<Account>(data.OrderBy(x => x.Name));
            }

            return dataAccess.SaveItem(account);
        }

        /// <summary>
        ///     Deletes the passed account and removes it from cache
        /// </summary>
        /// <param name="accountToDelete">accountToDelete to delete</param>
        public bool Delete(Account accountToDelete)
        {
            data.Remove(accountToDelete);
            return dataAccess.DeleteItem(accountToDelete);
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Account, bool>> filter = null)
        {
            data.Clear();

            foreach (var account in dataAccess.LoadList(filter))
            {
                data.Add(account);
            }
        }
    }
}