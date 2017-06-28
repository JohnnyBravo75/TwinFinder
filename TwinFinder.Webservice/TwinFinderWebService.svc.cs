namespace TwinFinder.Webservice
{
    using System.Collections.Generic;
    using log4net;
    using TwinFinder.Base.Extensions;
    using TwinFinder.Matching;
    using TwinFinder.Matching.Compare;

    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TwinFinderWebService : ITwinFinderWebService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TwinFinderWebService));

        public float CompareRecords(string sessionKey, Dictionary<string, string> record1, Dictionary<string, string> record2, CompareDefinitionGroup compareDefinitionGroup)
        {
            string errorMsg = Authenticator.Instance.ValidateSession(sessionKey);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return -1;
            }

            string explainPlan = "";
            return MatchingService.Instance.CompareRecords(record1.CastToGenericDictionary(),
                                                           record2.CastToGenericDictionary(),
                                                           compareDefinitionGroup, out explainPlan);
        }

        public string CreateSession(string userName, string password)
        {
            return Authenticator.Instance.CreateSession(userName, password);
        }

        public void CloseSession(string sessionKey)
        {
            Authenticator.Instance.CloseSession(sessionKey);
        }
    }
}