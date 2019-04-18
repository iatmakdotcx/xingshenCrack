using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{
    [AttrTableName("checkcode")]
    [Serializable]
    public class XingshenCheckcode : XingshenCheckcode_BLL
    {
        public XingshenCheckcode() { defaultValue(this); }
        public XingshenCheckcode(int id) { defaultValue(this); RefreshFromId(id); }
        [AttrTableFieldInfo(DbType.Int32, IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; protected set; }

        [AttrTableFieldInfo(DbType.String, 50)]
        public string uuid { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string user_name { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string token { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string player_name { get; set; }
        [AttrTableFieldInfo(DbType.Int32)]
        public int net_id { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string mac_addr { get; set; }
        [AttrTableFieldInfo(DbType.String, 500)]
        public string code { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string sg_version { get; set; }
    }
}
