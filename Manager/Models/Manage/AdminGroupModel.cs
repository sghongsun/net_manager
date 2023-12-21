using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models.Manage
{
    public class AdminGroupModel
    {
        public int groupcode { get; set; }
        public string groupname { get; set; }
        public string groupdesc { get; set; }
        public string groupwrite { get; set; }
        public string groupread { get; set; }
        public int admincnt { get; set; }
        public DateTime createdt { get; set; }
    }
    
    public class AdminGroupListModel
    {
        public List<AdminGroupModel> AdminGroupList { get; set; }
    }
}