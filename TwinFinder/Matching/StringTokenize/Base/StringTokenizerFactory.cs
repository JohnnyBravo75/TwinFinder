using TwinFinder.Base.Utils;

namespace TwinFinder.Matching.StringTokenize.Base
{
    public class StringTokenizerFactory : GenericFactory
    {
        public static StringTokenizer GetInstance(string typeName)
        {
            return GenericFactory.GetInstance<StringTokenizer>(typeName);
        }
    }
}
