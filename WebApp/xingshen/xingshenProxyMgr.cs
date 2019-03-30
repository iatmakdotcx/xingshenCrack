using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Web.Model;

namespace telegramSvr.xingshen
{
    public static class xingshenProxyMgr2
    {
        private static ushort defaultPort = 8877;
        public static List<Fiddler.Session> oAllSessions;

        public static void Start()
        {
            oAllSessions = new List<Fiddler.Session>();
            Fiddler.FiddlerApplication.BeforeRequest += delegate (Fiddler.Session oS)
            {
                oS.bBufferResponse = false;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);

                if (oS.fullUrl.EndsWith("api/v1/users/first_login"))
                {
                    string postData = Encoding.UTF8.GetString(oS.RequestBody);
                    JObject jo;
                    try
                    {
                        jo = (JObject)JsonConvert.DeserializeObject(postData);
                    }
                    catch (Exception exx)
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.oResponse.headers.SetStatus(200, "Ok");
                        oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                        oS.oResponse["Cache-Control"] = "private, max-age=0";
                        oS.utilSetResponseBody("Error");
                        return;
                    }

                    string user_name = jo["user_name"].ToString();
                    string password = jo["password"].ToString();
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    string dataStr = svrHelper.Create_first_login(user_name, password, ref headers);
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.SetStatus(200, "Ok");
                    foreach (var item in headers)
                    {
                        oS.oResponse[item.Key] = item.Value;
                    }
                    oS.utilSetResponseBody(dataStr);
                }
                else if (oS.fullUrl.EndsWith("api/v2/users/save_user"))
                {
                    //存档包，拦截作弊提示
                    string postData = Encoding.UTF8.GetString(oS.RequestBody);
                    JObject jo;
                    try
                    {
                        jo = (JObject)JsonConvert.DeserializeObject(postData);
                    }
                    catch (Exception exx)
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.oResponse.headers.SetStatus(200, "Ok");
                        oS.oResponse["Content-Type"] = "application/json; charset=utf-8";
                        oS.oResponse["Connection"] = "close";
                        oS.oResponse["Server-Time"] = "1553825434";
                        oS.oResponse["Connection"] = "Connection";
                        oS.utilSetResponseBody("{\"code\":1,\"type\":1,\"message\":\"无效的网络请求!\"}");
                        return;
                    }

                    if (jo["yisizuobi"] != null)
                    {                        
                        XingshenUserDataWarning ww = new XingshenUserDataWarning();
                        ww.jgxx = jo["zbbeizhu"].ToString();
                        ww.uuid = jo["uuid"].ToString();
                        ww.jgrq = DateTime.Now;
                        ww.Add();
                        jo.Remove("yisizuobi");
                        jo.Remove("zbbeizhu");
                        postData = jo.ToString(Formatting.None);
                        oS.RequestBody = Encoding.UTF8.GetBytes(postData);
                        string dct = oS.RequestHeaders["Server-Time"];
                        oS.RequestHeaders["Sign"] = svrHelper.SignData(dct, postData);
                    }
                }
                else if (oS.fullUrl == "https://www.wireshark.org/update/0/Wireshark/2.6.6/Windows/x86-64/en-US/stable.xml")
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.SetStatus(200, "Ok");
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>" + oS.fullUrl + "<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
            };
            Fiddler.FiddlerApplication.AfterSessionComplete += delegate (Fiddler.Session oS)
            {
                Console.WriteLine("Finished session:\t" + oS.fullUrl);
                //Console.Title = ("Session list contains: " + oAllSessions.Count.ToString() + " sessions");
            };

            Fiddler.CONFIG.IgnoreServerCertErrors = true;

            CONFIG.bDebugSpew = true;
            CONFIG.bAutoProxyLogon = false;
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.DecryptSSL | FiddlerCoreStartupFlags.MonitorAllConnections | FiddlerCoreStartupFlags.OptimizeThreadPool | FiddlerCoreStartupFlags.AllowRemoteClients;
            Fiddler.FiddlerApplication.Startup(defaultPort, oFCSF);
        }
        public static void Stop()
        {
            Fiddler.FiddlerApplication.Shutdown();
        }

        public static int ProxyPort()
        {
            if (IsRunning())
            {
                return 8877;
            }
            return 0;
        }
        public static bool IsRunning()
        {
            return Fiddler.FiddlerApplication.IsStarted();
        }
    }
}