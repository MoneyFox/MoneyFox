using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.Repositories
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
            data = new ObservableCollection<Account>(this.dataAccess.LoadList());
        }

        /// <summary>
        ///     Cached account data
        /// </summary>
        public ObservableCollection<Account> Data
        {
            get { return data ?? (data = new ObservableCollection<Account>(dataAccess.LoadList())); }
            set
            {
                if (data == null)
                {
                    data = new ObservableCollection<Account>(dataAccess.LoadList());
                }
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        public Account Selected { get; set; }

        /// <summary>
        ///     SaveItem a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(Account item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.Name = Strings.NoNamePlaceholderLabel;
            }

            if (item.Id == 0)
            {
                data.Add(item);
            }
            dataAccess.SaveItem(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(Account item)
        {
            data.Remove(item);
            dataAccess.DeleteItem(item);

            //TODO refactor this
            //transactionManager.DeleteAssociatedTransactionsFromDatabase(item);
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Account, bool>> filter = null)
        {
            Data = new ObservableCollection<Account>(dataAccess.LoadList(filter));
        }
    }
}