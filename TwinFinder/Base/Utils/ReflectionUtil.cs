namespace TwinFinder.Base.Utils
{
    public class ReflectionUtil
    {
        public static object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}