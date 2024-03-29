﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using xingshenSvrHelper;

namespace telegramSvr.xingshen
{
    public partial class rbts : pubPagebase
    {
        private static Dictionary<int, ZongmenAutoJob> Jobs = new Dictionary<int, ZongmenAutoJob>();
        protected int sect_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                JObject Rep = new JObject();
                Rep["ok"] = false;
                Rep["msg"] = "";
                try
                {
                    string postdata = Encoding.UTF8.GetString(HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes));
                    if (Request["a"] == "rc")
                    {
                        Rep = newUser(postdata, Rep);
                    }else if (Request["a"] == "cj")
                    {
                        Rep = newJob(postdata, Rep);
                    }
                    else if (Request["a"] == "ji")
                    {
                        Rep = newJobinfo(postdata, Rep);
                    } else if (Request["a"] == "chkb")
                    {
                        Rep = newCheckBan(postdata, Rep);
                    }
                }
                finally
                {
                    Response.Write(Rep.ToString(Formatting.None));
                    Response.End();
                }
            }else if (Mak.Common.MakRequest.GetInt("gid", -1) == -1)
            {
                Response.Redirect(Request.Path + "?gid=1");
            }
            else
            {
                ZongmenAutoJob job;
                if (Jobs.TryGetValue(Mak.Common.MakRequest.GetInt("gid", -1), out job) && !job.isfinish)
                {
                    sect_id = job.sect_id;
                }
            }

        }

        private static JObject newUser(string postdata, JObject Rep)
        {
            JObject jo = null;
            try
            {
                jo = (JObject)JsonConvert.DeserializeObject(postdata);
            }
            catch (Exception exx)
            {
                Rep["msg"] = exx.Message;
                return Rep;
            }

            bool isAndroid = jo["platform"]?.ToString() == "0";
            int cnt = int.Parse(jo["cnt"]?.ToString());
            int groupid = int.Parse(jo["groupid"]?.ToString());
            string errmsg = rbtMgr.CreateRobot2Group(groupid, isAndroid, cnt);
            if (!string.IsNullOrEmpty(errmsg))
            {
                Rep["msg"] = errmsg;
                return Rep;
            }
            Rep["ok"] = true;
            return Rep;
        }

        private static JObject newJob(string postdata, JObject Rep)
        {
            int gid = Mak.Common.MakRequest.GetInt("gid",0);
            int sid = Mak.Common.MakRequest.GetInt("sid",0);
            if (gid < 1 || sid < 1)
            {
                Rep["msg"] = "参数错误！";
                return Rep;
            }
            lock (Jobs)
            {
                ZongmenAutoJob job;
                if(!Jobs.TryGetValue(gid, out job))
                {
                    job = new ZongmenAutoJob(gid, sid);
                    Jobs.Add(gid, job);
                }
                if (job.isfinish)
                {
                    Thread th = new Thread(() =>
                    {
                        job.start();
                    });
                    th.Start();
                }
            }
            Rep["ok"] = true;
            return Rep;
        }
        private static JObject newJobinfo(string postdata, JObject Rep)
        {
            int gid = Mak.Common.MakRequest.GetInt("gid", 0);
            int sid = Mak.Common.MakRequest.GetInt("sid", 0);
            if (gid < 1 || sid < 1)
            {
                Rep["msg"] = "参数错误！";
                return Rep;
            }
            lock (Jobs)
            {
                ZongmenAutoJob job;
                if (!Jobs.TryGetValue(gid, out job))
                {
                    job = new ZongmenAutoJob(gid, sid);
                    Jobs.Add(gid, job);
                }
                JObject data = new JObject();
                data["max"] = job.max;
                data["posi"] = job.position;
                data["msg"] = string.Join("\r\n", job.msgs);
                data["finish"] = job.isfinish;
                Rep["data"] = data;
            }
            Rep["ok"] = true;
            return Rep;
        }

        private static JObject newCheckBan(string postdata, JObject Rep)
        {
            int gid = Mak.Common.MakRequest.GetInt("gid", 0);
            if (gid < 1)
            {
                Rep["msg"] = "参数错误！";
                return Rep;
            }
            lock (Jobs)
            {
                ZongmenAutoJob job = new ZongmenAutoJob(gid, 0);
                job.checkBan();
            }
            Rep["ok"] = true;
            return Rep;
        }
    }

}