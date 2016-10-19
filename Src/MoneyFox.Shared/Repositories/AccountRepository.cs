using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Resources;

namespace MoneyFox.Shared.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDataAccess<AccountViewModel> dataAccess;

        private List<AccountViewModel> data;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced AccountViewModel data Access</param>
        public AccountRepository(IDataAccess<AccountViewModel> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<AccountViewModel> GetList(Expression<Func<AccountViewModel, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            return filter != null ? data.Where(filter.Compile()) : data;
        }

        public AccountViewModel FindById(int id)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }
            return data.FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        ///     Save a new AccountViewModel or update an existing one.
        /// </summary>
        /// <param name="accountViewModelaccountViewModelToDelete to save</param>
        public bool Save(AccountViewModel accountViewModel)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            if (string.IsNullOrWhiteSpace(accountViewModel.Name))
            {
                accountViewModel.Name = Strings.NoNamePlaceholderLabel;
            }

            if (accountViewModel.Id == 0)
            {
                data.Add(accountViewModel);
                data = new List<AccountViewModel>(data.OrderBy(x => x.Name));
            }

            return dataAccess.SaveItem(accountViewModel);
        }

        /// <summary>
        ///     Deletes the passed AccountViewModel and removes it from cache
        /// </summary>
        /// <param name="accountViewModelToDeleteaccountViewModelToDelete to delete</param>
        public bool Delete(AccountViewModel accountViewModelToDelete)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            data.Remove(accountViewModelToDelete);
            return dataAccess.DeleteItem(accountViewModelToDelete);
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<AccountViewModel, bool>> filter = null)
        {
            data = dataAccess.LoadList();
        }
    }
}