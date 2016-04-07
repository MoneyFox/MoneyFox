using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MoneyFox.Shared.Groups
{
    public class DateListGroup<T> : List<T>
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
        public DateListGroup(string key)
        {
            Key = key;
        }

        /// <summary>
        ///     The Key of this group.
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping.
        /// </summary>
        /// <param name="items">The items to place in the groups.</param>
        /// <param name="ci">The CultureInfo to group and sort by.</param>
        /// <param name="getKey">A delegate to get the key from an item.</param>
        /// <param name="getSortKey">A delegate to get the key for sorting from an item.</param>
        /// <param name="sort">Will sort the data if true.</param>
        /// <returns>An items source for a LongListSelector</returns>
        public static List<DateListGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, GetKeyDelegate getKey,
            GetSortKeyDelegate getSortKey, bool sort)
        {
            var list = new List<DateListGroup<T>>();

            foreach (var item in items)
            {
                var index = getKey(item);

                if (list.All(a => a.Key != index))
                {
                    list.Add(new DateListGroup<T>(index));
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
    }
}