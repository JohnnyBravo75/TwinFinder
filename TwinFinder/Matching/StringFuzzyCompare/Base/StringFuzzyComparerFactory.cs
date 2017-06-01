using System.Reflection;
using TwinFinder.Base.Utils;

namespace TwinFinder.Matching.StringFuzzyCompare.Base
{
    public class StringFuzzyComparerFactory : GenericFactory
    {
        public static StringFuzzyComparer GetInstance(string typeName, params object[] parameters)
        {
            return GetInstance<StringFuzzyComparer>(Assembly.GetExecutingAssembly(), typeName, parameters);
        }
    }
}
