using System.Collections.ObjectModel;
using System.IO;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Core.Repositories
{
    [ImplementPropertyChanged]
    public class RecurringTransactionRepository : IRepository<RecurringTransaction>
    {
        private readonly IDataAccess<RecurringTransaction> dataAccess;
        private ObservableCollection<RecurringTransaction> data;

        /// <summary>
        ///     Creates a RecurringTransactionRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced recurring transaction data Access</param>
        public RecurringTransactionRepository(IDataAccess<RecurringTransaction> dataAccess)
        {
            this.dataAccess = dataAccess;
            data = new ObservableCollection<RecurringTransaction>(this.dataAccess.LoadList());
        }

        /// <summary>
        ///     cached recurring transaction data
        /// </summary>
        public ObservableCollection<RecurringTransaction> Data
        {
            get { return data ?? (data = new ObservableCollection<RecurringTransaction>(dataAccess.LoadList())); }
            set
            {
                if (data == null)
                {
                    data = new ObservableCollection<RecurringTransaction>(dataAccess.LoadList());
                }
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        public RecurringTransaction Selected { get; set; }

        /// <summary>
        ///     Save a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(RecurringTransaction item)
        {
            if (item.ChargedAccount == null)
            {
                throw new InvalidDataException("charged accout is missing");
            }

            if (item.Id == 0)
            {
                data.Add(item);
            }
            dataAccess.Save(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(RecurringTransaction item)
        {
            data.Remove(item);
            dataAccess.Delete(item);
        }

        /// <summary>
        ///     Loads all recurring transactions from the database to the data collection
        /// </summary>
        public void Load()
        {
            Data = new ObservableCollection<RecurringTransaction>(dataAccess.LoadList());
        }
    }
}