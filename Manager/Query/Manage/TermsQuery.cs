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

        public string insert_terms()
        {
            string MySql = "" +
                "insert into terms ( " +
                "           title, " +
                "           place, " +
                "           contents, " +
                "           createid, " +
                "           createip " +
                "   ) values ( " +
                "           @title, " +
                "           @place, " +
                "           @contents, " +
                "           @id, " +
                "           @ip " +
                ")";
            return MySql;
        }

        public string select_terms_by_idx()
        {
            string MySql = "" +
                "set session transaction isolation level read uncommitted; " +
                "select " +
                "               idx, title, place, contents, createid, createdt " +
                "from           terms " +
                "where          idx = @idx; " +
                "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string update_terms()
        {
            string MySql = "" +
                "update terms set " +
                "           title = @title, " +
                "           place = @place, " +
                "           contents = @contents, " +
                "           updateid = @id, " +
                "           updateip = @ip " +
                "where      idx = @idx;";
            return MySql;
        }

        public string update_terms_for_delflag()
        {
            string MySql = "" +
                "update terms set " +
                "           delflag = 'Y', " +
                "           updateid = @id, " +
                "           updateip = @ip " +
                "where      idx = @idx;";
            return MySql;
        }
    }
}