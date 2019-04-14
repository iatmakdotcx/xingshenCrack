using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class XingshenUser_BLL:ModelBase
    {
        public static List<XingshenUser> GetNormalALL()
        {
            return GetList<XingshenUser>("isBanned=0 and RobotGroup=0");
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
    }
}
