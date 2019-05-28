using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Model;
using xingshenSvrHelper;

namespace telegramSvr.xingshen
{
    public class ZongmenAutoJob
    {
        private int groupid;
        public int sect_id;

        public event EventHandler<LogEventArgs> OnLogString;
        public int max = 0;
        public int position = 0;
        public List<string> msgs = new List<string>();
        public bool isfinish = true;
        public ZongmenAutoJob(int groupid, int sect_id)
        {
            this.groupid = groupid;
            this.sect_id = sect_id;
        }
        public void LogFormat(string format, params object[] args)
        {
            LogString(string.Format(format, args));
        }
        public void LogString(string sMsg)
        {
            msgs.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >> " + sMsg);
            OnLogString?.Invoke(this, new LogEventArgs(sMsg));
        }
        public void start2()
        {
            isfinish = false;
            msgs.Clear();
            LogString("任务开始...");
            max = 1000;
            position = 0;
            for (int i = 0; i < max; i++)
            {
                LogString("管理员加宗门...");
                System.Threading.Thread.Sleep(1000);//等待3S再试
                position++;
            }
            LogString("任务完成...");
            isfinish = true;
        }
        public void start()
        {
            isfinish = false;
            try
            {
                msgs.Clear();
                LogString("任务开始...");
                if (groupid < 1)
                {
                    LogString("组号无效");
                    return;
                }
                if (sect_id < 1)
                {
                    LogString("宗门id无效");
                    return;
                }
                XingshenUser admin = XingshenUser.GetGroupAdmin(groupid);
                if (admin.id == 0)
                {
                    LogString("组中没有管理员！");
                    return;
                }
                LogString("管理员加宗门..." + admin.user_name);
                string sectName;
                int sectid;
                string errMsg = svrHelper.Create_sects_info(admin, out sectName, out sectid);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    LogString(errMsg);
                    return;
                }
                if (sectid == 0)
                {
                    //未加入宗门
                    errMsg = svrHelper.Create_sects_join(admin, sect_id);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        LogString(errMsg);
                        return;
                    }
                    //最长等待5分钟
                    for (int i = 0; i < 100; i++)
                    {
                        LogString("等待同意加入宗门...");
                        errMsg = svrHelper.Create_sects_info(admin, out sectName, out sectid);
                        if (sectid != 0)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(3000);//等待3S再试
                    }
                }
                else if (sectid != sect_id)
                {
                    //已加入其它宗门
                    LogString("当前管理员已加入其它宗门："+ sectName);
                    errMsg = svrHelper.Create_sects_quit(admin);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        LogString("退出宗门发生错误：" + errMsg);
                    }else
                        LogString("已退出宗门...2小时候再使用本功能！");
                    return;
                }
                LogString("已加入宗门...");

                JArray ja;
                //最长等待5分钟
                for (int i = 0; i < 100; i++)
                {
                    errMsg = svrHelper.Create_sects_joinlist(admin, out ja);
                    if (errMsg == "")
                    {
                        break;
                    }
                    LogString("等待成为管理员（副掌门）...");
                    if (errMsg != "您没有权限")
                    {
                        LogString(errMsg);
                        LogString("任务已终止！");
                        return;
                    }
                    System.Threading.Thread.Sleep(3000);//等待3S再试
                }
                List<XingshenUser> rs = XingshenUser.GetGroup(groupid);
                max = rs.Count;
                position = 0;
                foreach (var item in rs)
                {                   
                    if (item.id != admin.id)
                    {
                        LogFormat("[{0}]加入宗门...", item.user_name);
                        string robot_sectName;
                        int robot_sectid;
                        errMsg = svrHelper.Create_sects_info(item, out robot_sectName, out robot_sectid);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            LogString(errMsg);
                            LogString("跳过");
                            continue;
                        }
                        if (robot_sectid > 0 && robot_sectid != sect_id)
                        {
                            LogString("已加入其他宗门");
                            errMsg = svrHelper.Create_sects_quit(item);
                            if (!string.IsNullOrEmpty(errMsg))
                            {
                                LogString("退出宗门发生错误：" + errMsg);
                            }
                            else
                                LogString("已退出宗门...");
                            LogString("跳过");
                            continue;
                        }
                        errMsg = svrHelper.Create_sects_join(item, sect_id);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            LogString("加入宗门出错："+errMsg);
                            continue;
                        }
                        System.Threading.Thread.Sleep(100);
                        LogString("管理员同意...");
                        errMsg = svrHelper.Create_sects_joinlist(admin, out ja);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            LogString("加入列表查询失败："+errMsg);
                            LogString("任务已终止！");
                            return;
                        }
                        errMsg = svrHelper.Create_sects_agreed_join(admin, item.uuid);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            LogString("同意加入失败:"+errMsg);
                            if (errMsg!= "玩家已有宗门")
                            {
                                LogString("任务已终止！");
                                return;
                            }
                        }
                        /////////////////////////////////////
                        LogString("开始捐赠...");
                        errMsg = svrHelper.Create_sects_donate(item);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            LogString("捐赠失败:" + errMsg);
                            if (errMsg.IndexOf("100") > 0)
                            {
                                LogString("终止....");
                                break;
                            }
                        }
                        LogString("捐赠完成...");
                        svrHelper.Create_sects_quit(item);
                    }
                    else
                    {
                        LogString("管理员捐赠...");
                        errMsg = svrHelper.Create_sects_donate(admin);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            LogString("捐赠失败:" + errMsg);
                        }
                        LogString("捐赠完成...");
                    }
                    position++;
                }
            }
            finally
            {
                LogString("任务完成...");
                isfinish = true;
            }
        }

        public void checkBan()
        {
            List<XingshenUser> rs = XingshenUser.GetGroup(groupid);
            max = rs.Count;
            position = 0;
            foreach (var item in rs)
            {
                string dct;
                string errmsg = svrHelper.GetUserLastDCTime(item, out dct);
            }
        }
    }

    public class LogEventArgs : EventArgs
    {
        internal LogEventArgs(string sMsg)
        {
            this._sMessage = sMsg;
        }
        public string LogString
        {
            get
            {
                return this._sMessage;
            }
        }
        private readonly string _sMessage;
    }
}