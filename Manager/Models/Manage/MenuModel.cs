using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models.Manage
{
    public class MenuModel
    {
        public string menucode { get; set; }
        public string menupcode { get; set; }
        public string menuname { get; set; }
        public string menuurl { get; set; }
        public int menudispnum { get; set; }
        public string menuuseflag { get; set; }
        public string menuchoice { get; set; }
    }

    public class MenuListModel
    {
        public List<MenuModel> Depth1List { get; set; }
        public List<MenuModel > Depth2List { get; set; }
    }
}