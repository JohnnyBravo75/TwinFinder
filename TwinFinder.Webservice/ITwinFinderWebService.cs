namespace TwinFinder.Webservice
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Collections.Generic;
    using TwinFinder.Matching.Compare;

    [ServiceContract]
    public interface ITwinFinderWebService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "TwinFinderWebService/CompareRecords")]
        float CompareRecords(string sessionKey, Dictionary<string, string> record1, Dictionary<string, string> record2, CompareDefinitionGroup compareDefinitionGroup);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "TwinFinderWebService/CreateSession")]
        string CreateSession(string userName, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "TwinFinderWebService/CloseSession")]
        void CloseSession(string sessionKey);
    }
}