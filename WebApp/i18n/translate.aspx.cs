using Mak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace telegramSvr.i18n
{
    public partial class translate : basePage
    {
        protected int pageidx = 1;
        protected int datalimit = 20;
        protected int RecordCount = 0;
        protected int AllRecordCount = 0;
        protected string lang_src = "";
        protected string lang_dst = "";
        protected bool includeSys = false;
        protected string baseurl = "";
        protected int filterstar = 0;
        protected string filterkw = "";

        protected int[] s1_5 = new int[5];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                saveSubmit();
            }else
            {
                loadDefData2();
            }
        }

        private void loadDefData2()
        {
            pageidx = MakRequest.GetInt("page", 1);
            datalimit = MakRequest.GetInt("limit", 20);
            lang_src = MakRequest.GetString("lang1");
            lang_dst = MakRequest.GetString("lang2");
            includeSys = MakRequest.GetInt("isys", 0) == 1;

            filterstar = Utils.StrToInt(MakRequest.GetString("filter_s"), 0);
            filterkw = MakRequest.GetString("filter_k");

            if (!I18N.alllang.Contains(lang_dst))
            {
                lang_dst = "en-us";
            }
            I18N idst = new I18N(lang_dst);
            //过滤
            List<I18N.LangItem> filteredData = new List<I18N.LangItem>();             
            foreach (var item in idst.lanStrs)
            {
                I18N.LangItem li = item.Value;
                bool isSyskey = I18N.sysKey.Contains(li.Key);
                if (includeSys || !isSyskey)
                {
                    AllRecordCount++;
                    s1_5[li.Score - 1]++;
                    if (filterstar == 0 || filterstar == li.Score)
                    {
                        if (string.IsNullOrEmpty(filterkw) || li.Key.IndexOf(filterkw) > -1 || li.Text.IndexOf(filterkw) > -1)
                        {
                            filteredData.Add(li);
                        }
                    }
                }
            }
            //分页
            List<lstItemModel> ll = new List<lstItemModel>();
            int pageStartIdx = (pageidx - 1) * datalimit;
            for (int i = pageStartIdx; i < Math.Min(filteredData.Count, pageStartIdx + datalimit); i++)
            {
                I18N.LangItem item = filteredData[i];
                lstItemModel llm = new lstItemModel();
                ll.Add(llm);
                llm.isSyskey = I18N.sysKey.Contains(item.Key);
                llm.Original = item.Key;
                llm.lang1 = item.Key;
                llm.lang2 = item.Text;
                llm.Score = item.Score;
            }
            RecordCount = filteredData.Count;
            
            if ("o".Equals(lang_src))
            {
                //lang_src = LI("原始字符串");
            }
            else
            {
                if (!I18N.alllang.Contains(lang_src))
                {
                    lang_src = "zh-cn";
                }
                I18N i18 = new I18N(lang_src);
                foreach (var item in ll)
                {
                    if (!item.isSyskey)
                    {
                        item.lang1 = i18.I(item.Original);
                    }
                }
            }
            rptList.DataSource = ll;
            rptList.DataBind();
            
            baseurl = Request.Path + "?lang1=" + lang_src + "&lang2=" + lang_dst;
            if (includeSys)
            {
                baseurl += "&isys=1";
            }
            if (datalimit != 20)
            {
                baseurl += "&limit=" + datalimit.ToString();
            }
        }

        private void loadDefData()
        {
            pageidx = MakRequest.GetInt("page", 1);
            datalimit = MakRequest.GetInt("limit", 20);
            lang_src = MakRequest.GetString("lang1");
            lang_dst = MakRequest.GetString("lang2");
            includeSys = MakRequest.GetInt("isys", 0) == 1;

            if (string.IsNullOrEmpty(lang_dst) || !File.Exists(HttpContext.Current.Server.MapPath("/i18n/" + lang_dst + ".xml")))
            {
                lang_dst = "en-us";
            }
            List<lstItemModel> ll = new List<lstItemModel>();


            XmlDocument xml_src = new XmlDocument();
            xml_src.Load(HttpContext.Current.Server.MapPath("/i18n/" + lang_dst + ".xml"));
            XmlNode root = xml_src.SelectSingleNode("i18n");

            RecordCount = root.ChildNodes.Count;
            if (!includeSys)
            {
                RecordCount = RecordCount - I18N.sysKey.Count;
            }
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item.Name == "a" && item.Attributes["key"] != null)
                {
                    string key = item.Attributes["key"].Value;
                    bool isSyskey = I18N.sysKey.Exists(aa => aa == key);
                    if (includeSys || !isSyskey)
                    {
                        int scr = item.Attributes["score"] == null ? 5 : Utils.StrToInt(item.Attributes["score"].Value, 5);
                        if (scr > 5)
                        {
                            scr = 5;
                        }
                        else if (scr < 1)
                        {
                            scr = 1;
                        }
                        s1_5[scr - 1]++;
                    }
                }
            }

            int startIdx = (pageidx - 1) * datalimit;
            while (startIdx < root.ChildNodes.Count)
            {
                XmlNode a = root.ChildNodes[startIdx++];                
                if (a.Name == "a" && a.Attributes["key"] != null)
                {
                    string key = a.Attributes["key"].Value;
                    bool isSyskey = I18N.sysKey.Exists(aa => aa == key);
                    if (includeSys || !isSyskey)
                    {
                        lstItemModel llm = new lstItemModel();
                        ll.Add(llm);
                        llm.isSyskey = isSyskey;
                        llm.Original = key;
                        llm.lang1 = key;
                        llm.lang2 = a.Attributes["text"].Value;
                        if (string.IsNullOrEmpty(llm.lang2))
                        {
                            llm.Score = 1;
                        }
                        else
                        llm.Score = a.Attributes["score"] == null ? 5 : Utils.StrToInt(a.Attributes["score"].Value, 5);
                        if (ll.Count >= datalimit)
                        {
                            break;
                        }
                    }
                }           
            }
            if ("o".Equals(lang_src))
            {
                //lang_src = LI("原始字符串");
            }
            else
            {
                if (string.IsNullOrEmpty(lang_src) || !File.Exists(HttpContext.Current.Server.MapPath("/i18n/" + lang_src + ".xml")))
                {
                    lang_src = "zh-cn";
                }
                I18N i18 = new I18N(lang_src);
                foreach (var item in ll)
                {
                    if (!item.isSyskey)
                    {
                        item.lang1 = i18.I(item.Original);
                    }
                }
            }
            rptList.DataSource = ll;
            rptList.DataBind();            
        }

        private void saveSubmit()
        {
            JObject resObj = new JObject();
            resObj["ok"] = false;

            try
            {
                string PostData = System.Text.Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                string orig = "";
                int score = 5;
                string trans = "";
                try
                {
                    JObject jo = (JObject)JsonConvert.DeserializeObject(PostData);
                    orig = jo["orig"].ToString();
                    score = Utils.StrToInt(jo["score"].ToString(), 5);
                    trans = jo["trans"].ToString();
                }
                catch
                {
                    resObj["msg"] = LI("参数错误");
                    return;
                }
                string langPath = HttpContext.Current.Server.MapPath("/i18n/" + MakRequest.GetString("lang2") + ".xml");
                if (File.Exists(langPath))
                {
                    XmlDocument xml_src = new XmlDocument();
                    xml_src.Load(langPath);
                    XmlNode root = xml_src.SelectSingleNode("i18n");

                    foreach (XmlNode item in root.ChildNodes)
                    {
                        if (item.Name == "a" && item.Attributes["key"] != null)
                        {
                            string key = item.Attributes["key"].Value;
                            if (key == orig)
                            {
                                item.Attributes["text"].Value = trans;

                                if (item.Attributes["score"]==null)
                                {
                                    item.Attributes.Append(xml_src.CreateAttribute("score"));
                                }
                                item.Attributes["score"].Value = score.ToString();
                                resObj["ok"] = true;
                                break;
                            }
                        }
                    }
                    xml_src.Save(langPath);
                }
            }
            finally
            {
                Response.Write(resObj.ToString(Newtonsoft.Json.Formatting.None));
                Response.End();
            }
        }

        protected string pageUrl()
        {
            //将filter和sort包含进去
            string tmpUrl = baseurl;
            foreach (var item in Request.QueryString.AllKeys)
            {
                if(item.StartsWith("filter_") || item.StartsWith("sort_"))
                {
                    baseurl += "&" + item + "=" + Request.QueryString[item];
                }
            }
            return tmpUrl;
        }
        class lstItemModel
        {
            /// <summary>
            /// 原始字符串
            /// </summary>
            public string Original{ get; set; }
            /// <summary>
            /// 翻译对照语言
            /// </summary>
            public string lang1 { get; set; }
            /// <summary>
            /// 目标语言
            /// </summary>
            public string lang2 { get; set; }
            /// <summary>
            /// 是否为系统字符串
            /// </summary>
            public bool isSyskey { get; set; }
            /// <summary>
            /// 翻译评分
            /// </summary>
            public int Score { get; set; }
        }

    }
}