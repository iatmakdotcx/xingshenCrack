using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Model;

namespace telegramSvr.xingshen
{
    public class pubPagebase : System.Web.UI.Page
    {
        protected optUser _optuser;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _optuser = (optUser)Session["usrifo"];
#if DEBUG
            //if (_optuser == null)
            //{
            //    _optuser = new optUser();
            //    _optuser.isAdmin = true;
            //}
#endif
            if (_optuser == null)
            {
                //未登录
                if (Request.Path.ToLower().IndexOf("login") == -1)
                {
                    //非登陆页
                    HttpContext.Current.Response.Redirect("/login.aspx");
                }
                return;
            }
        }

        public class optUser
        {
            public bool isAdmin = false;
            public XingshenUser xingshenUser = null;
        }
    }

    
}