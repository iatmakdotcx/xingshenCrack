using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{
    [AttrTableName("rbt_sect")]
    [Serializable]
    public class XingshenSect : XingshenSect_BLL
    {
        public XingshenSect() { defaultValue(this); }
        public XingshenSect(int id) { defaultValue(this); RefreshFromId(id); }
        [AttrTableFieldInfo(DbType.Int32, IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; protected set; }

        [AttrTableFieldInfo(DbType.String, 100)]
        public string uuid { get; set; }
        [AttrTableFieldInfo(DbType.String, 100)]
        public string sectName { get; set; }
        [AttrTableFieldInfo(DbType.String, 100)]
        public string sectid { get; set; }
        [AttrTableFieldInfo(DbType.DateTime)]
        public DateTime lastdonate { get; set; }

    }
}
