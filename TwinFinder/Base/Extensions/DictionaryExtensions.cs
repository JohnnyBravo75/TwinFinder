using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TwinFinder.Base.Extensions
{
    /// <summary>
    /// DictionaryExtensions
    /// </summary>
    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> addDictionary)
        {
            foreach (var item in addDictionary)
            {
                dictionary.Add(item.Key, item.Value);
            }

            return dictionary;
        }

        public static IDictionary<TKey, TValue> AddRange<TKey, TValue, TA>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TA> collection, Func<TA, TKey> getKey, Func<TA, TValue> getValue)
        {
            foreach (var item in collection)
            {
                dictionary.Add(getKey(item), getValue(item));
            }

            return dictionary;
        }

        public static IDictionary<TKey, TValue> CloneDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            IDictionary<TKey, TValue> result = dictionary.GetType().GetConstructor(new Type[0]).Invoke(null) as IDictionary<TKey, TValue>;

            foreach (var key in dictionary.Keys)
            {
                result.Add(key, dictionary[key]);
            }

            return result;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue ret;

            if (!dictionary.TryGetValue(key, out ret))
            {
                dictionary[key] = ret = new TValue();
            }

            return ret;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue initialValue)
        {
            TValue ret;

            if (!dictionary.TryGetValue(key, out ret))
            {
                dictionary[key] = ret = initialValue;
            }

            return ret;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return default(TValue);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value
                 : defaultValueProvider();
        }

        public static IDictionary<TKey, TValue> RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> condition)
        {
            var temp = new List<TKey>();

            foreach (var item in dictionary)
            {
                if (!condition(item))
                {
                    temp.Add(item.Key);
                }
            }

            foreach (var itemKey in temp)
            {
                dictionary.Remove(itemKey);
            }

            return dictionary;
        }

        public static IDictionary<TKey, TValue> RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            if (values != null)
            {
                foreach (var x in values)
                {
                    dictionary.Remove(x);
                }
            }

            return dictionary;
        }

        public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = dictionary[key];
                dictionary.Remove(key);
                return true;
            }
        }

        public static DataTable ToDataTable<T>(this IDictionary<string, T> dictionary)
        {
            var list = new List<IDictionary<string, T>>();
            list.Add(dictionary);
            return ToDataTable<T>(list);
        }

        public static DataTable ToDataTable<T>(this IList<IDictionary<string, T>> dictionary)
        {
            DataTable dataTable = new DataTable();

            if (dictionary == null || !dictionary.Any())
            {
                return dataTable;
            }

            foreach (var column in dictionary.First().Select(c => new DataColumn(c.Key, c.Value != null ? c.Value.GetType()
                                                                                                        : typeof(T))))
            {
                dataTable.Columns.Add((DataColumn)column);
            }

            foreach (var row in dictionary.Select(
                r =>
                {
                    var dataRow = dataTable.NewRow();
                    r.ToList().ForEach(c => dataRow.SetField<T>(c.Key, c.Value));
                    return dataRow;
                }))
            {
                dataTable.Rows.Add((DataRow)row);
            }

            return dataTable;
        }

        public static Dictionary<string, object> CastToGenericDictionary(this IDictionary dictionary)
        {
            var result = new Dictionary<string, object>();
            foreach (DictionaryEntry entry in dictionary)
            {
                result.Add(entry.Key.ToString(), entry.Value);
            }

            return result;
        }
    }
}