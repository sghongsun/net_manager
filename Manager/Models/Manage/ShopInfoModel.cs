using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models.Manage
{
    public class ShopInfoModel
    {
        public int standardprice { get; set; }
        public int deliveryprice { get; set; }
        public int returndeliveryprice { get; set; }
        public int changedeliveryprice { get; set; }
        public int foreignstandardprice { get; set; }
        public int foreigndeliveryprice { get; set; }
        public int foreignreturndeliveryprice { get; set; }
        public int foreignchangedeliveryprice { get; set; }
        public string rzipcode { get; set; }
        public string raddr1 { get; set; }
        public string raddr2 { get; set; }
        public string foreignrzipcode { get; set; }
        public string foreignraddr1 { get; set; }
        public string foreignraddr2 { get; set; }
        public int txtreviewpoint { get; set; }
        public int imgreviewpoint { get; set; }
    }
}