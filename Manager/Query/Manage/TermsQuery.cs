using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Query.Manage
{
    public class TermsQuery
    {
        public string select_terms_by_list_for_totalcount()
        {
            string MySql = "" +
                "set session transaction isolation level read uncommitted; " +
                "select " +
                "               IFNULL(COUNT(idx), 0) as totalcount " +
                "from           terms " +
                "where          delflag = 'N' " +
                "#searchWhere ;" +
                "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_terms_by_list()
        {
            string MySql = "" +
                "set session transaction isolation level read uncommitted; " +
                "select " +
                "               idx, title, place, createid, createdt " +
                "from           terms " +
                "where          delflag = 'N' " +
                "#searchWhere " +
                "order by       idx desc " +
                "limit          @startlimit, @endlimit; " +
                "set session transaction isolation level repeatable read;";
            return MySql;
        }
    }
}