using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TwinFinder.Base.Extensions
{
    public static class DataTableExtensions
    {
        public static bool AddWhenNotExist(this DataColumnCollection columns, string columnName, Type dataType = null)
        {
            if (columnName == null)
            {
                return false;
            }

            if (!columns.Contains(columnName))
            {
                if (dataType == null)
                {
                    dataType = typeof(string);
                }

                columns.Add(columnName, dataType);

                return true;
            }

            return false;
        }

        public static bool AddWhenNotExist(this DataColumnCollection columns, DataColumn column)
        {
            if (column == null)
            {
                return false;
            }

            if (!columns.Contains(column.ColumnName))
            {
                columns.Add(column);
                return true;
            }

            return false;
        }

        public static void AddWhenNotExist(this DataColumnCollection columns, DataColumnCollection columnsToAdd)
        {
            if (columnsToAdd == null)
            {
                return;
            }

            foreach (DataColumn column in columns)
            {
                AddWhenNotExist(columns, column);
            }
        }

        public static DataTable ClearColumn(this DataTable table, DataColumn column, string whereExpression = "")
        {
            DataTable filteredTable = !string.IsNullOrEmpty(whereExpression) ? table.SelectRows(whereExpression, "")
                                                                             : table;

            foreach (DataRow row in filteredTable.Rows)
            {
                row[column] = null;
            }

            return filteredTable;
        }

        public static DataTable Delete(this DataTable table, string whereExpression = "")
        {
            DataTable filteredTable = !string.IsNullOrEmpty(whereExpression) ? table.SelectRows(whereExpression, "")
                                                                             : table;
            foreach (DataRow row in filteredTable.Rows)
            {
                row.Delete();
            }

            return filteredTable;
        }

        /// <summary>
        /// Lists the details of Column names and their types in a datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetColumnNamesAndDataType(this DataTable dt)
        {
            Dictionary<string, object> dictColumnNameAndType = new Dictionary<string, object>();
            Enumerable.Range(0, dt.Columns.Count)
                        .ToList()
                        .ForEach(i => dictColumnNameAndType.Add(dt.Columns[i].ColumnName, dt.Columns[i].DataType));

            return dictColumnNameAndType;
        }

        public static T GetField<T>(this DataRow row, string name)
        {
            return row.Field<T>(name);
        }

        public static T GetFieldbyFunction<T>(this DataRow row, string function)
        {
            int i = 0;
            foreach (var item in row.Table.Columns)
            {
                if (row.Table.Columns[i].ExtendedProperties["function"] as string == function)
                {
                    return (T)row[i];
                }
                i++;
            }

            return default(T);
        }

        public static string GetFunction(this DataColumn column)
        {
            return column.ExtendedProperties["function"] as string;
        }

        /// <summary>
        /// Checks if a datatable has rows
        /// </summary>
        /// <param name="dt">the DataTable</param>
        /// <returns></returns>
        public static bool HasRows(this DataTable dt)
        {
            return dt.Rows.Count > 0 ? true : false;
        }

        public static bool ImportExternalRow(this DataTable table, DataRow importRow)
        {
            bool wasFound = false;
            bool allFound = true;
            DataRow row = table.NewRow();
            for (int i = 0; i < importRow.Table.Columns.Count; i++)
            {
                wasFound = false;
                for (int j = 0; j < row.Table.Columns.Count; j++)
                {
                    if (importRow.Table.Columns[i].ColumnName == row.Table.Columns[j].ColumnName)
                    {
                        row[j] = importRow[i];
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                {
                    allFound = false;
                }
            }

            table.Rows.Add(row);

            return allFound;
        }

        /// <summary>
        /// Prints a DataTable to the console
        /// </summary>
        /// <param name="table">the DataTable</param>
        /// <param name="colLength">the max length of the printed columns</param>
        public static void PrintToConsole(this DataTable table, int colLength)
        {
            // print the header with the coluzmn names.
            foreach (var item in table.Columns)
            {
                string itemStr = item.ToString().TrySubstring(colLength);
                Console.Write(itemStr.PadRight(colLength) + " | ");
            }
            Console.WriteLine(""); // Print separator.
            Console.WriteLine("".PadRight((colLength + 3) * table.Columns.Count, '-'));

            // Loop over the rows.
            foreach (DataRow row in table.Rows)
            {
                // Loop over the column values.
                foreach (var item in row.ItemArray)
                {
                    string itemStr = item.ToString().TrySubstring(colLength);
                    Console.Write(itemStr.PadRight(colLength) + " | ");
                }
                Console.WriteLine(""); // Print separator.
            }
        }

        /// <summary>
        /// Filters a DataTable
        /// </summary>
        /// <param name="table">the DataTable</param>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <returns>the filtered DataTable</returns>
        public static DataTable SelectRows(this DataTable table, string whereExpression, string orderByExpression)
        {
            DataView view = new DataView(table, whereExpression, orderByExpression, DataViewRowState.CurrentRows);
            return view.ToTable();
        }

        public static void SetField<T>(this DataRow row, string name, T value)
        {
            row[name] = value;
        }

        public static void SetFieldbyFunction<T>(this DataRow row, string function, T value)
        {
            int i = 0;
            foreach (var item in row.Table.Columns)
            {
                if (row.Table.Columns[i].ExtendedProperties["function"] as string == function)
                {
                    row[i] = value;
                }
                i++;
            }
        }

        public static void SetFunction(this DataColumn column, string function)
        {
            column.ExtendedProperties["function"] = function;
        }

        public static Dictionary<string, object> ToDictionary(this DataRow row)
        {
            return ToDictionary<object>(row);
        }

        public static Dictionary<string, T> ToDictionary<T>(this DataRow row)
        {
            var rowValues = row.Table.Columns
                                    .Cast<DataColumn>()
                                    .ToDictionary(col => col.ColumnName, col => row.Field<T>(col.ColumnName));
            return rowValues;
        }
    }
}