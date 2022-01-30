using System.Collections.Generic;

namespace MoneyFox.Core._Pending_.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                collection.Add(item);
            }
        }
    }
}
