using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Web.Model;

namespace xingshenSvrHelper
{
    public static class xingshenProxyMgr
    {
        private static ushort defaultPort = 18877;
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
                        oS.oResponse["Server-Time"] = "1554991178";
                        oS.oResponse["Sign"] = "59a46dea1320406c36a4989aa42d6929";
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

            CONFIG.bDebugSpew = true;
            CONFIG.bAutoProxyLogon = false;

            FiddlerApplication.Log.OnLogString += delegate (object loger, LogEventArgs e)
            {
                //throw new Exception(e.LogString);
                Console.WriteLine(e.LogString);
            };
            //var oRootCert = new X509Certificate2("sss.pfx", "", X509KeyStorageFlags.Exportable);
            //var z = (RSACryptoServiceProvider)oRootCert.PrivateKey;
            //var cc = DotNetUtilities.GetRsaKeyPair(z);
            //var PrivateKeyInfo = Org.BouncyCastle.Pkcs.PrivateKeyInfoFactory.CreatePrivateKeyInfo(cc.Private);
            //byte[] derEncoded = PrivateKeyInfo.ToAsn1Object().GetDerEncoded();
            //FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.key", Convert.ToBase64String(derEncoded));
            //FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.cert", Convert.ToBase64String(oRootCert.Export(X509ContentType.Cert)));

            if (!Fiddler.CertMaker.createRootCert())
            {
                throw new Exception("创建根证书失败！");
            }
            // mono创建证书会失败，直接读取一个已有证书就是了
            var oRootCert = Fiddler.CertMaker.GetRootCertificate();
            //if (oRootCert == null)
            {
                X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                certStore.Open(OpenFlags.ReadWrite);
                try
                {
                    certStore.Add(oRootCert);
                }
                finally
                {
                    certStore.Close();
                }
                Console.WriteLine("=============================save my ok=================================");
                X509Store x509Store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                x509Store.Open(OpenFlags.ReadWrite);
                try
                {
                    x509Store.Add(oRootCert);
                }
                finally
                {
                    x509Store.Close();
                }
                Console.WriteLine("=============================save root ok=================================");
                oRootCert = Fiddler.CertMaker.GetRootCertificate();
                if (oRootCert == null)
                {
                    throw new Exception("保存根证书失败！");
                }
            }
            
            Console.WriteLine("==============================================================");
            Console.WriteLine("RootCertHasPrivateKey:" + oRootCert.HasPrivateKey);
            Console.WriteLine("rootCertExists:"+ CertMaker.rootCertExists());
            Console.WriteLine("rootCertIsTrusted:" + CertMaker.rootCertIsTrusted());
            if (!CertMaker.rootCertIsTrusted())
            {
                CertMaker.trustRootCert();
                Console.WriteLine("**rootCertIsTrusted:" + CertMaker.rootCertIsTrusted());
            }
            Console.WriteLine("==============================================================");
            Console.WriteLine(oRootCert);
            Console.WriteLine("==============================================================");
           
            Fiddler.CONFIG.IgnoreServerCertErrors = true;
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.DecryptSSL | FiddlerCoreStartupFlags.MonitorAllConnections | FiddlerCoreStartupFlags.OptimizeThreadPool | FiddlerCoreStartupFlags.AllowRemoteClients;
            Fiddler.FiddlerApplication.Startup(defaultPort, oFCSF);
            Console.WriteLine(Fiddler.FiddlerApplication.GetDetailedInfo());
            
        }
        public static void Stop()
        {
            Fiddler.FiddlerApplication.Shutdown();
        }

        public static int ProxyPort()
        {
            if (IsRunning())
            {
                return defaultPort;
            }
            return 0;
        }
        public static bool IsRunning()
        {
            return Fiddler.FiddlerApplication.IsStarted();
        }
    }
}
