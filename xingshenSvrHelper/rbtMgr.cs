using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Model;

namespace xingshenSvrHelper
{
    public class rbtMgr
    {
        public static Random rd = new Random();

        public static string Randstr(int len = 6)
        {
            string tmpstr = "";
            for (int i = 0; i < len; i++)
            {
                tmpstr += (char)(rd.Next(1, 27) + 64);
            }
            return tmpstr;
        }
        public static string CreateRobot2Group(int groupid, bool isAndroid = true, int cnt = 1)
        {
            int i = 0;
            while (i < cnt)
            {
                string user_name = Randstr();
                string password = "123456";
                XingshenUser Newuser;
                string msg = svrHelper.Create_register(out Newuser, user_name, password, isAndroid);
                if (string.IsNullOrEmpty(msg))
                {
                    Newuser.RobotGroup = groupid;
                    Newuser.Update();
                    i++;
                }
                else if (msg == "用户名已经被占用")
                {
                    //重新
                }
                else
                {
                    return msg;
                }
            }
            return "";
        }


    }
}
