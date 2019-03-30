using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{
    [AttrTableName("items")]
    [Serializable]
    public class XingshenItems : XingshenItems_BLL
    {
        public XingshenItems() { defaultValue(this); }
        public XingshenItems(int id) { defaultValue(this); RefreshFromId(id); }
        [AttrTableFieldInfo(DbType.Int32, IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; protected set; }
        [AttrTableFieldInfo(DbType.Int32)]
        public int itemid { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string itemtype { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string childtype { get; set; }
        [AttrTableFieldInfo(DbType.String, 50)]
        public string name { get; set; }
        [AttrTableFieldInfo(DbType.String, 1000)]
        public string miaoshu { get; set; }
    }
}
