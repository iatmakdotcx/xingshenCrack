using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{

    [AttrTableName("admins")]
    [Serializable]
    public class OptAdmin : OptAdmin_BLL
    {
        public OptAdmin() { defaultValue(this); }
        public OptAdmin(int id) { defaultValue(this); RefreshFromId(id); }
        [AttrTableFieldInfo(DbType.Int32, IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; protected set; }

        [AttrTableFieldInfo(DbType.String, 100)]
        public string username { get; set; }
        [AttrTableFieldInfo(DbType.String, 100)]
        public string pass { get; set; }

        [AttrTableFieldInfo(DbType.DateTime)]
        public DateTime lastlogin { get; set; }
    }
}
