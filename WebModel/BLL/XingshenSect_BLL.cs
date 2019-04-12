using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class XingshenSect_BLL:ModelBase
    {
        protected void defaultValue(XingshenSect model)
        {
            model.lastdonate = DateTime.Now;
        }
        public static XingshenSect GetModel(string uid)
        {
            return GetModelWhere<XingshenSect>("uuid=@uid", dbh.MakeInParam("uid", uid));
        }


    }

}
