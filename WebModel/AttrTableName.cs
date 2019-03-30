using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class AttrTableName : System.Attribute
    {
        public string TableName { get; set; }
        public AttrTableName(string tableName)
        {
            TableName = tableName;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AttrTableFieldInfo : System.Attribute
    {
        public System.Data.DbType Type { get; set; }
        public int Size { get; set; }
        public string Description { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }

        public string DefaultValue { get; set; }

        public AttrTableFieldInfo(System.Data.DbType type, int size)
        {
            Type = type;
            Size = size;
        }
        public AttrTableFieldInfo(System.Data.DbType type)
        {
            Type = type;
            Size = -1;
        }
    }

}
