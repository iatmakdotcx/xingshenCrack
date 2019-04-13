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
    public partial class rbtctl : System.Web.UI.Page
    {
        XingshenUser user = null;
        XingshenUserData ud = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                Page_Load_POST();
            }
            else
            {
                string uid = Request["uid"];
                if (string.IsNullOrEmpty(uid))
                {
                    Response.Redirect("lst.aspx");
                    return;
                }
                user = XingshenUser.GetModel(uid);
                if (user.id == 0)
                {
                    Response.Redirect("lst.aspx");
                    return;
                }
                ud = XingshenUserData.GetModel(uid);
                if (ud.id == 0)
                {
                    string ErrData = svrHelper.system_user_info(user, ref ud);
                    if (!string.IsNullOrEmpty(ErrData))
                    {
                        return;
                    }
                    user.Update();
                    ud.Add();
                }
            }
        }
        private void Page_Load_POST()
        {
            JObject Rep = new JObject();
            Rep["ok"] = false;
            Rep["msg"] = "";
            try
            {
                string uid = Request["uid"];
                if (string.IsNullOrEmpty(uid))
                {
                    Rep["msg"] = "参数错误：uid";
                    return;
                }
                user = XingshenUser.GetModel(uid);
                if (user.id == 0)
                {
                    Rep["msg"] = "参数错误：uid";
                    return;
                }
                if (Request["a"] == "donate")
                {
                    string errMsg = svrHelper.Create_sects_donate(user, 10000);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["ok"] = true;
                }else if (Request["a"] == "qs")
                {
                    string sectName;
                    string errMsg = svrHelper.Create_sects_info(user, out sectName);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["name"] = sectName;
                    Rep["ok"] = true;
                }else if (Request["a"] == "sj")
                {
                    int sect_id = Mak.Common.MakRequest.GetInt("sid", 0);
                    if (sect_id <= 0)
                    {
                        Rep["msg"] = "参数错误：sid";
                        return;
                    }
                    string errMsg = svrHelper.Create_sects_join(user, sect_id);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["ok"] = true;
                }else if (Request["a"] == "qu")
                {
                    string errMsg = svrHelper.Create_sects_quit(user);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["ok"] = true;
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