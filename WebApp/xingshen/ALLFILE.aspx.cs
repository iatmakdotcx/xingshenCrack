using Mak.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace telegramSvr.xingshen
{
    public partial class ALLFILE : System.Web.UI.Page
    {
        protected JObject ALLFILE_Item = new JObject();
        protected void Page_Load(object sender, EventArgs e)
        {
            priseALLFILE(Utils.GetMapPath("~/xingshen/ALLFILE"));
            Response.CacheControl = "Public";
            Response.Headers["Pragma"] = "Public";
            Response.ExpiresAbsolute = DateTime.MaxValue;
            Response.Write(" var ALLFILE_Item =");
            Response.Write(ALLFILE_Item.ToString(Newtonsoft.Json.Formatting.None));
            Response.End();
        }
        private void priseALLFILE(string path)
        {
            JObject Jo;
            using (StreamReader sr = new StreamReader(path))
            {
                Jo = (JObject)JsonConvert.DeserializeObject(sr.ReadToEnd());
            }
            foreach (var alist in Jo)
            {
                if (alist.Key.StartsWith("ITEMFILE"))
                {
                    ALLFILE_Item[alist.Key] = alist.Value;
                }
                else if (alist.Key == "KEYSTRNAME")
                {
                    ALLFILE_Item[alist.Key] = alist.Value;
                }
                else if (alist.Key == "ROLEFILE")
                {
                    ALLFILE_Item[alist.Key] = alist.Value;
                }
                else if (alist.Key == "EXPLEVEL")
                {
                    ALLFILE_Item[alist.Key] = alist.Value;
                }
            }

            /*
            foreach (var alist in Jo)
            {
                if (alist.Key.StartsWith("ITEMFILE"))
                {
                    var Itemtype = alist.Key.Substring("ITEMFILE".Length);
                    foreach (var item in (JObject)alist.Value)
                    {
                        var childType = item.Key;
                        var name = item.Value["name"].ToString();
                        var miaoshu = item.Value["miaoshu"] == null ? "" : item.Value["miaoshu"].ToString();

                        //string sql = "select count(1) from items where Itemtype=@a and childType=@b";
                        //int cnt = (int)dbh.ExecuteScalar(System.Data.CommandType.Text, sql, dbh.MakeInParam("@a", Itemtype), dbh.MakeInParam("@b", childType));
                        //if (cnt == 0)
                        //{
                        //    sql = "insert items(itemtype,childtype,name,miaoshu)values(@a,@b,@c,@d)";
                        //    dbh.ExecuteNonQuery(System.Data.CommandType.Text, sql,
                        //        dbh.MakeInParam("@a", Itemtype),
                        //        dbh.MakeInParam("@b", childType),
                        //        dbh.MakeInParam("@c", name),
                        //        dbh.MakeInParam("@d", miaoshu)
                        //        );
                        //}

                        if (XingshenItems.Counts(Itemtype, childType)==0)
                        {
                            XingshenItems xingshen = new XingshenItems();                      
                            xingshen.itemtype = Itemtype;
                            xingshen.childtype = childType;
                            xingshen.name = name;
                            xingshen.miaoshu = miaoshu;
                            xingshen.Add();
                        }
                    }
                }
            }*/
        }
    }
}