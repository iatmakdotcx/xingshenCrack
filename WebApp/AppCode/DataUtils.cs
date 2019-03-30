using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace telegramSvr
{
    public class DataUtils
    {
        private static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        public static List<T> DataTableToList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        DataColumn dc = dt.Columns[property.Name];
                        if (dc != null && dr[dc.Ordinal] != DBNull.Value)
                        {
                            property.SetValue(model, HackType(dr[dc.Ordinal], property.PropertyType), null);
                        }
                    }
                    list.Add(model);
                }
            }
            return list;
        }

        public static T DataRowToObj<T>(DataRow dr)
        {
            T model = default(T);
            if (dr != null)
            {
                model = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    DataColumn dc = dr.Table.Columns[property.Name];
                    if (dc != null && dr[dc.Ordinal] != DBNull.Value)
                    {
                        property.SetValue(model, HackType(dr[dc.Ordinal], property.PropertyType), null);
                    }
                }                
            }
            return model;
        }



    }
}