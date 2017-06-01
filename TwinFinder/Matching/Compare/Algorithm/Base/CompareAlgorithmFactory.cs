using System.Reflection;
using TwinFinder.Base.Utils;

namespace TwinFinder.Matching.Compare.Algorithm.Base
{
    public class CompareAlgorithmFactory : GenericFactory
    {
        public static CompareAlgorithm GetInstance(string typeName, params object[] parameters)
        {
            return GetInstance<CompareAlgorithm>(Assembly.GetExecutingAssembly(), typeName, parameters);
        }
    }
}
