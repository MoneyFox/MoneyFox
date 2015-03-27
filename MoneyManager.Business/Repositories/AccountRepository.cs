using System;
using System.Collections.ObjectModel;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class AccountRepository : IAccountRepository {

        private readonly IDataAccess<Account> _dataAccess; 

        private ObservableCollection<Account> _data;

        public AccountRepository(IDataAccess<Account> dataAccess) {
            _dataAccess = dataAccess;
            _data = new ObservableCollection<Account>(_dataAccess.LoadList());
        }

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

        public void Save(Account item) {
            if (String.IsNullOrWhiteSpace(item.Name)) {
                item.Name = Translation.GetTranslation("NoNamePlaceholderLabel");
            }

            if (item.Id == 0) {
                _data.Add(item);
            }
            _dataAccess.Save(item);
        }

        public void Delete(Account item) {
            _data.Remove(item);
            _dataAccess.Delete(item);
        }
    }
}
