using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Model;
using xingshenSvrHelper;

namespace telegramSvr.xingshen
{
    public partial class appApi : System.Web.UI.Page
    {
        private const int APIVERSION = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                string a = Request["a"];
                JObject Rep = new JObject();
                Rep["ok"] = false;
                Rep["msg"] = "";
                try
                {
                    string reqStr = System.Text.Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                    JObject ReqJo = null;
                    try
                    {
                        ReqJo = (JObject)JsonConvert.DeserializeObject(reqStr);
                    }
                    catch (Exception exx)
                    {
                        Rep["msg"] = exx.Message;
                        return;
                    }
                    switch (ReqJo["a"].ToString())
                    {
                        case "v":
                            checkVersion(ReqJo, Rep);
                            break;
                        case "d":
                            downUserData(ReqJo, Rep);
                            break;
                        case "s":
                            SignData(ReqJo, Rep);
                            break;
                        case "c":
                            CheckCodeData(ReqJo, Rep);
                            break;
                        default:
                            break;
                    }

                }
                finally
                {
                    if ((bool)Rep["ok"] == false && Rep["msg"].ToString() == "")
                    {
                        Rep["msg"] = "系统错误";
                    }
                    Response.Write(Rep.ToString(Formatting.None));
                    Response.End();
                }
            }
        }
        private void checkVersion(JObject ReqJo, JObject Rep)
        {
            Rep["v"] = APIVERSION;
            Rep["ok"] = true;
        }
        private void downUserData(JObject ReqJo, JObject Rep)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            string dataStr = svrHelper.Create_first_login(ReqJo["user"].ToString(), ReqJo["pass"].ToString(), ref headers);
            Rep["data"] = dataStr;
            JArray head = new JArray();
            foreach (var item in headers)
            {
                JObject ar = new JObject();
                ar["k"] = item.Key;
                ar["v"] = item.Value;
                head.Add(ar);
            }
            Rep["head"] = head;
            Rep["ok"] = true;
        }
        private void SignData(JObject ReqJo, JObject Rep)
        {
            if (ReqJo["zbxx"] != null && ReqJo["zbxx"].ToString() != "" && ReqJo["uuid"] != null && ReqJo["uuid"].ToString() != "")
            {
                XingshenUserDataWarning ww = new XingshenUserDataWarning();
                ww.jgxx = ReqJo["zbxx"].ToString();
                ww.uuid = ReqJo["uuid"].ToString();
                ww.jgrq = DateTime.Now;
                ww.Add();
            }

            string dct = ReqJo["dct"].ToString();
            string data = ReqJo["data"].ToString();
            Rep["sign"] = svrHelper.SignData(dct, data);
            Rep["ok"] = true;
        }
        private void CheckCodeData(JObject ReqJo, JObject Rep)
        {
            JObject jo;
            try
            {
                jo = (JObject)JsonConvert.DeserializeObject(ReqJo["data"].ToString());
            }
            catch (Exception exx)
            {
                return;
            }
            XingshenCheckcode xcc = new XingshenCheckcode();
            xcc.code = jo["code"].ToString();
            xcc.mac_addr = jo["mac_address"].ToString();
            xcc.player_name = jo["player_name"].ToString();
            xcc.token = jo["token"].ToString();
            xcc.user_name = jo["user_name"].ToString();
            xcc.uuid = jo["uuid"].ToString();
            xcc.net_id = int.Parse(jo["net_id"].ToString());
            xcc.sg_version = jo["sg_version"].ToString();
            xcc.Add();

            JObject GETBODY = new JObject();
            if (xcc.code.StartsWith("jinbi:"))
            {
                string tmpsl = xcc.code.Substring(xcc.code.IndexOf(":") + 1);
                int sl;
                if (int.TryParse(tmpsl, out sl))
                {
                    GETBODY["jinbi"] = sl;
                }
            } else if (xcc.code.StartsWith("yuanbao:"))
            {
                string tmpsl = xcc.code.Substring(xcc.code.IndexOf(":") + 1);
                int sl;
                if (int.TryParse(tmpsl, out sl))
                {
                    GETBODY["yuanbao"] = sl;
                }
            }
            else if (xcc.code.StartsWith("hyJiFen:"))
            {
                string tmpsl = xcc.code.Substring(xcc.code.IndexOf(":") + 1);
                int sl;
                if (int.TryParse(tmpsl, out sl))
                {
                    GETBODY["hyJiFen"] = sl;
                }
            }
            else if (xcc.code.StartsWith("yueka:"))
            {
                string tmpsl = xcc.code.Substring(xcc.code.IndexOf(":") + 1);
                int sl;
                if (int.TryParse(tmpsl, out sl))
                {
                    GETBODY["yueka"] = sl;
                }
            }
            else if (xcc.code.StartsWith("shl:"))
            {
                string tmpsl = xcc.code.Substring(xcc.code.IndexOf(":") + 1);
                int sl;
                if (int.TryParse(tmpsl, out sl))
                {
                    GETBODY["shl"] = sl;
                }
            }
            else if (xcc.code.StartsWith("wp:"))
            {
                string[] tmppars = xcc.code.Substring(xcc.code.IndexOf(":") + 1).Split(',');
                if (tmppars.Length >= 2)
                {
                    int itemType = 0;
                    int childType = 0;
                    int num = 1;
                    if (int.TryParse(tmppars[0],out itemType) && int.TryParse(tmppars[1], out childType))
                    {
                        if (tmppars.Length >= 3)
                        {
                            int.TryParse(tmppars[2], out num);
                        }
                        JArray itemGetArr = new JArray();
                        JObject joitem = new JObject();
                        joitem["itemType"] = itemType;
                        joitem["childType"] = childType;
                        joitem["num"] = num;
                        itemGetArr.Add(joitem);
                        GETBODY["itemGetArr"] = itemGetArr;
                    }
                }
            }
            else if (xcc.code.StartsWith("def:"))
            {
                string tmppars = xcc.code.Substring(xcc.code.IndexOf(":") + 1);
                string fn = System.IO.Path.GetFileName(Request.Path);
                tmppars = Request.Path.Substring(0, Request.Path.Length - fn.Length) + "codedef/" + tmppars + ".txt";
                tmppars = Mak.Common.Utils.GetMapPath(tmppars);
                if (System.IO.File.Exists(tmppars))
                {
                    tmppars = System.IO.File.ReadAllText(tmppars);
                    try
                    {
                        GETBODY["itemGetArr"] = (JArray)JsonConvert.DeserializeObject(tmppars);
                    }
                    catch (Exception)
                    {}
                }
            }
            if (GETBODY.HasValues)
            {
                JObject ppd = new JObject();
                ppd["error"] = 0;
                ppd["GETBODY"] = GETBODY;

                string data = ppd.ToString(Formatting.None);
                Rep["data"] = data;
                
                string dct = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers["Content-Type"] = "application/json; charset=utf-8";
                headers["Connection"] = "close";
                headers["Cache-Control"] = "max-age=0, private, must-revalidate";
                headers["Server-Time"] = dct;
                headers["Sign"] = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data + "QAbxK1exZYrK6WIO" + dct, "MD5").ToLower();

                JArray head = new JArray();
                foreach (var item in headers)
                {
                    JObject ar = new JObject();
                    ar["k"] = item.Key;
                    ar["v"] = item.Value;
                    head.Add(ar);
                }
                Rep["head"] = head;
                Rep["ok"] = true;
            }
            else
            {
                Rep["msg"] = "skip";
            }
        }
    }
}