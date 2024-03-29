﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Model;
using xingshenSvrHelper;

namespace telegramSvr.xingshen
{
    public partial class lst : pubPagebase
    {
        protected bool ShowBanned = false;
        protected bool ShowRobots = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_optuser.isAdmin)
            {
                Response.Redirect("/");
                return;
            }

            ShowBanned = Request["b"] == "1";
            ShowRobots = Request["r"] == "1";
            if (Request.HttpMethod == "POST") 
            {
                JObject Rep = new JObject();
                Rep["ok"] = false;
                Rep["msg"] = "";
                try
                {
                    if (Request["a"] == "new")
                    {
                        Rep = newUser(Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes)), Rep);
                    }else if (Request["a"] == "create")
                    {
                        Rep = CreatenewUser(Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes)), Rep);
                    }
                }
                finally
                {
                    Response.Write(Rep.ToString(Formatting.None));
                    Response.End();
                }
            }
        }
        public static JObject newUser(string JsonStr, JObject Rep)
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
            XingshenUser user = XingshenUser.GetModelByUserName(jo["user_name"].ToString());
            if (user.id > 0)
            {
                //已存在的用户
                Rep["ok"] = true;
                Rep["uid"] = user.uuid;
                return Rep;
            }
            user.user_name = jo["user_name"].ToString();
            if (string.IsNullOrEmpty(user.user_name))
            {
                Rep["msg"] = "参数错误:user_name";
                return Rep;
            }
            user.pass = jo["password"].ToString();
            if (string.IsNullOrEmpty(user.pass))
            {
                Rep["msg"] = "参数错误:password";
                return Rep;
            }
            user.isAndroid = jo["platform"].ToString() == "0";
            user.uuid = jo["uuid"].ToString();
            XingshenUserData ud = null;
            if (string.IsNullOrEmpty(user.uuid))
            {
                //没有UUid只能尝试登陆（登录有1小时只允许登录一次的限制
                string ErrData = svrHelper.first_login(user,ref ud);
                if (!string.IsNullOrEmpty(ErrData))
                {
                    Rep["msg"] = "登录失败！" + ErrData;
                    return Rep;
                }
                user.Add();
                Rep["ok"] = true;
                Rep["uid"] = user.uuid;
            }
            else
            {
                //如果有uuid可以通过system_user_info获取存档信息
                string ErrData = svrHelper.system_user_info(user, ref ud);
                if (!string.IsNullOrEmpty(ErrData))
                {
                    Rep["msg"] = "下载数据存档失败！" + ErrData;
                    return Rep;
                }
                user.Add();
                Rep["ok"] = true;
                Rep["uid"] = user.uuid;
            }
            if (ud != null)
            {
                if (ud.id > 0)
                {
                    ud.Update();
                }
                else
                {
                    ud.Add();
                }
            }
            return Rep;
        }
        public static JObject CreatenewUser(string JsonStr, JObject Rep)
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
            string user_name = jo["user_name"].ToString();
            string password = jo["password"].ToString();
            bool isAndroid = jo["platform"].ToString() == "0";
            string mac = jo["mac"] == null ? "" : jo["mac"].ToString();

            if (string.IsNullOrEmpty(user_name))
            {
                Rep["msg"] = "参数错误:user_name";
                return Rep;
            }
            if (string.IsNullOrEmpty(password))
            {
                Rep["msg"] = "参数错误:password";
                return Rep;
            }
            XingshenUser Newuser;
            string msg = svrHelper.Create_register(out Newuser, user_name, password, isAndroid, mac);
            if (string.IsNullOrEmpty(msg))
            {
                Rep["msg"] = "ok";
                Rep["ok"] = true;
            }
            else
            {
                Rep["msg"] = msg;
            }
            return Rep;
        }
    }
}