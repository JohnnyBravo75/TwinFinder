using System.Reflection;
using TwinFinder.Base.Utils;

namespace TwinFinder.Matching.StringPhoneticKey.Base
{
    public class StringPhoneticKeyBuilderFactory : GenericFactory
    {
        public static StringPhoneticKeyBuilder GetInstance(string typeName, params object[] parameters)
        {
            return GetInstance<StringPhoneticKeyBuilder>(Assembly.GetExecutingAssembly(), typeName, parameters);
        }
    }
}
