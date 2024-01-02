using Manager.Request.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models.Manage
{    
    public class TermsModel
    {
        public int idx { get; set; }
        public string title { get; set; }
        public string place { get; set; }
        public string contents { get; set; }
        public string createid { get; set; }
        public DateTime createdt { get; set; }
    }

    public class TermsListModel
    {
        public List<TermsModel> TermsList { get; set; }
        public TermsSearch Search { get; set; }
    }
}