using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using PropertyChanged;

namespace MoneyFox.Shared.Repositories
{
    [ImplementPropertyChanged]
    public class AccountRepository : IRepository<Account>
    {
        private readonly IDataAccess<Account> dataAccess;

        private ObservableCollection<Account> data;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced account data Access</param>
        public AccountRepository(IDataAccess<Account> dataAccess)
        {
            this.dataAccess = dataAccess;

            Data = new ObservableCollection<Account>();
            Load();
        }

        public Account FindById(int id) => data.FirstOrDefault(a => a.Id == id);

        /// <summary>
        ///     Cached account data
        /// </summary>
        public ObservableCollection<Account> Data
        {
            get { return data; }
            set
            {
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

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
                Data.Add(account);
            }

            return dataAccess.SaveItem(account);
        }

        /// <summary>
        ///     Deletes the passed account and removes it from cache
        /// </summary>
        /// <param name="accountToDelete">accountToDelete to delete</param>
        public bool Delete(Account accountToDelete)
        {
            Data.Remove(accountToDelete);
            return dataAccess.DeleteItem(accountToDelete);
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Account, bool>> filter = null)
        {
            Data.Clear();

            foreach (var account in dataAccess.LoadList(filter))
            {
                Data.Add(account);
            }
        }
    }
}