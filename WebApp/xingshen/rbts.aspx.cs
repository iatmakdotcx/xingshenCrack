using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using xingshenSvrHelper;

namespace telegramSvr.xingshen
{
    public partial class rbts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                JObject Rep = new JObject();
                Rep["ok"] = false;
                Rep["msg"] = "";
                try
                {
                    string postdata = Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                    if (Request["a"] == "rc")
                    {
                        Rep = newUser(postdata, Rep);
                    }
                }
                finally
                {
                    Response.Write(Rep.ToString(Formatting.None));
                    Response.End();
                }
            }else if (Mak.Common.MakRequest.GetInt("gid", -1) == -1)
            {
                Response.Redirect(Request.Path + "?gid=1");
            }
        }

        private static JObject newUser(string JsonStr, JObject Rep)
        {
            JObject jo = null;
            try
            {
                jo = (JObject)JsonConvert.DeserializeObject(JsonStr);
            }
            catch (Exception exx)
            {
                Rep["msg"] = exx.Message;
                return Rep;
            }
            var jr = new JsonReader(jo);
            bool isAndroid = jr.GetString("platform") == "0";
            int cnt = jr.GetInt("cnt");
            int groupid =jr.GetInt("groupid");
            string errmsg = rbtMgr.CreateRobot2Group(groupid, isAndroid, cnt);
            if (!string.IsNullOrEmpty(errmsg))
            {
                Rep["msg"] = errmsg;
                return Rep;
            }
            Rep["ok"] = true;
            return Rep;
        }
    }

    public class JsonReader
    {
        private JObject jObj;
        public JsonReader(JObject jo)
        {
            jObj = jo;
        }
        public JToken this[string propertyName]
        {
            get
            {
                return jObj[propertyName];
            }
            set
            {
                jObj[propertyName] = value;
            }
        }

        public bool ExistsKey(string key)
        {
            return jObj[key] == null;
        }
        public string GetString(string key)
        {
            var val = jObj[key];
            if (val==null)
            {
                return "";
            }
            return val.ToString();
        }
        public int GetInt(string key, int defVal = 0)
        {
            var val = jObj[key];
            if (val == null)
            {
                return defVal;
            }
            return Mak.Common.Utils.StrToInt(val.ToString(), defVal);
        }
    }

}