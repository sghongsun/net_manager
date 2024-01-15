using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Query.Manage
{
    public class ShopInfoQuery
    {
        public string select_shopinfo() 
        {
            string MySql = "" +
                "set session transaction isolation level read uncommitted; " +
                "select " +
                "               standardprice, " +
                "               deliveryprice, " +
                "               returndeliveryprice, " +
                "               changedeliveryprice, " +
                "               foreignstandardprice, " +
                "               foreigndeliveryprice, " +
                "               foreignreturndeliveryprice, " +
                "               foreignchangedeliveryprice, " +
                "               rzipcode, " +
                "               raddr1, " +
                "               raddr2, " +
                "               foreignrzipcode, " +
                "               foreignraddr1, " +
                "               foreignraddr2, " +
                "               txtreviewpoint, " +
                "               imgreviewpoint " +
                "from           shopinfos; " +
                "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string update_shopinfo()
        {
            string MySql = "" +
                "update shopinfos set " +
                "               standardprice = @standardprice, " +
                "               deliveryprice = @deliveryprice, " +
                "               returndeliveryprice = @returndeliveryprice, " +
                "               changedeliveryprice = @changedeliveryprice, " +
                "               foreignstandardprice = @foreignstandardprice, " +
                "               foreigndeliveryprice = @foreigndeliveryprice, " +
                "               foreignreturndeliveryprice = @foreignreturndeliveryprice, " +
                "               foreignchangedeliveryprice = @foreignchangedeliveryprice, " +
                "               rzipcode = @rzipcode, " +
                "               raddr1 = @raddr1, " +
                "               raddr2 = @raddr2, " +
                "               foreignrzipcode = @foreignrzipcode, " +
                "               foreignraddr1 = @foreignraddr1, " +
                "               foreignraddr2 = @foreignraddr2, " +
                "               txtreviewpoint = @txtreviewpoint, " +
                "               imgreviewpoint = @imgreviewpoint," +
                "               updateid = @id, " +
                "               updateip = @ip;";
            return MySql;
        }
    }
}