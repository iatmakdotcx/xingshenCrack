using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Model;

namespace telegramSvr.xingshen
{
    public partial class login : pubPagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                Page_Load_POST();
            }
        }

        private void Page_Load_POST()
        {
            JObject Rep = new JObject();
            Rep["ok"] = false;
            Rep["msg"] = "";
            try
            {
                string Data = Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                if (Request["a"] == "login")
                {
                    Rep["msg"] = "用户名或密码错误！";
                    string username = "";
                    string password = "";
                    try
                    {
                        JObject jo = (JObject)JsonConvert.DeserializeObject(Data);
                        username = jo["user_name"].ToString();
                        password = jo["password"].ToString();
                    }
                    catch (Exception )
                    {
                        return;
                    }
                    OptAdmin admin = OptAdmin_BLL.GetModel(username);
                    if (admin.id != 0 && admin.pass == password)
                    {
                        //管理员登陆
                        _optuser = new pubPagebase.optUser();
                        _optuser.isAdmin = true;
                        _optuser.username = username;
                        Session["usrifo"] = _optuser;
                        Rep["go"] = "/xingshen/lst.aspx";
                        Rep["ok"] = true;
                        return;
                    }
                    //尝试普通登陆
                    XingshenUser user = XingshenUser.GetModelByUserName(username);
                    if (user.id != 0 && user.pass == password)
                    {
                        _optuser = new pubPagebase.optUser();
                        _optuser.xingshenUser = user;
                        _optuser.isAdmin = false;
                        _optuser.username = username;
                        Session["usrifo"] = _optuser;
                        Rep["go"] = "/xingshen/accinfo.aspx?uid=" + user.uuid;
                        Rep["ok"] = true;
                    }
                }
            }
            finally
            {
                if ((bool)Rep["ok"] == false && Rep["msg"].ToString() == "")
                {
                    Rep["msg"] = "系统错误";
                }
                Response.CacheControl = "no-cache";
                Response.Write(Rep.ToString(Formatting.None));
                Response.End();
            }
        }
    }
}