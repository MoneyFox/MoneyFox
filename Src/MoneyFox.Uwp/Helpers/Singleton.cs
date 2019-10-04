using System;
using System.Collections.Concurrent;

namespace MoneyFox.Uwp.Helpers
{
    public static class Singleton<T>
        where T : new()
    {
        private static ConcurrentDictionary<Type, T> INSTANCES = new ConcurrentDictionary<Type, T>();

        public static T Instance
        {
            get
            {
                return INSTANCES.GetOrAdd(typeof(T), (t) => new T());
            }
        }
    }
}
