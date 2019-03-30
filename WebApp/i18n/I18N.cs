using Mak.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace telegramSvr
{
    public class I18N
    {
        /// <summary>
        /// 系统键列表
        /// </summary>
        public static List<string> sysKey = new List<string>();
        /// <summary>
        /// locker
        /// </summary>
        private static object lockObj = new object();
        /// <summary>
        /// 所有可支持的语言
        /// </summary>
        public static List<string> alllang = new List<string>();
        /// <summary>
        /// 当前语言对照数据
        /// </summary>
        public Dictionary<string, LangItem> lanStrs = null;
        private string lanPath;
        public I18N(string lanName)
        {
            initSysKeys();
            if (string.IsNullOrEmpty(lanName))
            {
                lanName = "zh-cn";
            }
            if (checkAllLang(lanName))
            {
                lanPath = HttpContext.Current.Server.MapPath("/i18n/" + lanName + ".xml");
                lanStrs = GetLan(lanName);
            }
        }
        private Dictionary<string, LangItem> GetLan(string lanName)
        {
            Mak.Cache.MakCache cache = Mak.Cache.MakCache.GetCacheService();
            Dictionary<string, LangItem> res = (Dictionary<string, LangItem>)cache.RetrieveObject(lanName);
            if (res == null)
            {
                lock (lockObj)
                {
                    res = (Dictionary<string, LangItem>)cache.RetrieveObject(lanName);
                    if (res == null)
                    {
                        res = new Dictionary<string, LangItem>();
                        if (File.Exists(lanPath))
                        {                            
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(lanPath);
                            XmlNode root = xmlDoc.SelectSingleNode("i18n");
                            foreach (XmlNode arow in root.ChildNodes)
                            {
                                if (arow.Name == "a" && arow.Attributes["key"] != null && arow.Attributes["text"] != null)
                                {
                                    string key = arow.Attributes["key"].Value;
                                    string text = arow.Attributes["text"].Value;
                                    int score = 1;
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        score = arow.Attributes["score"] == null ? 5 : Utils.StrToInt(arow.Attributes["score"].Value, 5);
                                    }
                                    res.Add(key, new LangItem(key, text, score));
                                }
                            }
                            cache.AddObject(lanName, res);
                        }
                        else
                        {
                            //没有文件
                            XmlDocument xmlDoc = new XmlDocument();
                            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                            xmlDoc.AppendChild(xmldecl);
                            XmlElement rootElement = xmlDoc.CreateElement("i18n");
                            xmlDoc.AppendChild(rootElement);
                            try
                            {
                                xmlDoc.Save(lanPath);
                            }
                            catch { }
                        }
                    }
                }
            }
            return res;
        }

        public string I(string key, params object[] args)
        {
            string resStr = key;
            if (!string.IsNullOrEmpty(resStr))
            {
                LangItem litem = null;
                if (!lanStrs.TryGetValue(key, out litem))
                {
                    //查找翻译失败
                    saveUnTransStr(key);
                }
                else if(!string.IsNullOrEmpty(litem.Text))
                    resStr = litem.Text;
            }
            if (args.Length > 0)
                return string.Format(resStr, args);
            else
                return resStr;
        }
        private void saveUnTransStr(string key)
        {
            lock (lockObj)
            {
                if (!lanStrs.ContainsKey(key))
                {
                    lanStrs.Add(key, new LangItem(key));
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(lanPath);
                    XmlNode root = xmlDoc.SelectSingleNode("i18n");
                    XmlElement nodea = xmlDoc.CreateElement("a");
                    nodea.SetAttribute("key", key);
                    nodea.SetAttribute("text", "");
                    root.AppendChild(nodea);
                    try
                    {
                        xmlDoc.Save(lanPath);
                    }
                    catch { }
                }
            }
        }
        private static bool checkAllLang(string name)
        {
            if (alllang.Count == 0)
            {
                lock (alllang)
                {
                    if (alllang.Count == 0)
                    {
                        #region addlang
                        alllang.Add("en-us");
                        alllang.Add("af-za");
                        alllang.Add("am-et");
                        alllang.Add("ar-sa");
                        alllang.Add("as-in");
                        alllang.Add("az-latn-az");
                        alllang.Add("bg-bg");
                        alllang.Add("bn-bd");
                        alllang.Add("bn-in");
                        alllang.Add("bs-cyrl-ba");
                        alllang.Add("bs-latn-ba");
                        alllang.Add("ca-es");
                        alllang.Add("cs-cz");
                        alllang.Add("cy-gb");
                        alllang.Add("da-dk");
                        alllang.Add("de-de");
                        alllang.Add("el-gr");
                        alllang.Add("es-es");
                        alllang.Add("et-ee");
                        alllang.Add("eu-es");
                        alllang.Add("fa-ir");
                        alllang.Add("fi-fi");
                        alllang.Add("fil-ph");
                        alllang.Add("fr-fr");
                        alllang.Add("ga-ie");
                        alllang.Add("gl-es");
                        alllang.Add("gu-in");
                        alllang.Add("ha-latn-ng");
                        alllang.Add("he-il");
                        alllang.Add("hi-in");
                        alllang.Add("hr-hr");
                        alllang.Add("hu-hu");
                        alllang.Add("hy-am");
                        alllang.Add("id-id");
                        alllang.Add("ig-ng");
                        alllang.Add("is-is");
                        alllang.Add("it-it");
                        alllang.Add("ja-jp");
                        alllang.Add("ka-ge");
                        alllang.Add("kk-kz");
                        alllang.Add("km-kh");
                        alllang.Add("kn-in");
                        alllang.Add("kok-in");
                        alllang.Add("ko-kr");
                        alllang.Add("ky-kg");
                        alllang.Add("lb-lu");
                        alllang.Add("lt-lt");
                        alllang.Add("lv-lv");
                        alllang.Add("mi-nz");
                        alllang.Add("mk-mk");
                        alllang.Add("ml-in");
                        alllang.Add("mn-mn");
                        alllang.Add("mr-in");
                        alllang.Add("ms-bn");
                        alllang.Add("ms-my");
                        alllang.Add("mt-mt");
                        alllang.Add("nb-no");
                        alllang.Add("ne-np");
                        alllang.Add("nl-nl");
                        alllang.Add("nn-no");
                        alllang.Add("nso-za");
                        alllang.Add("or-in");
                        alllang.Add("pa-in");
                        alllang.Add("pl-pl");
                        alllang.Add("prs-af");
                        alllang.Add("pt-br");
                        alllang.Add("pt-pt");
                        alllang.Add("quz-pe");
                        alllang.Add("ro-ro");
                        alllang.Add("ru-ru");
                        alllang.Add("si-lk");
                        alllang.Add("sk-sk");
                        alllang.Add("sl-si");
                        alllang.Add("sq-al");
                        alllang.Add("sr-cyrl-cs");
                        alllang.Add("sr-latn-cs");
                        alllang.Add("sv-se");
                        alllang.Add("sw-ke");
                        alllang.Add("ta-in");
                        alllang.Add("te-in");
                        alllang.Add("th-th");
                        alllang.Add("tk-tm");
                        alllang.Add("tn-za");
                        alllang.Add("tr-tr");
                        alllang.Add("tt-ru");
                        alllang.Add("uk-ua");
                        alllang.Add("ur-pk");
                        alllang.Add("uz-latn-uz");
                        alllang.Add("vi-vn");
                        alllang.Add("xh-za");
                        alllang.Add("yo-ng");
                        alllang.Add("zh-cn");
                        alllang.Add("zh-hk");
                        alllang.Add("zh-tw");
                        alllang.Add("zu-za");
                        #endregion addlang
                    }
                }
            }
            return alllang.Contains(name);
        }

        private void initSysKeys()
        {
            if (sysKey.Count == 0)
            {
                lock (sysKey)
                {
                    if (sysKey.Count == 0)
                    {
                        string lanPath = HttpContext.Current.Server.MapPath("/i18n/syskeys.xml");
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(lanPath);
                        XmlNode root = xmlDoc.SelectSingleNode("i18n");
                        if (root != null)
                        {
                            foreach (XmlNode arow in root.ChildNodes)
                            {
                                if (arow.Name == "a")
                                {
                                    sysKey.Add(arow.Attributes["key"].Value);
                                }
                            }
                        }
                    }
                }
            }
        }

        public class LangItem
        {
            public LangItem(string key) : this(key, "", 1) { }
            public LangItem(string key, string text, int score)
            {
                Key = key;
                Text = text;
                Score = score;
            }
            public string Key { get; set; }
            public string Text { get; set; }
            public int Score { get; set; }            
        }

    }
}