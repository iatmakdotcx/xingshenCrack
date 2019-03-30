using Mak.Common;
using Mak.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Web.Model
{
    public abstract class ModelBase
    {
        /// <summary>
        /// 保存原始数据，Update的时候只Update变动的字段
        /// </summary>
        [NonSerialized]
        private DataTable sourceDt = null;
        [NonSerialized]
        private static object locker = new object();
        /// <summary>
        /// 数据库访问接口
        /// </summary>
        [NonSerialized]
        protected static DbHelperItem dbh = null;
        /// <summary>
        /// 初始化数据库连接对象
        /// </summary>
        /// <param name="helperItem"></param>
        public static void Init(DbHelperItem helperItem)
        {
            if (dbh == null)
            {
                lock (locker)
                {
                    if (dbh == null)
                    {
                        dbh = helperItem;
                    }
                }
            }
        }
        public static void Init(string configName)
        {
            Init(DbHelper.getHelper(configName));
        }
        protected virtual void defaultValue(object model) { }
        private string getTableName()
        {
            var classAttrs = this.GetType().GetCustomAttributes(typeof(AttrTableName), true);
            if (classAttrs.Length != 1)
            {
                throw new NotSupportedException("必须给类设置AttrTableName特性才能使用此功能！");
            }
            return (classAttrs[0] as AttrTableName).TableName;
        }
        /// <summary>
        /// 根据Form的数据填充当前模型
        /// </summary>
        public void BindForm()
        {
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (!MakRequest.Exists(pi.Name)) continue;
                var attrs = pi.GetCustomAttributes(typeof(AttrTableFieldInfo), false);
                if (attrs.Length > 0)
                {
                    AttrTableFieldInfo item = attrs[0] as AttrTableFieldInfo;
                    if (!item.IsPrimaryKey)
                    {
                        Type type = pi.PropertyType;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            //如果是带问号的类型
                            type = pi.PropertyType.GetGenericArguments()[0];
                        }
                        if (type == typeof(int))
                        {
                            pi.SetValue(this, MakRequest.GetInt(pi.Name, 0), null);
                        }
                        else if (type == typeof(long))
                        {
                            pi.SetValue(this, MakRequest.GetInt64(pi.Name, 0), null);
                        }
                        else if (type == typeof(string))
                        {
                            pi.SetValue(this, MakRequest.GetString(pi.Name), null);
                        }

                        else if (type == typeof(DateTime))
                        {
                            pi.SetValue(this, Utils.StrToDateTime(MakRequest.GetString(pi.Name), DateTime.MinValue).Value, null);
                        }
                        else if (type == typeof(bool))
                        {
                            string boolval = MakRequest.GetString(pi.Name).ToLower();
                            if (boolval == "1" || boolval == "yes" || boolval == "on")
                            {
                                pi.SetValue(this, true, null);
                            }
                            else
                                pi.SetValue(this, false, null);
                        }
                        else if (type == typeof(decimal))
                        {
                            pi.SetValue(this, (decimal)MakRequest.GetFloat(pi.Name, 0), null);
                        }
                        else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                        {
                            pi.SetValue(this, MakRequest.GetFloat(pi.Name, 0), null);
                        }
                        else
                        {
                            throw new NotImplementedException("未完整定义的类型！" + pi.GetType().FullName);
                        }
                    }
                    var value = pi.GetValue(this, null);
                }
            }
        }
        /// <summary>
        /// 更新模型
        /// </summary>
        /// <param name="where">扩展更新条件</param>
        /// <returns></returns>
        public bool Update(string where = "")
        {
            return Update(null, where);
        }
        public bool Update(DbTransaction transaction, string where = "")
        {
            string tablename = getTableName();
            List<DbParameter> paras = new List<DbParameter>();
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();//数据字段
            StringBuilder str2 = new StringBuilder();//条件字段
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                var attrs = pi.GetCustomAttributes(typeof(AttrTableFieldInfo), false);
                if (attrs.Length > 0)
                {
                    AttrTableFieldInfo item = attrs[0] as AttrTableFieldInfo;
                    var value = pi.GetValue(this, null);
                    if (value != null && value.GetType() == typeof(DateTime) && (DateTime)value == DateTime.MinValue)
                    {
                        continue;
                    }
                    if (item.IsPrimaryKey)
                    {
                        str2.Append(pi.Name + "=@" + pi.Name + ",");
                        paras.Add(dbh.MakeInParam(pi.Name, item.Type, item.Size, value ?? DBNull.Value));
                    }
                    if (sourceDt != null && sourceDt.Rows.Count > 0)
                    {
                        object dbval = sourceDt.Rows[0][pi.Name];
                        if (dbval == DBNull.Value || value == null)
                        {
                            continue;
                        }
                        else if (value.ToString() == dbval.ToString())
                        {
                            continue;
                        }
                        else if (dbval.GetType() == typeof(float) || dbval.GetType() == typeof(double) || dbval.GetType() == typeof(decimal))
                        {
                            float f1 = Utils.StrToFloat(value, 0);
                            float f2 = Utils.StrToFloat(dbval, 0);
                            if (f1 == f2) continue;
                        }
                    }
                    if (!item.IsIdentity)
                    {
                        str1.Append(pi.Name + "=@" + pi.Name + ",");
                        paras.Add(dbh.MakeInParam(pi.Name, item.Type, item.Size, value ?? DBNull.Value));
                    }
                }
            }
            if (str1.Length == 0) return true;
            strSql.Append("Update " + tablename + " set ");
            strSql.Append(str1.ToString().Trim(','));
            strSql.Append(" where ");
            if (str2.Length > 0)
                strSql.Append(str2.ToString().Trim(','));
            else
                strSql.Append(" id=@id ");

            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append(" and ").Append(where);
            }
            if (transaction != null)
                return dbh.ExecuteNonQuery(transaction, CommandType.Text, strSql.ToString(), paras.ToArray()) > 0;
            else
                return dbh.ExecuteNonQuery(CommandType.Text, strSql.ToString(), paras.ToArray()) > 0;
        }
        public bool Add(DbTransaction transaction = null)
        {
            string tablename = getTableName();
            List<DbParameter> paras = new List<DbParameter>();
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();//数据字段
            StringBuilder str2 = new StringBuilder();//数据参数
            PropertyInfo identityPi = null;
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                var value = pi.GetValue(this, null);
                if (value == null) continue;
                if (value.GetType() == typeof(DateTime) && (DateTime)value == DateTime.MinValue)
                {
                    continue;
                }
                var attrs = pi.GetCustomAttributes(typeof(AttrTableFieldInfo), false);
                if (attrs.Length > 0)
                {
                    AttrTableFieldInfo item = attrs[0] as AttrTableFieldInfo;
                    if (!item.IsIdentity)
                    {
                        paras.Add(dbh.MakeInParam(pi.Name, item.Type, item.Size, value ?? DBNull.Value));
                        str1.Append(pi.Name + ",");
                        str2.Append("@" + pi.Name + ",");
                    }
                    else identityPi = pi;
                }
            }
            if (str1.Length > 0)
            {
                strSql.Append("insert into  " + tablename + "(");
                strSql.Append(str1.ToString().Trim(','));
                strSql.Append(") values (");
                strSql.Append(str2.ToString().Trim(','));
                strSql.Append(") ");
                if (identityPi == null)
                {
                    if (transaction != null)
                        return dbh.ExecuteNonQuery(transaction, CommandType.Text, strSql.ToString(), paras.ToArray()) > 0;
                    else
                        return dbh.ExecuteNonQuery(CommandType.Text, strSql.ToString(), paras.ToArray()) > 0;
                }
                else
                {
                    int id;
                    int optCnt = 0;
                    if (transaction != null)
                        optCnt = dbh.ExecuteNonQuery(out id, transaction, CommandType.Text, strSql.ToString(), paras.ToArray());
                    else
                        optCnt = dbh.ExecuteNonQuery(out id, CommandType.Text, strSql.ToString(), paras.ToArray());
                    if (optCnt > 0)
                    {
                        identityPi.SetValue(this, id, null);
                        return true;
                    }
                }
            }
            return false;
        }
        protected bool Delete(int id)
        {
            return Delete(null, id);
        }
        protected bool Delete(DbTransaction transaction, int id)
        {
            string SsSql = "delete " + getTableName() + " where id=@id ";
            if (transaction != null)
                return dbh.ExecuteNonQuery(transaction, CommandType.Text, SsSql, dbh.MakeInParam("id", id)) > 0;
            else
                return dbh.ExecuteNonQuery(CommandType.Text, SsSql, dbh.MakeInParam("id", id)) > 0;
        }
        public DataTable getDataTableById(DbTransaction transaction, int id)
        {
            string SsSql = "select * from " + getTableName() + " where id=@id ";
            DataSet ds;
            if (transaction != null)
                ds = dbh.ExecuteDataset(transaction, CommandType.Text, SsSql, dbh.MakeInParam("id", id));
            else
                ds = dbh.ExecuteDataset(CommandType.Text, SsSql, dbh.MakeInParam("id", id));
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
        }
        protected void RefreshFromId(int id)
        {
            RefreshFromId(null, id);
        }
        protected void RefreshFromId(DbTransaction transaction, int id)
        {
            if (id > 0)
            {
                sourceDt = getDataTableById(transaction, id);
                if (sourceDt != null && sourceDt.Rows.Count > 0)
                {
                    DataRowToObj(this, sourceDt.Rows[0]);
                }
            }
        }
        protected static T GetModelWhere<T>(string strWhere, params DbParameter[] commandParameters)
        {
            return GetModelWhere<T>(null, strWhere, commandParameters);
        }
        protected static T GetModelWhere<T>(DbTransaction transaction, string strWhere, params DbParameter[] commandParameters)
        {
            T model = Activator.CreateInstance<T>();
            string tablename = (model as ModelBase).getTableName();
            string SsSql = "select top 1 * from " + tablename;
            if (!string.IsNullOrEmpty(strWhere))
                SsSql += " where " + strWhere;            
            DataSet ds;
            if (transaction != null)
                ds = dbh.ExecuteDataset(transaction, CommandType.Text, SsSql, commandParameters);
            else
                ds = dbh.ExecuteDataset(CommandType.Text, SsSql, commandParameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRowToObj(model, ds.Tables[0].Rows[0]);
            }
            return model;
        }

        //public DataTable getByParams(params DbParameter[] commandParameters)
        //{
        //    string SsSql = "select * from " + getTableName();
        //    if (commandParameters.Length > 0)
        //    {
        //        SsSql += " where ";
        //        foreach (var item in commandParameters)
        //        {
        //            SsSql += item.ParameterName + "=@" + item.ParameterName;
        //        }
        //    }
        //    DataSet ds = dbh.ExecuteDataset(CommandType.Text, SsSql, commandParameters);
        //    return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
        //}
        //public DataTable getWhere(string wherestr)
        //{
        //    string SsSql = "select * from " + getTableName();
        //    if (!string.IsNullOrEmpty(wherestr))
        //        SsSql += " where " + wherestr;

        //    DataSet ds = dbh.ExecuteDataset(SsSql);
        //    return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
        //}
        private static object hackType(object value, Type conversionType)
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

        protected static void DataRowToObj(object model, DataRow dr)
        {
            if (dr != null)
            {
                DataRowToObj(model, dr, model.GetType().GetProperties());
            }
        }
        protected static void DataRowToObj(object model, DataRow dr, PropertyInfo[] Properties)
        {
            if (dr != null)
            {
                foreach (PropertyInfo property in Properties)
                {
                    DataColumn dc = dr.Table.Columns[property.Name];
                    if (dc != null && dr[dc.Ordinal] != DBNull.Value)
                    {
                        //property.SetValue(model, HackType(dr[dc.Ordinal], property.PropertyType), null);
                        property.SetValue(model, dr[dc.Ordinal], null);
                    }
                }
            }
        }
        protected static List<T> GetList<T>(string strWhere, string strFieldOrder, int PageSize, int page)
        {
            return GetList<T>(null, strWhere, strFieldOrder, PageSize, page);
        }
        protected static List<T> GetList<T>(DbTransaction transaction,string strWhere, string strFieldOrder, int PageSize, int page)
        {
            bool flag = strWhere.IndexOf("lbsql{") > 0;
            List<T> result = new List<T>();
            if (strWhere.IndexOf("lbsql{") > 0)
            {
                throw new NotSupportedException("未处理 lbsql 关键字！");
                //SQLPara para = new SQLPara(strWhere, strFieldOrder, "");
                //result = this.GetList(para, PageSize, page);
            }
            else
            {
                string strFieldKey = "id";
                string strFieldShow = "*";
                T model = Activator.CreateInstance<T>();
                string tablename = (model as ModelBase).getTableName();
                DbParameter[] paras = new DbParameter[7] {
                    dbh.MakeInParam("@TableName", tablename),
                    dbh.MakeInParam("@FieldKey", strFieldKey),
                    dbh.MakeInParam("@FieldShow", strFieldShow),
                    dbh.MakeInParam("@FieldOrder", strFieldOrder),
                    dbh.MakeInParam("@Where", strWhere),
                    dbh.MakeInParam("@PageSize", DbType.Int32, PageSize),
                    dbh.MakeInParam("@PageIndex", DbType.Int32, page)
                };
                DataSet ds;
                if (transaction != null)
                    ds = dbh.ExecuteDataset(transaction, CommandType.StoredProcedure, "usp_CommonPagination", paras);
                else
                    ds = dbh.ExecuteDataset(CommandType.StoredProcedure, "usp_CommonPagination", paras);
                if (ds != null && ds.Tables.Count > 0)
                {
                    PropertyInfo[] Properties = model.GetType().GetProperties();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model = Activator.CreateInstance<T>();
                        DataRowToObj(model, dr, Properties);
                        result.Add(model);
                    }
                }
            }
            return result;
        }
        protected static List<T> GetList<T>(string strWhere, params DbParameter[] commandParameters)
        {
            return GetList<T>(null, strWhere, commandParameters);
        }
        protected static List<T> GetList<T>(DbTransaction transaction, string strWhere, params DbParameter[] commandParameters)
        {
            List<T> result = new List<T>();
            T model = Activator.CreateInstance<T>();
            string tablename = (model as ModelBase).getTableName();
            string SsSql = "select * from " + tablename;
            if (!string.IsNullOrEmpty(strWhere))
                SsSql += " where " + strWhere;

            DataSet ds;
            if (transaction != null)
                ds = dbh.ExecuteDataset(transaction, CommandType.Text, SsSql, commandParameters);
            else
                ds = dbh.ExecuteDataset(CommandType.Text, SsSql, commandParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                PropertyInfo[] Properties = model.GetType().GetProperties();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    model = Activator.CreateInstance<T>();
                    DataRowToObj(model, dr, Properties);
                    result.Add(model);
                }
            }
            return result;
        }
        protected static int Counts<T>(string where, params DbParameter[] commandParameters)
        {
            return Counts<T>(null, where, commandParameters);
        }
        protected static int Counts<T>(DbTransaction transaction, string where, params DbParameter[] commandParameters)
        {
            if (where.IndexOf("lbsql{") > 0)
            {
                throw new NotSupportedException("未处理 lbsql 关键字！");
                //SQLPara para = new SQLPara(strWhere, "", "");
                //result = this.Counts(para);
            }
            else
            {
                T model = Activator.CreateInstance<T>();
                string tablename = (model as ModelBase).getTableName();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from " + tablename);
                if (where.Trim() != "")
                {
                    strSql.Append(" where " + where);
                }
                if (transaction != null)
                    return (int)dbh.ExecuteScalar(transaction, CommandType.Text, strSql.ToString(), commandParameters);
                else
                    return (int)dbh.ExecuteScalar(CommandType.Text, strSql.ToString(), commandParameters);
            }
        }
        protected static int Delete<T>(string where, params DbParameter[] commandParameters)
        {
            return Delete<T>(null, where, commandParameters);
        }
        protected static int Delete<T>(DbTransaction transaction ,string where, params DbParameter[] commandParameters)
        {
            T model = Activator.CreateInstance<T>();
            string tablename = (model as ModelBase).getTableName();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete " + tablename);
            if (where.Trim() != "")
            {
                strSql.Append(" where " + where);
            }
            if (transaction != null)
                return dbh.ExecuteNonQuery(transaction, strSql.ToString(), commandParameters);
            else
                return dbh.ExecuteNonQuery(CommandType.Text, strSql.ToString(), commandParameters);
        }
        protected static string GetScalar<T>(string field, string where, params DbParameter[] commandParameters)
        {
            return GetScalar<T>(null, field, where,  commandParameters);
        }
        protected static string GetScalar<T>(DbTransaction transaction, string field, string where, params DbParameter[] commandParameters)
        {
            if (where.IndexOf("lbsql{") > 0)
            {
                throw new NotSupportedException("未处理 lbsql 关键字！");
                //SQLPara para = new SQLPara(strWhere, "", "");
                //result = this.Counts(para);
            }
            else
            {
                T model = Activator.CreateInstance<T>();
                string tablename = (model as ModelBase).getTableName();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 " + field + " from " + tablename);
                if (where.Trim() != "")
                {
                    strSql.Append(" where " + where);
                }
                object scobj;
                if (transaction != null)
                    scobj = dbh.ExecuteScalar(transaction, CommandType.Text, strSql.ToString(), commandParameters);
                else
                    scobj = dbh.ExecuteScalar(CommandType.Text, strSql.ToString(), commandParameters);
                if (scobj == null || scobj == DBNull.Value)
                {
                    return "";
                }
                return scobj.ToString();
            }
        }
    }
}
