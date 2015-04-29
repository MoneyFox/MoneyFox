using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class AccountRepository : IAccountRepository {
        private readonly IDataAccess<Account> _dataAccess;
        private ObservableCollection<Account> _data;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced account data Access</param>
        public AccountRepository(IDataAccess<Account> dataAccess) {
            _dataAccess = dataAccess;
            _data = new ObservableCollection<Account>(_dataAccess.LoadList());
        }

        /// <summary>
        ///     Cached account data
        /// </summary>
        public ObservableCollection<Account> Data {
            get { return _data ?? (_data = new ObservableCollection<Account>(_dataAccess.LoadList())); }
            set {
                if (_data == null) {
                    _data = new ObservableCollection<Account>(_dataAccess.LoadList());
                }
                if (Equals(_data, value)) {
                    return;
                }
                _data = value;
            }
        }

        public Account Selected { get; set; }

        /// <summary>
        ///     Save a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(Account item) {
            if (String.IsNullOrWhiteSpace(item.Name)) {
                item.Name = Translation.GetTranslation("NoNamePlaceholderLabel");
            }

            if (item.Id == 0) {
                _data.Add(item);
            }
            _dataAccess.Save(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(Account item) {
            _data.Remove(item);
            _dataAccess.Delete(item);

            TransactionLogic.DeleteAssociatedTransactionsFromDatabase(item);
        }
    }
}