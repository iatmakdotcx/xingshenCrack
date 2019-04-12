﻿using Mak.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Web.Model;

namespace xingshenSvrHelper
{
    public class svrHelper
    {
        public static string Andorid_VERSION = "128";
        public static string IOS_VERSION = "405";

        public static string Andorid_Svr = "https://47.99.61.85";
        public static string IOS_Svr = "https://sanguo.chengduicecloud.com";

        public static Random rd = new Random();

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

        public static string GenMac()
        {
            return string.Format("00:{0:X}:{1:X}:{2:X}:{3:X}:{4:X}"
               ,rd.Next(0, 255)
               ,rd.Next(0, 255)
               ,rd.Next(0, 255)
               ,rd.Next(0, 255)
               ,rd.Next(0, 255)
                );
        }
        public static string GenJmKey()
        {            
            return Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");
        }

        public static string Create_register(string user_name, string password, bool isAndroid = true, string mac = null)
        {
            string url = "/api/v2/users/register";
            if (string.IsNullOrEmpty(mac))
            {
                mac = GenMac();
            }
            JObject resJo = new JObject();
            if (isAndroid)
            {
                resJo["sg_version"] = Andorid_VERSION;
                url = Andorid_Svr + url;
            }
            else
            {
                resJo["sg_version"] = IOS_VERSION;
                url = IOS_Svr + url;
            }
            resJo["password"] = password;
            resJo["user_name"] = user_name;
            resJo["userdata"] = "{}";
            JObject player_data = new JObject();
            JObject playerDict = new JObject();
            playerDict["rolesArr"] = new JArray();
            playerDict["roleID"] = "1";
            playerDict["jmkey"] = GenJmKey();
            playerDict["lastLoginTime"] = null;
            playerDict["playerId"] = mac;
            playerDict["settingDict"] = new JObject();
            playerDict["secondPlay"] = null;
            playerDict["xsjLDExp"] = "";

            playerDict["mapSLDDict"] = new JObject();
            playerDict["zhaomuling"] = "";
            playerDict["shuye"] = "";
            playerDict["mjslNum"] = "";
            playerDict["scslLv"] = "";
            playerDict["hyJiFen"] = "";
            playerDict["juntuanExp"] = "";
            playerDict["ybao"] = "";
            playerDict["coin"] = "";
            playerDict["guajiMapId"] = null;
            playerDict["smTGLV"] = "";
            playerDict["xsjLv"] = "";
            playerDict["tsChengJiuGet"] = "";
            playerDict["lvChengJiuGet"] = "";
            playerDict["czJiFen"] = "";
            playerDict["shNum"] = "";
            playerDict["dalaoChengJiuGet"] = "";
            playerDict["zzybChengJiuGet"] = "";
            playerDict["zmlqsChengJiuGet"] = "";
            playerDict["zmlqs_gjNum"] = "";
            playerDict["cangkuArr"] = new JArray();
            playerDict["sldNum"] = "";
            playerDict["packageArr"] = new JArray();
            playerDict["tgChengJiuGet"] = "";
            playerDict["leftTL"] = "";
            playerDict["qdTime"] = "0";
            playerDict["xsjLQExp"] = "";
            playerDict["sltChengJiuGet"] = "";
            playerDict["battleRolesArr"] = new JArray();
            playerDict["syTGLV"] = "";
            playerDict["normalMapUnLock"] = "";
            playerDict["VipCJGet"] = "";
            playerDict["czCJGet"] = "";

            ulong exp = (ulong)rd.Next(300000000, 900000000);
            exp = exp * 10 + (ulong)rd.Next(1, 9);
            playerDict["juntuanExp"] = exp.ToString();

            #region battleRolesArr

            JArray brLst = new JArray();
            JObject role = new JObject();
            role["roleID"] = "1";
            role["ID"] = "00001";
            role["addDict"] = new JObject();
            brLst.Add(role);
            playerDict["battleRolesArr"] = brLst;

            #endregion

            #region items



            #endregion
            player_data["playerDict"] = playerDict;
            resJo["player_data"] = player_data.ToString(Formatting.None);
            JObject player_zhong_yao = new JObject();
            player_zhong_yao["curGameCenterID"] = mac;
            player_zhong_yao["createGCID"] = mac;
            player_zhong_yao["shiliantaLevel"] = "";
            player_zhong_yao["playerJFXHKey"] = "0";
            player_zhong_yao["playerYbKey"] = "";
            player_zhong_yao["playerGJBaoShiDai"] = "0";
            player_zhong_yao["isDLSave"] = "0";
            player_zhong_yao["playerLevel"] = "1";
            player_zhong_yao["playerBuyYbKey"] = "";
            player_zhong_yao["playerYBXHKey"] = "";
            player_zhong_yao["playerBaoShiDai"] = "";
            player_zhong_yao["lastDCTime"] = "";
            player_zhong_yao["playerPaiZi"] = "";
            player_zhong_yao["createTime"] = "0";
            player_zhong_yao["shenyuanLevel"] = "";
            resJo["player_zhong_yao"] = player_zhong_yao.ToString(Formatting.None);

            string errMsg;
            string repdata = PostData(url, resJo.ToString(Formatting.None), out errMsg);
            if (!string.IsNullOrEmpty(repdata))
            {
                JObject jo = null;
                try
                {
                    jo = (JObject)JsonConvert.DeserializeObject(repdata);
                    if (jo["code"].ToString() == "0" && jo["type"].ToString() == "4")
                    {
                        string uuid = jo["data"]["uuid"].ToString();
                        XingshenUser user = XingshenUser.GetModelByUserName(user_name);
                        user.user_name = user_name;
                        user.pass = password;
                        user.isAndroid = isAndroid;
                        user.uuid = uuid;
                        if (user.id == 0)
                        {
                            user.Add();
                        }
                        else
                        {
                            user.Update();
                        }
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
                if (savedata.id == 0)
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
            catch (Exception ex)
            {
                resJo["message"] = ex.Message;
            }
            return Create_first_login(resJo, ref headers);
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
            player_data["playerDict"]["user_name"] = user.user_name;
            player_data["playerDict"]["password"] = user.pass;
            if (player_data["playerDict"]["playName"] == null)
            {
                player_data["playerDict"]["playName"] = player_data["playerDict"]["user_name"];
            }
            player_data["playerDict"]["uuid"] = user.uuid;
            player_data["playerDict"]["token"] = user.token;

            if (player_data["playerDict"]["VipCJGet"] == null)
                player_data["playerDict"]["VipCJGet"] = "";
            if (player_data["playerDict"]["cangkuArr"] == null)
                player_data["playerDict"]["cangkuArr"] = new JArray();
            if (player_data["playerDict"]["cdTime"] == null)
                player_data["playerDict"]["cdTime"] = null;
            if (player_data["playerDict"]["cjbzArr"] == null)
                player_data["playerDict"]["cjbzArr"] = new JArray();
            
            if (player_data["playerDict"]["coin"] == null)
                player_data["playerDict"]["coin"] = "600";
            if (player_data["playerDict"]["createGCID"] == null)
                player_data["playerDict"]["createGCID"] = player_data["playerDict"]["playerId"];
            if (player_data["playerDict"]["czCJGet"] == null)
                player_data["playerDict"]["czCJGet"] = "";
            if (player_data["playerDict"]["czJiFen"] == null)
                player_data["playerDict"]["czJiFen"] = "";
            if (player_data["playerDict"]["dalaoChengJiuGet"] == null)
                player_data["playerDict"]["dalaoChengJiuGet"] = "";
            if (player_data["playerDict"]["equipDown"] == null)
                player_data["playerDict"]["equipDown"] = null;
            if (player_data["playerDict"]["firstPlayTime"] == null)
                player_data["playerDict"]["firstPlayTime"] = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            if (player_data["playerDict"]["guajiMapId"] == null)
                player_data["playerDict"]["guajiMapId"] = null;
            if (player_data["playerDict"]["hdTZDict"] == null)
                player_data["playerDict"]["hdTZDict"] = new JObject();
            if (player_data["playerDict"]["hsDict"] == null)
                player_data["playerDict"]["hsDict"] = new JObject();
            if (player_data["playerDict"]["huiyuanGetTime"] == null)
                player_data["playerDict"]["huiyuanGetTime"] = null;
            if (player_data["playerDict"]["hyJiFen"] == null)
                player_data["playerDict"]["hyJiFen"] = "";
            if (player_data["playerDict"]["isDLSave"] == null)
                player_data["playerDict"]["isDLSave"] = "0";
            if (player_data["playerDict"]["kssmLV"] == null)
                player_data["playerDict"]["kssmLV"] = "0";
            if (player_data["playerDict"]["lastLoginTime"] == null || player_data["playerDict"]["lastLoginTime"].Type == JTokenType.Null)
                player_data["playerDict"]["lastLoginTime"] = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            if (player_data["playerDict"]["ldDict"] == null)
                player_data["playerDict"]["ldDict"] = new JObject();
            if (player_data["playerDict"]["leftTL"] == null)
                player_data["playerDict"]["leftTL"] = "";

            if (player_data["playerDict"]["leftWithBoss"] == null)
                player_data["playerDict"]["leftWithBoss"] = "";
            if (player_data["playerDict"]["lqDict"] == null)
                player_data["playerDict"]["lqDict"] = new JObject();


            if (player_data["playerDict"]["lvChengJiuGet"] == null)
                player_data["playerDict"]["lvChengJiuGet"] = "";
            if (player_data["playerDict"]["mjslNum"] == null)
                player_data["playerDict"]["mjslNum"] = "";
            if (player_data["playerDict"]["mzslJCDict"] == null)
                player_data["playerDict"]["mzslJCDict"] = new JObject();
            if (player_data["playerDict"]["normalMapUnLock"] == null)
                player_data["playerDict"]["normalMapUnLock"] = "";
            if (player_data["playerDict"]["packageArr"] == null)
                player_data["playerDict"]["packageArr"] = new JArray();
            if (player_data["playerDict"]["mapSLDDict"] == null)
                player_data["playerDict"]["mapSLDDict"] = new JObject();
            if (player_data["playerDict"]["scslLv"] == null)
                player_data["playerDict"]["scslLv"] = "";
            if (player_data["playerDict"]["secondPlay"] == null || player_data["playerDict"]["secondPlay"].Type == JTokenType.Null)
                player_data["playerDict"]["secondPlay"] = "1";
            if (player_data["playerDict"]["shNum"] == null)
                player_data["playerDict"]["shNum"] = "";
            if (player_data["playerDict"]["shuye"] == null)
                player_data["playerDict"]["shuye"] = null;
            if (player_data["playerDict"]["sjUseNum"] == null)
                player_data["playerDict"]["sjUseNum"] = "";
            if (player_data["playerDict"]["sldNum"] == null)
                player_data["playerDict"]["sldNum"] = "";
            if (player_data["playerDict"]["sltChengJiuGet"] == null)
                player_data["playerDict"]["sltChengJiuGet"] = "";
            if (player_data["playerDict"]["smTGLV"] == null)
                player_data["playerDict"]["smTGLV"] = "";
            if (player_data["playerDict"]["syTGLV"] == null)
                player_data["playerDict"]["syTGLV"] = "";
            if (player_data["playerDict"]["syXH"] == null)
                player_data["playerDict"]["syXH"] = null;
            if (player_data["playerDict"]["tgChengJiuGet"] == null)
                player_data["playerDict"]["tgChengJiuGet"] = "";
            if (player_data["playerDict"]["tsChengJiuGet"] == null)
                player_data["playerDict"]["tsChengJiuGet"] = "";
            if (player_data["playerDict"]["tsJCDict"] == null)
                player_data["playerDict"]["tsJCDict"] = new JObject();
            if (player_data["playerDict"]["tzlXH"] == null)
                player_data["playerDict"]["tzlXH"] = null;
            if (player_data["playerDict"]["xsjLDExp"] == null)
                player_data["playerDict"]["xsjLDExp"] = "";
            if (player_data["playerDict"]["xsjLQExp"] == null)
                player_data["playerDict"]["xsjLQExp"] = "";
            if (player_data["playerDict"]["xsjLv"] == null)
                player_data["playerDict"]["xsjLv"] = "";
            if (player_data["playerDict"]["ybao"] == null || player_data["playerDict"]["ybao"].Type == JTokenType.Null)
                player_data["playerDict"]["ybao"] = "0";
            if (player_data["playerDict"]["ybaoXH"] == null)
                player_data["playerDict"]["ybaoXH"] = null;
            if (player_data["playerDict"]["ykLQNum"] == null)
                player_data["playerDict"]["ykLQNum"] = null;
            if (player_data["playerDict"]["yuekaNum"] == null)
                player_data["playerDict"]["yuekaNum"] = null;
            if (player_data["playerDict"]["zdglDict"] == null)
                player_data["playerDict"]["zdglDict"] = new JObject();
            if (player_data["playerDict"]["zfDict"] == null)
                player_data["playerDict"]["zfDict"] = new JObject();
            if (player_data["playerDict"]["zfKG"] == null)
                player_data["playerDict"]["zfKG"] = new JObject();
            if (player_data["playerDict"]["zhaomuling"] == null)
                player_data["playerDict"]["zhaomuling"] = "";
            if (player_data["playerDict"]["zkLQNum"] == null)
                player_data["playerDict"]["zkLQNum"] = null;
            if (player_data["playerDict"]["zkaNum"] == null)
                player_data["playerDict"]["zkaNum"] = null;
            if (player_data["playerDict"]["zmlqsChengJiuGet"] == null)
                player_data["playerDict"]["zmlqsChengJiuGet"] = "";
            if (player_data["playerDict"]["zmlqs_gjNum"] == null)
                player_data["playerDict"]["zmlqs_gjNum"] = "";
            if (player_data["playerDict"]["zzybChengJiuGet"] == null)
                player_data["playerDict"]["zzybChengJiuGet"] = "";

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
            if (player_zhong_yao["iCloudName"] == null)
                player_zhong_yao["iCloudName"] = user.user_name;
            
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
        public static string first_login(XingshenUser user, ref XingshenUserData ud)
        {
            if (ud == null)
            {
                ud = XingshenUserData.GetModel(user.uuid);
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

        public static string system_user_info(XingshenUser user, ref XingshenUserData ud)
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
                        if (player_data["playerDict"]["token"]!=null)
                        {
                            user.token = player_data["playerDict"]["token"].ToString();
                        }

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
        public static string Create_save_user(XingshenUser user, XingshenUserData ud)
        {
            if (ud == null)
            {
                ud = XingshenUserData.GetModel(user.uuid);
            }
            if (ud.id == 0 || string.IsNullOrEmpty(ud.data))
            {
                return "未找到用户存档！";
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(ud.data);
            string errMsg = adjustmentData(jo, user);
            if (string.IsNullOrEmpty(errMsg))
            {
                jo["net_id"] = user.net_id + 1;
                string url = "/api/v2/users/save_user";
                if (user.isAndroid)
                {
                    jo["sg_version"] = Andorid_VERSION;
                    url = Andorid_Svr + url;
                }
                else
                {
                    jo["sg_version"] = IOS_VERSION;
                    url = IOS_Svr + url;
                }
                jo["userdata"] = "{}";
                jo["uuid"] = user.uuid;
                jo["token"] = user.token;
                jo["player_data"] = jo["data"]["player_data"];
                jo["player_zhong_yao"] = jo["data"]["player_zhong_yao"];
                jo.Remove("type");
                jo.Remove("code");
                jo.Remove("data");
                string repdata = PostData(url, jo.ToString(Formatting.None), out errMsg);
                if (!string.IsNullOrEmpty(repdata))
                {
                    JObject Repjo = null;
                    try
                    {
                        Repjo = (JObject)JsonConvert.DeserializeObject(repdata);
                        if (Repjo["code"].ToString() == "0" && Repjo["type"].ToString() == "2")
                        {
                            return "";
                        }
                        else if (Repjo["message"] != null)
                        {
                            return Repjo["message"].ToString();
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
            }
            return errMsg;
        }

        /// <summary>
        /// 设置下品灵石
        /// </summary>
        /// <param name="jo"></param>
        /// <param name="cnt"></param>
        private static void resetLingshi(JObject jo,int cnt)
        {
            JObject player_data = (JObject)JsonConvert.DeserializeObject(jo["data"]["player_data"].ToString());
            JArray items = (JArray)player_data["playerDict"]["packageArr"];
            if (items.Count == 0)
            {
                JObject it = new JObject();
                it["itemType"] = "8";
                it["childType"] = "34";
                it["num"] = cnt.ToString();
                it["itemID"] = "1";
                items.Add(it);
            }
            else
            {
                bool found = false;
                foreach (JObject it in items)
                {
                    if (it["itemType"].ToString() == "8" && it["childType"].ToString() == "34")
                    {
                        it["num"] = cnt.ToString();
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    JObject it = (JObject)items[0];
                    it["itemType"] = "8";
                    it["childType"] = "34";
                    it["num"] = cnt.ToString();
                    items[0] = it;
                }
            }
            player_data["playerDict"]["packageArr"] = items;
            jo["data"]["player_data"] = player_data.ToString(Formatting.None);
        }
        public static string Create_sects_donate(XingshenUser user, int cnt = 10000)
        {
            XingshenUserData ud = XingshenUserData.GetModel(user.uuid);
            if (ud.id == 0 || string.IsNullOrEmpty(ud.data))
            {
                return "未找到用户存档！";
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(ud.data);
            string errMsg = adjustmentData(jo, user);
            if (string.IsNullOrEmpty(errMsg))
            {
                resetLingshi(jo, 11000);
                jo["net_id"] = user.net_id + 1;
                string url = "/api/v2/sects/donate";
                if (user.isAndroid)
                {
                    jo["sg_version"] = Andorid_VERSION;
                    url = Andorid_Svr + url;
                }
                else
                {
                    jo["sg_version"] = IOS_VERSION;
                    url = IOS_Svr + url;
                }
                jo["net_id"] = user.net_id + 1;
                jo["count"] = cnt.ToString();
                jo["userdata"] = "{}";
                jo["uuid"] = user.uuid;
                jo["token"] = user.token;
                jo["player_data"] = jo["data"]["player_data"];
                jo["player_zhong_yao"] = jo["data"]["player_zhong_yao"];
                jo.Remove("type");
                jo.Remove("code");
                jo.Remove("data");
                string repdata = PostData(url, jo.ToString(Formatting.None), out errMsg);
                if (!string.IsNullOrEmpty(repdata))
                {
                    JObject Repjo = null;
                    try
                    {
                        Repjo = (JObject)JsonConvert.DeserializeObject(repdata);
                        if (Repjo["code"].ToString() == "0" && Repjo["type"].ToString() == "34")
                        {
                            XingshenSect sect = XingshenSect.GetModel(user.uuid);
                            sect.lastdonate = DateTime.Now;                            
                            if (sect.id == 0)
                            {
                                sect.uuid = user.uuid;
                                sect.Add();
                            }
                            else
                            {
                                sect.Update();
                            }
                            return "";
                        }
                        else if (Repjo["message"] != null)
                        {
                            return Repjo["message"].ToString();
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