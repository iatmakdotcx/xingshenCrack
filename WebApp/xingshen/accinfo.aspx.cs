﻿using Mak.Common;
using Mak.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Model;
using xingshenSvrHelper;

namespace telegramSvr.xingshen
{
    public partial class accinfo : pubPagebase
    {        
        protected JObject playerdata = new JObject();
        protected JArray warningdata = new JArray();

        protected XingshenUser user = null;
        XingshenUserData ud = null;

        protected string errMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_optuser.isAdmin)
            {
                //普通用户只能看自己的
                if (_optuser.xingshenUser == null || _optuser.xingshenUser.uuid != Request["uid"])
                {
                    Response.Redirect("/");
                    return;
                }               
            }

            if (Request.HttpMethod == "POST")
            {
                Page_Load_POST();
            }
            else
            {                
                string uid = Request["uid"];
                if (string.IsNullOrEmpty(uid))
                {
                    if (_optuser.isAdmin)
                    {
                        Response.Redirect("lst.aspx");
                    }
                    else
                    {
                        Response.Redirect("/");
                    }
                    return;
                }
                user = XingshenUser.GetModel(uid);
                if (user.id == 0)
                {
                    if (_optuser.isAdmin)
                    {
                        Response.Redirect("lst.aspx");
                    }
                    else
                    {
                        Response.Redirect("/");
                    }
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
                if (!_optuser.isAdmin && _optuser.xingshenUser.ExpiryDate < DateTime.Now && Request["a"] != "del")
                {
                    Rep["msg"] = "操作时限已过 ";
                    return;
                }
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
                    user = XingshenUser.GetModel(uid);
                    if (user.id > 0)
                    {
                        //检查是否被封
                        string dct;
                        svrHelper.GetUserLastDCTime(user, out dct);

                        user.isHold = false;
                        user.Update();
                    }
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
                else if (Request["a"] == "downfirst")
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

                    Rep["data"] = svrHelper.Create_first_login(user, data);
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
                    user = XingshenUser.GetModel(uid);
                    if (user.id > 0)
                    {
                        user.isHold = true;
                        user.Update();
                    }
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "refreshwarn")
                {
                    //刷新警告信息
                    Rep["data"] = MakJsonHelper.DataTableToJsonArr_AllRow(XingshenUserDataWarning.GetWarningList(uid));
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "sign")
                {
                    string Data = Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                    string dct = MakRequest.GetString("ts");
                    if (string.IsNullOrEmpty(dct))
                    {
                        ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                    }
                    Rep["ServerTime"] = dct;
                    Rep["Sign"] = svrHelper.SignData(user, dct, Data);                    
                    Rep["ok"] = true;
                    Rep["Sign2"] = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Data + "QAbxK1exZYrK6WIO" + dct, "MD5").ToLower();
                }
                else if (Request["a"] == "upload")
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
                    ud.data = jo.ToString(Formatting.None);
                    ud.Update();
                    string errMsg = svrHelper.Create_save_user(user, ud);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                    }
                    else
                        Rep["ok"] = true;
                }
                else if (Request["a"] == "aed")
                {
                    if (!_optuser.isAdmin)
                    {
                        return;
                    }
                    string Data = Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                    JObject jo = null;
                    try
                    {
                        jo = (JObject)JsonConvert.DeserializeObject(Data);
                    }
                    catch (Exception exx)
                    {
                        Rep["msg"] = exx.Message;
                        return;
                    }
                    if (jo["timeval"] == null || jo["units"] == null)
                    {
                        Rep["msg"] = "参数错误";
                        return;
                    }
                    int timeval = Utils.StrToInt(jo["timeval"].ToString(), 0);
                    int units = Utils.StrToInt(jo["units"].ToString(), 0);
                    DateTime dtt;
                    switch (units)
                    {
                        case 0: dtt = DateTime.Now.AddMinutes(timeval); break;
                        case 1: dtt = DateTime.Now.AddHours(timeval); break;
                        case 2: dtt = DateTime.Now.AddDays(timeval); break;
                        default: dtt = DateTime.Now; break;
                    }
                    user.ExpiryDate = dtt;
                    user.Update();
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "del")
                {
                    //从系统中删除账号
                    user.Delete();
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "login")
                {
                    //强制登录一波，刷新token
                    string ErrData = svrHelper.first_login(user, ref ud);
                    if (!string.IsNullOrEmpty(ErrData))
                    {
                        Rep["msg"] = "登录失败！" + ErrData;
                        return;
                    }
                    user.Update();
                    Rep["ok"] = true;
                    Rep["uid"] = user.uuid;
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
