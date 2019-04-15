using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace nnproxy
{
    public static class xingshenProxyMgr
    {
        private static ushort defaultPort = 8877;
        public static string SvrApiUrl = "http://127.0.0.1:10666/xingshen/appApi.aspx";
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

                    JObject rep;
                    string errmsg = getUserSaveData(out rep, user_name, password);
                    if (!string.IsNullOrEmpty(errmsg))
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.oResponse.headers.SetStatus(200, "Ok");
                        oS.oResponse["Content-Type"] = "application/json; charset=utf-8";
                        oS.oResponse["Connection"] = "close";
                        oS.oResponse["Server-Time"] = "1554991178";
                        oS.oResponse["Sign"] = "59a46dea1320406c36a4989aa42d6929";
                        oS.utilSetResponseBody("{\"code\":1,\"type\":1,\"message\":\"无效的网络请求!\"}");
                    }
                    else
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.oResponse.headers.SetStatus(200, "Ok");
                        foreach (JObject item in (JArray)rep["head"])
                        {
                            oS.oResponse[item["k"].ToString()] = item["v"].ToString();
                        }
                        oS.utilSetResponseBody(rep["data"].ToString());
                    }

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
                        string zbxx = jo["zbbeizhu"].ToString();
                        jo.Remove("yisizuobi");
                        jo.Remove("zbbeizhu");
                        postData = jo.ToString(Formatting.None);
                        oS.RequestBody = Encoding.UTF8.GetBytes(postData);
                        string dct = oS.RequestHeaders["Server-Time"];
                        string signStr;
                        string errMsg = getSignData(out signStr, dct, postData, zbxx, jo["uuid"].ToString());
                        if (string.IsNullOrEmpty(errMsg))
                        {
                            oS.RequestHeaders["Sign"] = signStr;
                        }
                        else
                        {
                            //失败
                            Console.WriteLine("**Err>>:" + signStr);
                            oS.utilCreateResponseAndBypassServer();
                            oS.oResponse.headers.SetStatus(200, "Ok");
                            oS.oResponse["Content-Type"] = "application/json; charset=utf-8";
                            oS.oResponse["Connection"] = "close";
                            oS.oResponse["Server-Time"] = "1554991178";
                            oS.oResponse["Sign"] = "59a46dea1320406c36a4989aa42d6929";
                            oS.utilSetResponseBody("{\"code\":1,\"type\":1,\"message\":\"无效的网络请求!\"}");
                        }
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

            var oRootCert = Fiddler.CertMaker.GetRootCertificate();
            if (oRootCert == null)
            {
                if (!Fiddler.CertMaker.createRootCert())
                {
                    throw new Exception("创建根证书失败！");
                }
                oRootCert = Fiddler.CertMaker.GetRootCertificate();
                if (oRootCert == null)
                {
                    throw new Exception("创建根证书失败！");
                }
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
            Console.WriteLine("rootCertExists:" + CertMaker.rootCertExists());
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
        private static string PostData(string url, string data, out string errMsg)
        {
            errMsg = "";
            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, err) => true;
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                byte[] buffer = encoding.GetBytes(data);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                errMsg = ex.Message;
            }
            return "";
        }

        private static string getUserSaveData(out JObject obj, string user,string pass)
        {
            obj = null;
            JObject reqData = new JObject();
            reqData["a"] = "d";
            reqData["user"] = user;
            reqData["pass"] = pass;
            string errmsg = "";
            string respData = PostData(SvrApiUrl, reqData.ToString(Formatting.None), out errmsg);
            if (!string.IsNullOrEmpty(errmsg))
            {
                return errmsg;
            }
            JObject RespJo;
            try
            {
                RespJo = (JObject)JsonConvert.DeserializeObject(respData);
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
            if ((bool)RespJo["ok"])
            {
                obj = RespJo;
            }
            else
            {
                return RespJo["msg"].ToString();
            }
            return "";
        }
        private static string getSignData(out string signStr, string dct, string postData,string zbxx, string uuid)
        {
            signStr = "";
            JObject reqData = new JObject();
            reqData["a"] = "s";
            reqData["dct"] = dct;
            reqData["data"] = postData;
            string errmsg = "";
            string respData = PostData(SvrApiUrl, reqData.ToString(Formatting.None), out errmsg);
            if (!string.IsNullOrEmpty(errmsg))
            {
                return errmsg;
            }
            JObject RespJo;
            try
            {
                RespJo = (JObject)JsonConvert.DeserializeObject(respData);
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
            if ((bool)RespJo["ok"])
            {
                signStr = RespJo["sign"].ToString();
            }
            else
            {
                return RespJo["msg"].ToString();
            }
            return "";
        }

        public static string getApiVersion(out int version)
        {
            version = 0;
            JObject reqData = new JObject();
            reqData["a"] = "v";
            string errmsg = "";
            string respData = PostData(SvrApiUrl, reqData.ToString(Formatting.None), out errmsg);
            if (!string.IsNullOrEmpty(errmsg))
            {
                return errmsg;
            }
            JObject RespJo;
            try
            {
                RespJo = (JObject)JsonConvert.DeserializeObject(respData);
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
            if ((bool)RespJo["ok"])
            {
                version = (int)RespJo["v"];
            }
            else
            {
                return RespJo["msg"].ToString();
            }
            return "";
        }
    }
}
