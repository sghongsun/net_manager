using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Query.Manage
{
    public class AdminGroupQuery
    {
        public string select_admin_group_by_groupcode()
        {
            string MySql = "" +
                "set session transaction isolation level read uncommitted; " +
                "select " +
                "               groupcode, groupname, groupdesc, groupwrite, groupread " +
                "from           admin_groups " +
                "where          groupcode = @groupcode; " +
                "set session transaction isolation level repeatable read;";
            return MySql;
        }
    }
}