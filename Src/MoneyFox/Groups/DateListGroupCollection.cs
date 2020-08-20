using MoneyFox.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MoneyFox.Groups
{
    public class DateListGroupCollection<T> : List<T>
    {
        /// <summary>
        /// The delegate that is used to get the key information.
        /// </summary>
        /// <param name="item">An object of type T</param>
        /// <returns>The key value to use for this object</returns>
        public delegate string GetKeyDelegate(T item);

        public delegate DateTime GetSortKeyDelegate(T item);

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="key">The key for this group.</param>
        /// <param name="itemClickCommand">The command to execute on click</param>
        public DateListGroupCollection(string key, Command<T>? itemClickCommand = null)
        {
            Key = key;
            ItemClickCommand = itemClickCommand;
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="key">The key for this group.</param>
        /// <param name="Subtitle">The title for this group.</param>
        /// <param name="itemClickCommand">The command to execute on click</param>
        public DateListGroupCollection(string key, string subtitle, Command<T>? itemClickCommand = null)
        {
            Key = key;
            Subtitle = subtitle;
            ItemClickCommand = itemClickCommand;
        }

        /// <summary>
        /// The Key of this group.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The Title of this group.
        /// </summary>
        public string Subtitle { get; set; } = "";

        /// <summary>
        /// The command to execute on a click.
        /// </summary>
        public Command<T> ItemClickCommand { get; }

        /// <summary>
        /// Create a list of AlphaGroup{T} with keys set by a SortedLocaleGrouping.
        /// </summary>
        /// <param name="items">The items to place in the groups.</param>
        /// <param name="getKey">A delegate to get the key from an item.</param>
        /// <param name="getSortKey">A delegate to get the key for sorting from an item.</param>
        /// <param name="sort">Will sort the data if true.</param>
        /// <param name="itemClickCommand">The command to execute on a click.</param>
        /// <returns>An items source for a LongListSelector</returns>
        public static List<DateListGroupCollection<T>> CreateGroups(IEnumerable<T> items,
                                                                    GetKeyDelegate getKey,
                                                                    GetSortKeyDelegate getSortKey,
                                                                    bool sort = true,
                                                                    Command<T>? itemClickCommand = null)
        {
            ThrowIfNull(items);

            var list = new List<DateListGroupCollection<T>>();

            foreach(T item in items)
            {
                string index = getKey(item);

                if(list.All(a => a.Key != index))
                    list.Add(new DateListGroupCollection<T>(index, itemClickCommand));

                if(!string.IsNullOrEmpty(index))
                    list.Find(a => a.Key == index).Add(item);
            }

            if(sort)
            {
                foreach(DateListGroupCollection<T> group in list)
                {
                    group.Sort((c0, c1) => getSortKey(c1).Date.Day.CompareTo(getSortKey(c0).Date.Day));
                }
            }

            return list;
        }

        private static void ThrowIfNull(object parameter)
        {
            if(parameter == null)
                throw new GroupListParameterNullException();
        }
    }
}
