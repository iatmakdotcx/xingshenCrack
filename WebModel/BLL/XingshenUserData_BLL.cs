using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class XingshenUserData_BLL:ModelBase
    {
        protected void defaultValue(XingshenUserData model)
        {
            model.savetime = DateTime.Now;
        }
        public static XingshenUserData GetModel(string uid)
        {
            return GetModelWhere<XingshenUserData>("uuid=@uid", dbh.MakeInParam("uid", uid));
        }
    }
}
