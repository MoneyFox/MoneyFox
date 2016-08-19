using System.Collections.ObjectModel;

namespace MoneyFox.Shared.Interfaces
{
    public interface IData<T>
    {
        /// <summary>
        ///     All payment loaded from the database
        /// </summary>
        ObservableCollection<T> Data { get; set; }

    }
}