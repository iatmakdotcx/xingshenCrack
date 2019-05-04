using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class OptAdmin_BLL:ModelBase
    {
        public static OptAdmin GetModel(string username)
        {
            return GetModelWhere<OptAdmin>("username=@username", dbh.MakeInParam("username", username));
        }
    }
}
