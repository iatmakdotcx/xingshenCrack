using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{
    [AttrTableName("userdataWarning")]
    [Serializable]
    public class XingshenUserDataWarning : XingshenUserDataWarning_BLL
    {
        public XingshenUserDataWarning() { defaultValue(this); }
        public XingshenUserDataWarning(int id) { defaultValue(this); RefreshFromId(id); }
        [AttrTableFieldInfo(DbType.Int32, IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; protected set; }

        [AttrTableFieldInfo(DbType.String, 50)]
        public string uuid { get; set; }
        [AttrTableFieldInfo(DbType.String, 5000)]
        public string jgxx { get; set; }
        [AttrTableFieldInfo(DbType.DateTime)]
        public DateTime jgrq { get; set; }
    }
}
