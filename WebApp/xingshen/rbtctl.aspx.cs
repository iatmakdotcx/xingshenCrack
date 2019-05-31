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
    public partial class rbtctl : pubPagebase
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
                    int sectid;
                    string errMsg = svrHelper.Create_sects_info(user, out sectName, out sectid);
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
                }
                else if (Request["a"] == "qu")
                {
                    string errMsg = svrHelper.Create_sects_quit(user);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "sl")
                {
                    //从商会列表
                    JArray ja;
                    int shl;
                    string errMsg = svrHelper.Create_shop_list(user, out ja, out shl);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["data"] = ja;
                    Rep["ok"] = true;
                } else if (Request["a"] == "slb1")
                {
                    //从商会购买一个物品
                    int id = Mak.Common.MakRequest.GetInt("id", 0);
                    if (id > 0)
                    {
                        JObject jo;
                        string errMsg = svrHelper.Create_shop_buy(user, id, out jo);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            Rep["msg"] = errMsg;
                            return;
                        }
                        Rep["name"] = jo["item_name"].ToString();
                        Rep["ok"] = true;
                    }
                }
                else if (Request["a"] == "bo")
                {
                    JArray ja;
                    int shl;
                    string errMsg = svrHelper.Create_shop_list(user, out ja, out shl);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    foreach (var item in ja)
                    {
                        int price = int.Parse(item["price"].ToString());
                        if (price < 10)
                        {
                            int idone = int.Parse(item["id"].ToString());
                            JObject jo;
                            errMsg = svrHelper.Create_shop_buy(user, idone, out jo);
                            if (!string.IsNullOrEmpty(errMsg))
                            {
                                Rep["msg"] = errMsg;
                                return;
                            }
                            Rep["name"] = jo["item_name"].ToString();
                            Rep["ok"] = true;
                            return;
                        }
                        
                    }
                    
                    //JObject jo;
                    //errMsg = svrHelper.Create_shop_buy(user, idone, out jo);
                    //if (!string.IsNullOrEmpty(errMsg))
                    //{
                    //    Rep["msg"] = errMsg;
                    //    return;
                    //}
                    //Rep["name"] = jo["item_name"].ToString();
                    //Rep["ok"] = true;
                }
                else if (Request["a"] == "shl")
                {
                    int sl = Mak.Common.MakRequest.GetInt("sl", 0);
                    if (sl <= 0)
                    {
                        Rep["msg"] = "参数错误：sl";
                        return;
                    }
                    string errMsg = svrHelper.Create_addling(user, sl);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    JArray data;
                    int shl;
                    errMsg = svrHelper.Create_shop_list(user, out data, out shl);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["shl"] = shl;
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "qshl")
                {
                    JArray data;
                    int shl;
                    string errMsg = svrHelper.Create_shop_list(user, out data, out shl);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["shl"] = shl;
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "bossinfo")
                {
                    int level;
                    long HP;
                    string errMsg = svrHelper.Create_Bossinfo(user, out level, out HP);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["level"] = level;
                    Rep["hp"] = HP;
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "ad")//attack_damage
                {
                    int sl = Mak.Common.MakRequest.GetInt("sl", 0);
                    if (sl <= 0)
                    {
                        Rep["msg"] = "参数错误：sl";
                        return;
                    }
                    string errMsg = svrHelper.Create_attack_damage(user, sl);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["ok"] = true;
                }else if (Request["a"] == "sell")//卖东西
                {
                    int price = Mak.Common.MakRequest.GetInt("p",1);
                    string item_name = Mak.Common.MakRequest.GetString("in");
                    int itemType = Mak.Common.MakRequest.GetInt("it", 0);
                    int childType = Mak.Common.MakRequest.GetInt("ct", 0);
                    string errMsg = svrHelper.Create_shop_sell(user, price, item_name, itemType, childType);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "os")//自己卖的东西
                {
                    JArray data;
                    string errMsg = svrHelper.Create_Owner_shop(user, out data);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["data"] = data;
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "mji")//秘境信息
                {
                    JObject data;
                    string errMsg = svrHelper.Create_mi_jings_info(user, out data);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        Rep["msg"] = errMsg;
                        return;
                    }
                    Rep["data"] = data;
                    Rep["ok"] = true;
                }
                else if (Request["a"] == "mjs")//秘境攻击
                {
                    int sl = Mak.Common.MakRequest.GetInt("sl", 0);
                    string errMsg = svrHelper.Create_mi_jings_success(user, sl);
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