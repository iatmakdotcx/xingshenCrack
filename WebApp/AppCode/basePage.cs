using Mak.Common;
using Mak.Data;
using System;
using System.Web;

namespace telegramSvr
{
    public class basePage : System.Web.UI.Page
    {
        protected DbHelperItem dbH = DbHelper.getHelper("data");
        protected I18N L;
        protected UserInfo userInfo;
        public System.Web.UI.HtmlControls.HtmlForm mainform = null;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string lang = MakRequest.GetString("lang");
            if (!string.IsNullOrEmpty(lang))
            {
                Session["lang"] = lang;
            }
            L = new I18N((string)Session["lang"]);
            
            userInfo = (UserInfo)Session["usrifo"];
#if DEBUG
            if (userInfo == null)
            {
                userInfo = new UserInfo();
                userInfo.username = "9999";
                userInfo.nickname = "艹鸡用户";
            }
#endif
            if (userInfo == null)
            {
                //未登录
                if (Request.Path.ToLower().IndexOf("login") ==-1)
                {
                    //非登陆页
                    HttpContext.Current.Response.Redirect("/login.aspx");
                }
                return;
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {

        }
        protected string LI(string key, params object[] args)
        {
            return L.I(key, args);
        }

    }

}