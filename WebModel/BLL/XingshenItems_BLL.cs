using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Model
{
    public class XingshenItems_BLL : ModelBase
    {
        public static XingshenItems GetModel(string itemtype, string childtype)
        {
            return GetModelWhere<XingshenItems>("itemtype=@itemtype and childtype=@childtype", dbh.MakeInParam("itemtype", itemtype), dbh.MakeInParam("childtype", childtype));
        }
        public static int Counts(string itemtype, string childtype)
        {
            return Counts<XingshenItems>("itemtype=@itemtype and childtype=@childtype", dbh.MakeInParam("itemtype", itemtype), dbh.MakeInParam("childtype", childtype));
        }

    }

}
