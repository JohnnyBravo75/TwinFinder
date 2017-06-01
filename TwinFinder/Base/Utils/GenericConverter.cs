using System;

namespace TwinFinder.Base.Utils
{
    /// <summary>
    ///  Converts from "object" general type to a concrete type
    /// </summary>
    public class GenericConverter
    {
        /// <summary>
        /// Returns True if the type can get Null as a value (is a reference type and not a value one)
        /// </summary>
        public static bool IsNullable(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            Type typeDef = type.GetGenericTypeDefinition();
            return (typeDef.Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// Returns a real type of a first generic argument
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static Type UnderlyingTypeOf(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// Converter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(object value, T defaultValue)
        {
            if (value == DBNull.Value)
            {
                return defaultValue;
            }

            Type t = typeof(T);
            if (IsNullable(t))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = UnderlyingTypeOf(t);
            }
            else
            {
                if ((value == null) && (t.IsValueType))
                {
                    return defaultValue;
                }
            }

            return (T)Convert.ChangeType(value, t);
        }
    }
}