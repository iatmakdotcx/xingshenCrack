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
    }
}