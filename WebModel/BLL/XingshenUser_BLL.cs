using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class XingshenUser_BLL:ModelBase
    {
        public static List<XingshenUser> GetALL()
        {
            return GetList<XingshenUser>("RobotGroup=0");
        }
        public static List<XingshenUser> GetALLRobot()
        {
            return GetList<XingshenUser>("RobotGroup>0");
        }
        public static List<XingshenUser> GetGroup(int GroupId)
        {
            return GetList<XingshenUser>("RobotGroup=" + GroupId);
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
