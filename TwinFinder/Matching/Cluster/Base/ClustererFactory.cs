using System;
using System.Reflection;
using TwinFinder.Base.Utils;

namespace TwinFinder.Matching.Cluster.Base
{
    public class ClustererFactory : GenericFactory
    {
        public static new Clusterer<T> GetInstance<T>(string typeName, params object[] parameters)
        {
            Type[] genericTypes = new Type[] { typeof(T) };
            return GetInstance<Clusterer<T>>(Assembly.GetExecutingAssembly(), typeName, genericTypes, parameters);
        }
    }
}