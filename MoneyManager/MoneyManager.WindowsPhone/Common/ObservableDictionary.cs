using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;

namespace MoneyManager.Common
{
    /// <summary>
    ///     Implementation of IObservableMap that supports reentrancy for use as a default view
    ///     model.
    /// </summary>
    public class ObservableDictionary : IObservableMap<string, object>
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public event MapChangedEventHandler<string, object> MapChanged;

        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
            InvokeMapChanged(CollectionChange.ItemInserted, key);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Remove(string key)
        {
            if (_dictionary.Remove(key))
            {
                InvokeMapChanged(CollectionChange.ItemRemoved, key);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            object currentValue;
            if (_dictionary.TryGetValue(item.Key, out currentValue) &&
                Equals(item.Value, currentValue) && _dictionary.Remove(item.Key))
            {
                InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
                return true;
            }
            return false;
        }

        public object this[string key]
        {
            get { return _dictionary[key]; }
            set
            {
                _dictionary[key] = value;
                InvokeMapChanged(CollectionChange.ItemChanged, key);
            }
        }

        public void Clear()
        {
            string[] priorKeys = _dictionary.Keys.ToArray();
            _dictionary.Clear();
            foreach (string key in priorKeys)
            {
                InvokeMapChanged(CollectionChange.ItemRemoved, key);
            }
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return _dictionary.Values; }
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            int arraySize = array.Length;
            foreach (var pair in _dictionary)
            {
                if (arrayIndex >= arraySize) break;
                array[arrayIndex++] = pair;
            }
        }

        private void InvokeMapChanged(CollectionChange change, string key)
        {
            MapChangedEventHandler<string, object> eventHandler = MapChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new ObservableDictionaryChangedEventArgs(change, key));
            }
        }

        private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<string>
        {
            public ObservableDictionaryChangedEventArgs(CollectionChange change, string key)
            {
                CollectionChange = change;
                Key = key;
            }

            public CollectionChange CollectionChange { get; private set; }

            public string Key { get; private set; }
        }
    }
}