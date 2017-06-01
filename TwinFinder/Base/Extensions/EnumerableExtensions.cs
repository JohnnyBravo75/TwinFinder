using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace TwinFinder.Base.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Contains(this IEnumerable collection, object item)
        {
            foreach (var o in collection)
            {
                if (o == item)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }

            return collection;
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, T item)
        {
            int i = 0;
            foreach (var val in collection)
            {
                if (EqualityComparer<T>.Default.Equals(item, val))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            int i = 0;
            foreach (var val in collection)
            {
                if (condition(val))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return !collection.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// Determines whether a collection is null or has no elements without having to enumerate the entire collection to get a count.  Uses LINQ.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>
        /// <c>true</c> if the collection is null or has no elements; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            if (items != null)
            {
                return !items.GetEnumerator().MoveNext();
            }

            return true;
        }

        /// <summary>
        /// Creates an array from a enumerable source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static T[] ToArray<T>(IEnumerable<T> enumerable)
        {
            if (enumerable is T[])
                return (T[])enumerable;

            List<T> tempList = new List<T>(enumerable);
            return tempList.ToArray();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            // DataTable mit Namen aus GUID erstellen:
            DataTable dt = new DataTable(Guid.NewGuid().ToString());

            // Spaltennamen:
            PropertyInfo[] cols = null;

            // Ist das LINQ-Ergebnis null wird ein leeres DataTable
            // zurückgegeben:
            if (list == null)
                return dt;

            // Alle Elemente der Liste durchlaufen (LINQ-Ergebnis):
            foreach (T item in list)
            {
                // Die Spaltennamen werden per Reflektion ermittelt.
                // Wird nur beim 1. Durchlauf ermittelt:
                if (cols == null)
                {
                    // Alle Spalten ermitteln:
                    cols = item.GetType().GetProperties();

                    // Spalten durchlaufen und im DataTable die Spalten erstellen:
                    foreach (PropertyInfo pi in cols)
                    {
                        // Spaltentyp:
                        Type colType = pi.PropertyType;

                        if (colType.IsGenericType &&
                            colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            colType = colType.GetGenericArguments()[0];

                        // Spalte der DataTable hinzufügen:
                        dt.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                // Zeile hinzufügen:
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pi in cols)
                    dr[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        //public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        //{
        //    DataTable dtReturn = new DataTable();

        //    // column names
        //    PropertyInfo[] oProps = null;

        //    oProps = typeof(T).GetProperties();
        //    foreach (PropertyInfo pi in oProps)
        //    {
        //        Type colType = pi.PropertyType;

        //        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
        //        {
        //            colType = colType.GetGenericArguments()[0];
        //        }

        //        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
        //    }

        //    if (varlist == null) return dtReturn;

        //    foreach (T rec in varlist)
        //    {
        //        DataRow dr = dtReturn.NewRow();

        //        foreach (PropertyInfo pi in oProps)
        //        {
        //            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
        //            (rec, null);
        //        }

        //        dtReturn.Rows.Add(dr);
        //    }
        //    return dtReturn;
        //}
        /// <summary>
        /// Traversiert über ein hierarchisches IEnumerable
        /// </summary>
        /// <typeparam name="T">Parameter, der den Typ der IEnumerable-Member angibt</typeparam>
        /// <param name="source">die übergebene IEnumerable</param>
        /// <param name="functionRecurse">Funktion die die Child-Elemente für T ermittelt</param>
        /// <returns>IEnumerable der flachgeklopften Hierarchie</returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> functionRecurse)
        {
            foreach (T item in source)
            {
                yield return item;

                IEnumerable<T> seqRecurse = functionRecurse(item);

                if (seqRecurse != null)
                {
                    foreach (T itemRecurse in Traverse(seqRecurse, functionRecurse))
                    {
                        yield return itemRecurse;
                    }
                }
            }
        }
    }
}