using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Query.Manage
{
    public class MyMenuChoiceQuery
    {
        public string select_admin_menu_choice_by_adminId()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select      " +
                    "               a.menucode, a.dispnum, b.menuname, b.menuurl " +
                    "from           admin_menu_choices as a " +
                    "inner join     admin_menus as b on a.menucode = b.menucode " +
                    "where          a.adminid = @adminid " +
                    "and            b.menuuseflag = 'Y' " +
                    "and            b.menuchoice = 'Y' " +
                    "order by       a.dispnum ASC; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string delete_admin_menu_choice_by_menucode_For_All()
        {
            string MySql = "" +
                    "delete " +
                    "from        admin_menu_choices " +
                    "where       menucode = @menuCode";
            return MySql;
        }
    }
}