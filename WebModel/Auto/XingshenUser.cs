using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{
    [AttrTableName("users")]
    [Serializable]
    public class XingshenUser: XingshenUser_BLL
    {
        public XingshenUser() { defaultValue(this); }
        public XingshenUser(int id) { defaultValue(this); RefreshFromId(id); }
        [AttrTableFieldInfo(DbType.Int32, IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; protected set; }

        [AttrTableFieldInfo(DbType.String, 100)]
        public string user_name { get; set; }
        [AttrTableFieldInfo(DbType.String, 100)]
        public string pass { get; set; }
        [AttrTableFieldInfo(DbType.String, 100)]
        public string uuid { get; set; }
        [AttrTableFieldInfo(DbType.String, 100)]
        public string token { get; set; }
        [AttrTableFieldInfo(DbType.Boolean)]
        public bool isAndroid { get; set; }
        [AttrTableFieldInfo(DbType.Int32)]
        public int net_id { get; set; }
        [AttrTableFieldInfo(DbType.Int32)]
        public int RobotGroup { get; set; }
    }
}
