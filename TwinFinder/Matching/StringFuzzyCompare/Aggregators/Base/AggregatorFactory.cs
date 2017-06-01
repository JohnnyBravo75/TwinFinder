using System.Reflection;
using TwinFinder.Base.Utils;

namespace TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base
{
    public class AggregatorFactory : GenericFactory
    {
        public static Aggregator GetInstance(string typeName, params object[] parameters)
        {
            return GetInstance<Aggregator>(Assembly.GetExecutingAssembly(), typeName, parameters);
        }
    }
}
