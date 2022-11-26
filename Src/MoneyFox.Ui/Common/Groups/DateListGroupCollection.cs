namespace MoneyFox.Ui.Common.Groups;

using CommunityToolkit.Mvvm.Input;
using Core.ApplicationCore.Domain.Exceptions;

public class DateListGroupCollection<T> : List<T>
{
    /// <summary>
    ///     The delegate that is used to get the key information.
    /// </summary>
    /// <param name="item">An object of type T</param>
    /// <returns>The key value to use for this object</returns>
    public delegate string GetKeyDelegate(T item);

    public delegate DateTime GetSortKeyDelegate(T item);

    /// <summary>
    ///     Public constructor.
    /// </summary>
    /// <param name="key">The key for this group.</param>
    /// <param name="itemClickCommand">The command to execute on click</param>
    public DateListGroupCollection(string key, string subtitle, RelayCommand<T>? itemClickCommand = null)
    {
        Key = key;
        Subtitle = subtitle;
        ItemClickCommand = itemClickCommand;
    }

    /// <summary>
    ///     Public constructor.
    /// </summary>
    /// <param name="key">The key for this group.</param>
    /// <param name="itemClickCommand">The command to execute on click</param>
    /// <param name="date">The date for this group.</param>
    private DateListGroupCollection(string key, RelayCommand<T>? itemClickCommand = null, DateTime? date = null)
    {
        Key = key;
        ItemClickCommand = itemClickCommand;
        Date = date;
    }

    /// <summary>
    ///     The Key of this group.
    /// </summary>
    public string Key { get; }

    /// <summary>
    ///     The Title of this group.
    /// </summary>
    public string Subtitle { get; set; } = "";

    public DateTime? Date { get; }

    /// <summary>
    ///     The command to execute on a click.
    /// </summary>
    public RelayCommand<T>? ItemClickCommand { get; }

    /// <summary>
    ///     Create a list of AlphaGroup{T} with keys set by a SortedLocaleGrouping.
    /// </summary>
    /// <param name="items">The items to place in the groups.</param>
    /// <param name="getKey">A delegate to get the key from an item.</param>
    /// <param name="getSortKey">A delegate to get the key for sorting from an item.</param>
    /// <param name="sort">Will sort the data if true.</param>
    /// <param name="itemClickCommand">The command to execute on a click.</param>
    /// <returns>An items source for a LongListSelector</returns>
    public static List<DateListGroupCollection<T>> CreateGroups(
        IEnumerable<T> items,
        GetKeyDelegate getKey,
        GetSortKeyDelegate getSortKey,
        bool sort = true,
        RelayCommand<T>? itemClickCommand = null)
    {
        ThrowIfNull(items);
        var list = new List<DateListGroupCollection<T>>();
        foreach (var item in items)
        {
            var index = getKey(item);
            if (list.All(a => a.Key != index))
            {
                list.Add(new(key: index, itemClickCommand: itemClickCommand, date: getSortKey(item).Date));
            }

            if (!string.IsNullOrEmpty(index))
            {
                list.Find(a => a.Key == index).Add(item);
            }
        }

        if (sort)
        {
            foreach (var group in list)
            {
                group.Sort((c0, c1) => getSortKey(c1).Date.Day.CompareTo(getSortKey(c0).Date.Day));
            }
        }

        return list;
    }

    private static void ThrowIfNull(object parameter)
    {
        if (parameter == null)
        {
            throw new GroupListParameterNullException();
        }
    }
}

