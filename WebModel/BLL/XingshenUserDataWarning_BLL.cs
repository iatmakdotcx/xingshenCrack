using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class XingshenUserDataWarning_BLL:ModelBase
    {
        public static DataTable GetWarningList(string uuid)
        {
            return dbh.GetDataTableBySQL("SELECT id,jgxx,jgrq FROM userdataWarning where uuid=@uuid order by id desc ",
                dbh.MakeInParam("uuid", uuid));
        }

        public static bool Delete(string uuid, int id)
        {
            if (id == -1)
            {
                return Delete<XingshenUserDataWarning>("uuid=@uuid", dbh.MakeInParam("uuid", uuid)) > 0;
            }
            else
                return Delete<XingshenUserDataWarning>("id=@id and uuid=@uuid", dbh.MakeInParam("id", id), dbh.MakeInParam("uuid", uuid)) > 0;
        }

    }
}
