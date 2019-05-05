using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class XingshenUser_BLL:ModelBase
    {
        public static List<XingshenUser> GetNormalALL(bool ShowBanned, bool ShowRobots)
        {
            string where = "";
            if (!ShowBanned)
            {
                where = "isBanned=0";
            }
            if (!ShowRobots)
            {
                if (string.IsNullOrEmpty(where))
                    where += "RobotGroup=0";
                else
                where += " and RobotGroup=0";
            }
            return GetList<XingshenUser>(where);
        }
        public static List<XingshenUser> GetALLRobot()
        {
            return GetList<XingshenUser>("RobotGroup>0");
        }
        public static List<XingshenUser> GetGroup(int GroupId)
        {
            return GetList<XingshenUser>("RobotGroup=" + GroupId);
        }
        public static XingshenUser GetGroupAdmin(int GroupId)
        {
            return GetModelWhere<XingshenUser>("RobotGroup=@gid and isGroupAdmin=1", dbh.MakeInParam("gid", GroupId));
        }
        public static XingshenUser GetModel(string uid)
        {
            return GetModelWhere<XingshenUser>("uuid=@uid", dbh.MakeInParam("uid", uid));
        }
        public static XingshenUser GetModelByUserName(string user_name)
        {
            return GetModelWhere<XingshenUser>("user_name=@user_name", dbh.MakeInParam("user_name", user_name));
        }
        public bool Delete()
        {
            int id = ((XingshenUser)this).id;
            string sql = "insert into users_delete(user_name,pass,uuid,token,isAndroid,net_id,RobotGroup,isBanned,isGroupAdmin,isHold,BanMsg,ExpiryDate,shl)" +
            "select user_name, pass, uuid, token, isAndroid, net_id, RobotGroup, isBanned, isGroupAdmin, isHold, BanMsg, ExpiryDate, shl from users where id = " + id;
            dbh.ExecuteNonQuery(sql);

            sql = "delete from users where id=" + id;
            return dbh.ExecuteNonQuery(sql) > 0;
        }
    }
}
