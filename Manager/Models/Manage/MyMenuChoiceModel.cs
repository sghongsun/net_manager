using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models.Manage
{
    public class MyMenuChoiceModel
    {
        public string menucode { get; set; }
        public string menuname { get; set; }
        public string menuurl { get; set; }
    }

    public class MyMenuChoiceListModel
    {
        public List<MyMenuChoiceModel> MyMenuChoiceList { get; set; }
    }
}