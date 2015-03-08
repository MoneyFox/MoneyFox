using System.Collections.ObjectModel;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IRepository<T>{
        /// <summary>
        ///     All item loaded from the database
        /// </summary>
        ObservableCollection<T> Data { get; set; }

        /// <summary>
        ///     The selected Item
        /// </summary>
        T Selected { get; set; }

        /// <summary>
        ///     Will update an existing entry and add a non existing
        /// </summary>
        /// <param name="item">Item.</param>
        void Save(T item);

        /// <summary>
        ///     Delete the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        void Delete(T item);
    }
}
