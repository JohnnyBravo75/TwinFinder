using System;
using System.Text;

namespace TwinFinder.Base.Utils
{
    public class ConvertUtil
    {
        public static string StringToBase64(string str, Encoding encoding)
        {
            byte[] toEncodeAsBytes = encoding.GetBytes(str);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public static string Base64ToString(string str, Encoding encoding)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(str);
            return encoding.GetString(encodedDataAsBytes);
        }

        public static string StringToHex(string asciiString)
        {
            string hex = "";
            foreach (char chr in asciiString)
            {
                int tmp = chr;
                hex += string.Format("{0:x2}", (uint)Convert.ToUInt32(tmp.ToString()));
            }

            return hex;
        }

        public static string HexToString(string hexValue)
        {
            string strValue = "";
            while (hexValue.Length > 0)
            {
                strValue += Convert.ToChar(Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }

            return strValue;
        }

        public static bool CanChangeType<T>(object value)
        {
            T val;
            return TryChangeType<T>(value, out val);
        }

        public static bool TryChangeType<T>(object value, out T val)
        {
            try
            {
                val = (T)Convert.ChangeType(value, typeof(T));
                return true;
            }
            catch (Exception)
            {
                val = default(T);
                return false;
            }
        }

        public static bool CanChangeType(object value, Type targetType)
        {
            object val;

            try
            {
                val = Convert.ChangeType(value, targetType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}