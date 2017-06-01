using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace TwinFinder.Base.Extensions
{
    public static class CollectionExtensions
    {
        public static IList<T> AddOrUpdate<T>(this IList<T> collection, T item)
        {
            if (item != null)
            {
                var oldItems = collection.Where(x => x.Equals(item));

                if (!oldItems.Any())
                {
                    collection.Add(item);
                }
                else
                {
                    foreach (T oldItem in oldItems)
                    {
                        int index = collection.IndexOf(oldItem);
                        collection[index] = item;
                    }
                }
            }

            return collection;
        }

        public static IList<T> AddRange<T>(this IList<T> collection, IEnumerable<T> range)
        {
            if (range != null)
            {
                foreach (var x in range)
                {
                    collection.Add(x);
                }
            }

            return collection;
        }

        public static IList<T> AddRangeDistinct<T>(this IList<T> collection, IEnumerable<T> range)
        {
            if (range != null)
            {
                foreach (var x in range)
                {
                    if (!collection.Contains(x))
                    {
                        collection.Add(x);
                    }
                }
            }

            return collection;
        }

        public static T GetByIndex<T>(this IList<T> list, int index)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return list[index];
        }

        public static bool ListEquals<T>(IList<T> a, IList<T> b)
        {
            if (a == null || b == null)
                return (a == null && b == null);

            if (a.Count != b.Count)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < a.Count; i++)
            {
                if (!comparer.Equals(a[i], b[i]))
                    return false;
            }

            return true;
        }

        public static IList<T> RemoveRange<T>(this IList<T> collection, IEnumerable<T> removerange)
        {
            if (removerange != null)
            {
                foreach (var x in removerange)
                {
                    collection.Remove(x);
                }
            }

            return collection;
        }

        public static IList<T> SwapElements<T>(this IList<T> collection, int index1, int index2)
        {
            T temp = collection[index1];
            collection[index1] = collection[index2];
            collection[index2] = temp;

            return collection;
        }

        public static string[] ToArray(this MatchCollection matchCollection)
        {
            string[] strMatches = new string[matchCollection.Count];
            int i = 0;
            foreach (var match in matchCollection)
            {
                strMatches[i] = match.ToString();
                i++;
            }

            return strMatches;
        }

        public static List<T> ToList<T>(ICollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            T[] array = new T[collection.Count];
            collection.CopyTo(array, 0);

            return new List<T>(array);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
        {
            if (enumerableList != null)
            {
                ////create an emtpy observable collection object
                var observableCollection = new ObservableCollection<T>();

                ////loop through all the records and add to observable collection object
                foreach (var item in enumerableList)
                {
                    observableCollection.Add(item);
                }
                ////return the populated observable collection
                return observableCollection;
            }

            return null;
        }

        public static int IndexOf<T>(this T[] array, T value) where T : class
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool ContainsAll<T>(this T[] array1, T[] array2)
        {
            foreach (var item in array2)
            {
                if (!array1.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        public static List<T> ToList<T>(params T[] array)
        {
            return new List<T>(array);
        }
    }
}