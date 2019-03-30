using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace telegramSvr
{
    public class UserInfo
    {
        public string id { get; set; }
        public DateTime createtime { get; set; }
        public string username { get; set; }
        public string passwd { get; set; }
        public string token { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public int is_admin { get; set; }
        public bool is_email_verify { get; set; }
    }
}