using System;
using System.Collections.ObjectModel;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class CategoryRepository : IRepository<Category> {
        private readonly IDataAccess<Category> _dataAccess; 

        private ObservableCollection<Category> _data;

        public CategoryRepository(IDataAccess<Category> dataAccess) {
            _dataAccess = dataAccess;
            _data = new ObservableCollection<Category>(_dataAccess.LoadList());
        }

        public ObservableCollection<Category> Data {
            get { return _data ?? (_data = new ObservableCollection<Category>(_dataAccess.LoadList())); }
            set {
                if (_data == null) {
                    _data = new ObservableCollection<Category>(_dataAccess.LoadList());
                }
                if (Equals(_data, value)) {
                    return;
                }
                _data = value;
            }
        }

        public Category Selected { get; set; }

        public void Save(Category item) {
            if (String.IsNullOrWhiteSpace(item.Name)) {
                item.Name = Translation.GetTranslation("NoNamePlaceholderLabel");
            }

            if (item.Id == 0) {
                _data.Add(item);
            }
            _dataAccess.Save(item);
        }

        public void Delete(Category item) {
            _data.Remove(item);
            _dataAccess.Delete(item);
        }
    }
}
