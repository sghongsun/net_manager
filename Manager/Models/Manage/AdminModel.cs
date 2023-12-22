using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models.Manage
{
    public class AdminModel
    {
        public string adminid { get; set; }
        public string adminpwd { get; set; }
        public string adminname { get; set; }
        public int groupcode { get; set; }
        public string hp { get; set; }
        public string authflag { get; set; }
        public int pwderrcnt { get; set; }
        public string groupname { get; set; }
        public string groupwrite { get; set; }
        public string groupread { get; set; }
        public string createid { get; set; }
        public string createip { get; set; }
        public DateTime createdt { get; set; }
        public string updateid { get; set; }
        public string updateip { get; set; }
        public DateTime updatedt { get; set; }
    }
}