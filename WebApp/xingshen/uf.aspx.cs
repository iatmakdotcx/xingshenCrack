using Mak.Common;
using Mak.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Model;

namespace telegramSvr.xingshen
{
    public partial class uf : System.Web.UI.Page
    {        
        protected JObject playerdata = new JObject();
        protected JArray warningdata = new JArray();

        XingshenUser user = null;
        XingshenUserData ud = null;

        protected string errMsg = "";

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
                    string ErrData = svrHelper.system_user_info(user,ref ud);
                    if (!string.IsNullOrEmpty(ErrData))
                    {
                        errMsg = "下载数据存档失败！" + ErrData;
                        return;
                    }
                    user.Update();
                    ud.Add();
                }
               
                playerdata = (JObject)JsonConvert.DeserializeObject(ud.data);

                warningdata = MakJsonHelper.DataTableToJsonArr_AllRow(XingshenUserDataWarning.GetWarningList(uid));

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

                if (Request["a"] == "dct")
                {
                    string dct;
                    string errMsg = svrHelper.GetUserLastDCTime(user, out dct);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["lastDCTime"] = dct;
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "rdt")
                {
                    ud = XingshenUserData.GetModel(uid);
                    //刷新存档
                    string ErrData = svrHelper.system_user_info(user, ref ud);
                    if (!string.IsNullOrEmpty(ErrData))
                    {
                        Rep["msg"] = "下载数据存档失败！" + ErrData;
                        return;
                    }
                    if (ud.id == 0)
                    {
                        ud.Add();
                    }
                    else
                        ud.Update();
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "delwarning")
                {
                    //删除警告信息
                    int wid = MakRequest.GetInt("wid", 0);
                    if (wid != 0 && XingshenUserDataWarning_BLL.Delete(uid, wid))
                    {
                        Rep["ok"] = true;
                    }
                    else
                    {
                        Rep["msg"] = "无效id";
                    }
                }
                else if  (Request["a"] == "downfirst")
                {
                    ud = XingshenUserData.GetModel(uid);                    
                    JObject jo = null;
                    try
                    {
                        jo = (JObject)JsonConvert.DeserializeObject(ud.data);
                    }
                    catch (Exception exx)
                    {
                        Rep["msg"] = exx.Message;
                        return;
                    }

                    string Data = Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                    jo["data"]["player_data"] = Data;

                    string errMsg = svrHelper.adjustmentData(jo, user);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    string data = jo.ToString(Formatting.None);
                    ud.data = data;
                    ud.Update();
                    
                    Rep["data"] = svrHelper.Create_first_login(data);
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "save")
                {
                    ud = XingshenUserData.GetModel(uid);
                    JObject jo = null;
                    try
                    {
                        jo = (JObject)JsonConvert.DeserializeObject(ud.data);
                    }
                    catch (Exception exx)
                    {
                        Rep["msg"] = exx.Message;
                        return;
                    }
                    string Data = Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                    jo["data"]["player_data"] = Data;
                    string errMsg = svrHelper.adjustmentData(jo, user);
                    string data = jo.ToString(Formatting.None);
                    ud.data = data;
                    ud.Update();
                    Rep["ok"] = true;
                }else if  (Request["a"] == "refreshwarn")
                {
                    //刷新警告信息
                    Rep["data"] = MakJsonHelper.DataTableToJsonArr_AllRow(XingshenUserDataWarning.GetWarningList(uid));
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
