using System.Collections.Generic;

namespace TwinFinder.Base.Utils
{
    public class ReflectionUtil
    {
        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj is IDictionary<string, object>)
            {
                return (obj as IDictionary<string, object>)[propertyName];
            }
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}