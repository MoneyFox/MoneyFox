namespace MoneyFox.Ui.Common.Groups;

using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using Domain.Exceptions;

/// <summary>
///     Can be used for a alphanumeric grouping. It will show the whole key as title.     This can be a single name or a
///     whole word.
/// </summary>
/// <typeparam name="T"></typeparam>
public class AlphaGroupListGroupCollection<T> : List<T>
{
    /// <summary>
    ///     The delegate that is used to get the key information.
    /// </summary>
    /// <param name="item">An object of type T</param>
    /// <returns>The key value to use for this object</returns>
    public delegate string GetKeyDelegate(T item);

    /// <summary>
    ///     Public constructor.
    /// </summary>
    /// <param name="key">The key for this group.</param>
    public AlphaGroupListGroupCollection(string key)
    {
        Key = key;
    }

    /// <summary>
    ///     Public constructor.
    /// </summary>
    /// <param name="key">The key for this group.</param>
    /// <param name="itemClickCommand">The command to execute on click.</param>
    public AlphaGroupListGroupCollection(string key, RelayCommand<T>? itemClickCommand)
    {
        Key = key;
        ItemClickCommand = itemClickCommand;
    }

    /// <summary>
    ///     The Key of this group.
    /// </summary>
    public string Key { get; }

    /// <summary>
    ///     The command to execute on a click.
    /// </summary>
    public RelayCommand<T>? ItemClickCommand { get; }

    /// <summary>
    ///     Create a list of AlphaGroup{T} with keys set by a SortedLocaleGrouping.
    /// </summary>
    /// <param name="items">The items to place in the groups.</param>
    /// <param name="ci">The CultureInfo to group and sort by.</param>
    /// <param name="getKey">A delegate to get the key from an item.</param>
    /// <param name="sort">Will sort the data if true.</param>
    /// <param name="itemClickCommand">The command to execute on a click</param>
    /// <returns>An items source for a LongListSelector</returns>
    public static List<AlphaGroupListGroupCollection<T>> CreateGroups(
        IEnumerable<T> items,
        CultureInfo ci,
        GetKeyDelegate getKey,
        bool sort = true,
        RelayCommand<T>? itemClickCommand = null)
    {
        ThrowIfNull(items);
        var list = new List<AlphaGroupListGroupCollection<T>>();
        foreach (var item in items)
        {
            var index = getKey(item);
            if (list.All(a => a.Key != index))
            {
                list.Add(new(key: index, itemClickCommand: itemClickCommand));
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
                group.Sort((c0, c1) => ci.CompareInfo.Compare(string1: getKey(c0), string2: getKey(c1)));
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
