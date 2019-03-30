
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace telegramSvr
{


    public class Global : System.Web.HttpApplication
    {        
       
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            Web.Model.ModelBase.Init("xinshen");

            xingshen.xingshenProxyMgr2.Start();
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
#if !DEBUG
            //Exception objErr = Server.GetLastError().GetBaseException();
            //StringBuilder sberror = new StringBuilder();
            //sberror.Append("1.异常页面: " + Request.Url + "\r\n");
            //sberror.Append("2.异常信息: " + objErr.Message + "\r\n");
            //sberror.Append("3.堆栈信息: \r\n" + objErr.StackTrace + "\r\n");
            //log4net.ILog log = log4net.LogManager.GetLogger("logerror");            
            //log.Error(sberror.ToString());
            //Server.ClearError();
            //Application["error"] = sberror.Replace("\r\n", "<br />");
            //Response.Redirect("~/ErrorPage.aspx");
#endif
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            xingshen.xingshenProxyMgr2.Stop();
        }
    }
}