using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{
    /// <summary>
    /// 用户存档信息
    /// </summary>
    [AttrTableName("userdata")]
    [Serializable]
    public class XingshenUserData: XingshenUserData_BLL
    {
        public XingshenUserData() { defaultValue(this); }
        public XingshenUserData(int id) { defaultValue(this); RefreshFromId(id); }
        [AttrTableFieldInfo(DbType.Int32, IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; protected set; }

        [AttrTableFieldInfo(DbType.String, 100)]
        public string uuid { get; set; }
        [AttrTableFieldInfo(DbType.DateTime)]
        public DateTime savetime { get; set; }
        [AttrTableFieldInfo(DbType.String, -1)]
        public string data { get; set; }
    }
}
