using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace telegramSvr
{
    public class MakJsonHelper
    {
        /// <summary>
        /// 将第一行数据转换为JObject
        /// </summary>
        /// <param name="atable"></param>
        /// <returns></returns>
        public static JObject DataTableToJsonObj_FirstRow(DataTable atable)
        {
            JObject jo = new JObject();
            if (atable != null)
            {
                DataRow dr = null;
                if (atable.Rows.Count > 0)
                {
                    dr = atable.Rows[0];
                }
                foreach (DataColumn column in atable.Columns)
                {
                    if (dr == null)
                    {
                        jo[column.ColumnName] = null;
                    }
                    else
                    {
                        object vv = dr[column.Ordinal];
                        if (vv == DBNull.Value)
                        {
                            jo[column.ColumnName] = null;
                        }
                        else if (column.DataType.Name == "DateTime")
                        {
                            jo[column.ColumnName] = DateTime.Parse(vv.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                            jo[column.ColumnName] = vv.ToString();
                    }
                }
            }
            return jo;
        }
        /// <summary>
        /// 将第一行数据转换为JObject字符串
        /// </summary>
        /// <param name="atable"></param>
        /// <returns></returns>
        public static string DataTableToJsonObjStr_FirstRow(DataTable atable)
        {
            return DataTableToJsonObj_FirstRow(atable).ToString(Newtonsoft.Json.Formatting.None);
        }

        /// <summary>
        /// 将datatable中的全部行转换为一个JArray
        /// </summary>
        /// <param name="atable"></param>
        /// <returns></returns>
        public static JArray DataTableToJsonArr_AllRow(DataTable atable)
        {
            JArray jA = new JArray();
            if (atable != null && atable.Rows.Count > 0)
            {
                foreach (DataRow dr in atable.Rows)
                {
                    JObject jo = new JObject();
                    foreach (DataColumn column in atable.Columns)
                    {
                        object vv = dr[column.Ordinal];
                        if (vv == DBNull.Value)
                        {
                            jo[column.ColumnName] = null;
                        }
                        else if (column.DataType.Name == "DateTime")
                        {
                            jo[column.ColumnName] = DateTime.Parse(vv.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                            jo[column.ColumnName] = vv.ToString();
                    }
                    jA.Add(jo);
                }
            }
            return jA;
        }
        /// <summary>
        /// 将datatable中的全部行转换为一个JArray字符串
        /// </summary>
        /// <param name="atable"></param>
        /// <returns></returns>
        public static string DataTableToJsonArrStr_AllRow(DataTable atable)
        {
            return DataTableToJsonArr_AllRow(atable).ToString(Newtonsoft.Json.Formatting.None);
        }
        public static string DataTableToJson(DataTable dt, string dtName)
        {
            JObject tableJson = new JObject();
            tableJson[dtName] = DataTableToJsonArr_AllRow(dt);
            return tableJson.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}