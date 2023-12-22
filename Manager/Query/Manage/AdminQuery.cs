using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Manager.Query.Manage
{
    public class AdminQuery
    {
        public string select_by_adminId_for_group()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "               a.adminid, a.adminname, a.adminpwd, a.groupcode, a.authflag, a.pwderrcnt, a.hp, b.groupwrite, b.groupread " +
                    "from           admins as a " +
                    "inner join     admin_groups as b on a.groupcode = b.groupcode " +
                    "where          adminid = @adminId " +
                    "and            a.delflag = 'N'; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string login_insert()
        {
            string MySql = "" +
                    "insert into admin_logins ( " +
                    "           adminid, " +
                    "           ip" +
                    "        ) values ( " +
                    "           @adminId, " +
                    "           @ip" +
                    "        )";
            return MySql;
        }

        public string update_for_login_success()
        {
            string MySql = "" +
                    "update admins set " +
                    "       pwderrcnt = 0, " +
                    "       updateid = @adminId, " +
                    "       pdateip = @ip, " +
                    "       updatedt = now() " +
                    "where adminid = @adminId";
            return MySql;
        }

        public string update_for_login_fail()
        {
            string MySql = "" +
                "update admins set " +
                "       pwderrcnt = pwderrcnt + 1, " +
                "       updateid = @adminId, " +
                "       updateip = @ip, " +
                "       updatedt = now() " +
                "where adminid = @adminId";
            return MySql;
        }

        public string select_admin_by_groupcode()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "               a.adminid, a.adminname, a.adminpwd, a.groupcode, a.hp, a.authflag, a.pwderrcnt, a.createdt, " +
                    "               b.groupname, b.groupwrite, b.groupread " +
                    "from           admins as a " +
                    "inner join     admin_groups as b on a.groupcode = b.groupcode " +
                    "where          a.groupcode = @groupcode " +
                    "and            a.delflag = 'N'; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }
    }
}