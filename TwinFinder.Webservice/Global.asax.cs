namespace TwinFinder.Webservice
{
    using System;
    using System.IO;
    using System.Web;
    using log4net;
    using log4net.Config;

    public class Global : HttpApplication
    {
        private readonly static object lockCount = new object();

        private static ulong activeSessions = 0;

        private static ulong activeRequests = 0;

        private static readonly ILog log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(this.Server.MapPath("~/Configs/LogConfig.log4net")));

            log.Debug("################ Startup Server ################");

            Authenticator.Instance.Initialize();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            lock (lockCount)
            {
                activeSessions += 1;
            }

            log.Debug(string.Format("## Start Session (sessionID: {0}, ClientHost: {1},  URL: {2}) ##  ActiveSessions: {3}", this.Session.SessionID, this.Request.UserHostName, this.Request.Url, activeSessions));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (this.Request.Path.Contains(".svc"))
            {
                lock (lockCount)
                {
                    activeRequests += 1;
                }

                log.Debug(string.Format("## Begin Request (ClientHost:  {0}, URL:  {1}) ##  ActiveRequests: {2}", this.Request.UserHostName, this.Request.Url, activeRequests));
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (this.Server.GetLastError() != null)
            {
                Exception objErr = this.Server.GetLastError().GetBaseException();

                if (objErr is HttpException)
                {
                    string filePath = this.Context.Request.FilePath;
                    Uri url = ((HttpApplication)sender).Context.Request.Url;
                    log.Error(string.Format("URL: {0}, FilePath: {1}", url, filePath), objErr);
                }
                else
                {
                    log.Error(string.Format("Error: {0}", sender), this.Server.GetLastError());
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            lock (lockCount)
            {
                activeSessions -= 1;
            }

            log.Debug(string.Format("## End Session (sessionID: {0}) ##  ActiveSessions: {1}", this.Session.SessionID, activeSessions));
        }

        protected void Application_End(object sender, EventArgs e)
        {
            log.Debug("################ Shutdown Server ################");
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (this.Request.Path.Contains(".svc"))
            {
                lock (lockCount)
                {
                    activeRequests -= 1;
                }

                log.Debug(string.Format("## End Request (ClientHost: {0}, URL: {1}) ##  ActiveRequests: {2}", this.Request.UserHostName, this.Request.Url, activeRequests));
            }
        }
    }
}