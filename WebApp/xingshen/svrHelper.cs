﻿using Mak.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Web.Model;

namespace telegramSvr.xingshen
{
    public class svrHelper
    {
        public static string Andorid_VERSION = "127";
        public static string IOS_VERSION = "405";

        public static string Andorid_Svr = "https://47.99.61.85";
        public static string IOS_Svr = "https://sanguo.chengduicecloud.com";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dct">Server-Time</param>
        /// <param name="data">postData</param>
        /// <returns></returns>
        public static string SignData(string dct, string data)
        {
            string k1 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dct + "^" + dct + "!" + dct, "MD5").ToLower();
            string k2 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("zRBcyL2fy[ZsL7XJP$AIDJE*2DFF=#Dxjef2@LDLF", "MD5").ToLower();
            k2 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dct + "#" + k2, "MD5").ToLower();
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(k1 + data.Trim() + k2, "MD5").ToLower();
        }

        public static string Create_first_login(string user_name, string password, ref Dictionary<string, string> headers)
        {
            JObject resJo = new JObject();
            try
            {
                resJo["code"] = 1;
                resJo["type"] = 7;
                XingshenUser user = XingshenUser.GetModelByUserName(user_name);
                if (user.id == 0)
                {
                    resJo["message"] = "代理系统中不存在当前用户！请到系统中添加！";
                    return Create_first_login(resJo, ref headers);
                }
                if (user.pass != password)
                {
                    resJo["message"] = "密码错误！";
                    return Create_first_login(resJo, ref headers);
                }
                XingshenUserData savedata = XingshenUserData.GetModel(user.uuid);
                if (savedata.id==0)
                {
                    resJo["message"] = "未找到用户存在，请在代理系统中刷新后重试！";
                    return Create_first_login(resJo, ref headers);
                }
                JObject DataJO = (JObject)JsonConvert.DeserializeObject(savedata.data);
                string errMsg = adjustmentData(DataJO, user);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    resJo["message"] = errMsg;
                    return Create_first_login(resJo, ref headers);
                }
                DataJO["code"] = 0;
                DataJO["type"] = 8;
                return Create_first_login(DataJO, ref headers);
            }
            catch(Exception ex)
            {
                resJo["message"] = ex.Message;               
            }
            return Create_first_login(resJo,ref headers);
        }
        public static string Create_first_login(JObject DataJO, ref Dictionary<string, string> headers)
        {
            string dataStr = DataJO.ToString(Formatting.None);
            headers = Create_first_login_getHeader(DataJO.ToString(Formatting.None));
            return dataStr;
        }
        public static string Create_first_login(string data)
        {
            Dictionary<string, string> headers = Create_first_login_getHeader(data);
            StringBuilder sbdata = new StringBuilder();
            sbdata.AppendLine("HTTP/1.1 200 OK");
            foreach (var item in headers)
            {
                sbdata.AppendLine(item.Key + ": " + item.Value);
            }
            sbdata.AppendLine("");
            sbdata.Append(data);
            return sbdata.ToString();
        }
        public static Dictionary<string, string> Create_first_login_getHeader(string data)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            string dct = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            headers["Content-Type"] = "application/json; charset=utf-8";
            headers["Connection"] = "close";
            headers["Cache-Control"] = "max-age=0, private, must-revalidate";
            headers["Server-Time"] = dct;
            headers["Sign"] = SignData(dct, data);
            return headers;
        }
        public static string adjustmentData(JObject jo, XingshenUser user)
        {
            string dct = "";
            string errMsg = svrHelper.GetUserLastDCTime(user, out dct);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }
            JObject player_data = (JObject)JsonConvert.DeserializeObject(jo["data"]["player_data"].ToString());
            player_data["playerDict"]["lastDCTime"] = dct;
            jo["data"]["player_data"] = player_data.ToString(Formatting.None);
            JObject player_zhong_yao = (JObject)JsonConvert.DeserializeObject(jo["data"]["player_zhong_yao"].ToString());
            player_zhong_yao["lastDCTime"] = dct;

            player_zhong_yao["shiliantaLevel"] = player_data["playerDict"]["scslLv"];
            player_zhong_yao["shenyuanLevel"] = player_data["playerDict"]["syTGLV"];
            player_zhong_yao["playerBaoShiDai"] = player_data["playerDict"]["czJiFen"];
            player_zhong_yao["playerYBXHKey"] = player_data["playerDict"]["ybaoXH"];
            player_zhong_yao["playerBuyYbKey"] = player_data["playerDict"]["normalMapUnLock"];
            player_zhong_yao["playerYbKey"] = player_data["playerDict"]["ybao"];
            player_zhong_yao["createTime"] = player_data["playerDict"]["firstPlayTime"];
            string playerGJBaoShiDai = "";//挑战令

            foreach (var item in (JArray)player_data["playerDict"]["packageArr"])
            {
                if (item["itemType"].ToString() == "8" && item["childType"].ToString() == "33")
                {
                    playerGJBaoShiDai = (Utils.StrToInt(playerGJBaoShiDai, 0) + Utils.StrToInt(item["num"].ToString(), 0)).ToString();
                }
            }
            foreach (var item in (JArray)player_data["playerDict"]["cangkuArr"])
            {
                if (item["itemType"].ToString() == "8" && item["childType"].ToString() == "33")
                {
                    playerGJBaoShiDai = (Utils.StrToInt(playerGJBaoShiDai, 0) + Utils.StrToInt(item["num"].ToString(), 0)).ToString();
                }
            }
            player_zhong_yao["playerGJBaoShiDai"] = playerGJBaoShiDai;
            player_zhong_yao["playerPaiZi"] = player_data["playerDict"]["normalMapUnLock"];
            jo["data"]["player_zhong_yao"] = player_zhong_yao.ToString(Formatting.None);

            return "";
        }
        public static string first_login(XingshenUser user,ref XingshenUserData ud)
        {
            if (ud == null)
            {
                ud = new XingshenUserData();
            }
            JObject jO = new JObject();
            string url = "/api/v1/users/first_login";
            if (user.isAndroid)
            {
                jO["sg_version"] = Andorid_VERSION;
                url = Andorid_Svr + url;
            }
            else
            {
                jO["sg_version"] = IOS_VERSION;
                url = IOS_Svr + url;
            }
            jO["user_name"] = user.user_name;
            jO["password"] = user.pass;
            string errMsg;
            string repdata = PostData(url, jO.ToString(Formatting.None), out errMsg);
            if (!string.IsNullOrEmpty(repdata))
            {
                JObject jo = null;
                try
                {
                    jo = (JObject)JsonConvert.DeserializeObject(repdata);
                    if (jo["code"].ToString() == "0" && jo["type"].ToString() == "8")
                    {
                        JObject player_data = (JObject)JsonConvert.DeserializeObject(jo["data"]["player_data"].ToString());
                        user.uuid = jo["data"]["uuid"].ToString();
                        user.token = player_data["playerDict"]["token"].ToString();

                        ud.uuid = user.uuid;
                        ud.data = jo.ToString(Formatting.None);
                        return "";
                    }
                    else if (jo["message"] != null)
                    {
                        return jo["message"].ToString();
                    }
                    else
                    {
                        return repdata;
                    }
                }
                catch (Exception exx)
                {
                    return exx.Message;
                }
            }
            return errMsg;            
        }

        public static string system_user_info(XingshenUser user,ref XingshenUserData ud)
        {
            if (ud == null)
            {
                ud = new XingshenUserData();
            }
            JObject jO = new JObject();
            string url = "/api/v1/users/system_user_info";
            if (user.isAndroid)
            {
                jO["sg_version"] = Andorid_VERSION;
                url = Andorid_Svr + url;
            }
            else
            {
                jO["sg_version"] = IOS_VERSION;
                url = IOS_Svr + url;
            }
            jO["user_name"] = user.user_name;
            jO["password"] = user.pass;
            jO["uuid"] = user.uuid;
            string errMsg;
            string repdata = PostData(url, jO.ToString(Formatting.None), out errMsg);
            if (!string.IsNullOrEmpty(repdata))
            {

                JObject jo = null;
                try
                {
                    jo = (JObject)JsonConvert.DeserializeObject(repdata);
                    if (jo["code"].ToString() == "0" && jo["type"].ToString() == "7")
                    {
                        jo["type"] = 8;
                        jo["data"]["userdata"] = "{}";

                        JObject player_data = (JObject)JsonConvert.DeserializeObject(jo["data"]["player_data"].ToString());
                        user.token = player_data["playerDict"]["token"].ToString();

                        ud.uuid = user.uuid;
                        ud.data = jo.ToString(Formatting.None);
                        return "";
                    }
                    else if (jo["message"] != null)
                    {
                        return jo["message"].ToString();
                    }
                    else
                    {
                        return repdata;
                    }
                }
                catch (Exception exx)
                {
                    return exx.Message;
                }
            }
            return errMsg;
        }

        public static string GetUserLastDCTime(XingshenUser user, out string dct)
        {
            dct = "";
            JObject jO = new JObject();
            string url = "/api/v2/users/login";
            if (user.isAndroid)
            {
                jO["sg_version"] = Andorid_VERSION;
                url = Andorid_Svr + url;
            }
            else
            {
                jO["sg_version"] = IOS_VERSION;
                url = IOS_Svr + url;
            }
            jO["user_name"] = user.user_name;
            jO["password"] = user.pass;
            jO["uuid"] = user.uuid;
            string errMsg;
            string repdata = PostData(url, jO.ToString(Formatting.None), out errMsg);
            if (!string.IsNullOrEmpty(repdata))
            {
                JObject jo = null;
                try
                {
                    jo = (JObject)JsonConvert.DeserializeObject(repdata);
                    if (jo["code"].ToString() == "0" && jo["type"].ToString() == "3")
                    {
                        dct = jo["data"]["lastDCTime"].ToString();
                        user.token = jo["data"]["token"].ToString();
                        user.net_id = Utils.StrToInt(jo["data"]["net_id"].ToString(), 1);
                        return "";
                    }
                    else if (jo["message"] != null)
                    {
                        return jo["message"].ToString();
                    }
                    else
                    {
                        return repdata;
                    }
                }
                catch (Exception exx)
                {
                    return exx.Message;
                }
            }
            return errMsg;
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
                request.Headers["Server-Time"] = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                request.Headers["Sign"] = SignData(request.Headers["Server-Time"], data);

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

    }
}