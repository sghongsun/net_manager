using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Query.Product
{
    public class CategoryQuery
    {
        public string select_category1_by_list_for_productcnt()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   a.categorycode1, a.categoryname1, a.displayflag, a.displaynum, " +
                    "                   IFNULL(COUNT(b.productcode), 0) as productcnt, " +
                    "                   IFNULL(SUM(case when b.salestate = 'Y' then 1 else 0 end), 0) as productsalecnt " +
                    "from               products_category1 as a " +
                    "left outer join    products as b on a.categorycode1 = b.categorycode1 " +
                    "group by           a.categorycode1, a.categoryname1, a.displayflag, a.displaynum " +
                    "order by           a.displaynum asc; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;            
        }

        public string select_category2_by_list_for_productcnt()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   a.categorycode1, a.categorycode2, a.categoryname2, a.displayflag, a.displaynum, " +
                    "                   IFNULL(COUNT(b.productcode), 0) as productcnt, " +
                    "                   IFNULL(SUM(case when b.salestate = 'Y' then 1 else 0 end), 0) as productsalecnt " +
                    "from               products_category2 as a " +
                    "left outer join    products as b on a.categorycode1 = b.categorycode1 and a.categorycode2 = b.categorycode2 " +
                    "group by           a.categorycode1, a.categorycode2, a.categoryname2, a.displayflag, a.displaynum " +
                    "order by           a.categorycode1 asc, a.displaynum asc; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category1_by_list()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, categoryname1, displayflag, displaynum " +
                    "from               products_category1 " +
                    "order by           displaynum asc; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category2_by_list()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, categorycode2, categoryname2, displayflag, displaynum " +
                    "from               products_category2 " +
                    "where              categorycode1 = @categorycode1 " +
                    "order by           categorycode1 asc, displaynum asc; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category1_for_max_code()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   IFNULL(MAX(categorycode1), 0) + 1 " +
                    "from               products_category1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category2_for_max_code()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   IFNULL(MAX(categorycode2), 0) + 1 " +
                    "from               products_category2 " +
                    "where              categorycode1 = @categorycode1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category1_for_max_displaynum()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   IFNULL(MAX(displaynum), 0) + 1 " +
                    "from               products_category1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category2_for_max_displaynum()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   IFNULL(MAX(displaynum), 0) + 1 " +
                    "from               products_category2 " +
                    "where              categorycode1 = @categorycode1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string insert_category1()
        {
            string MySql = "" +
                    "insert into products_category1 ( " +
                    "                   categorycode1, " +
                    "                   categoryname1, " +
                    "                   displayflag, " +
                    "                   displaynum, " +
                    "                   createid, " +
                    "                   createip " +
                    ") values ( " +
                    "                   @categorycode1, " +
                    "                   @categoryname1, " +
                    "                   @displayflag, " +
                    "                   @displaynum, " +
                    "                   @id, " +
                    "                   @ip " +
                    ");";
            return MySql;
        }

        public string insert_category2()
        {
            string MySql = "" +
                    "insert into products_category2 ( " +
                    "                   categorycode1, " +
                    "                   categorycode2, " +
                    "                   categoryname2, " +
                    "                   displayflag, " +
                    "                   displaynum, " +
                    "                   createid, " +
                    "                   createip " +
                    ") values ( " +
                    "                   @categorycode1, " +
                    "                   @categorycode2, " +
                    "                   @categoryname2, " +
                    "                   @displayflag, " +
                    "                   @displaynum, " +
                    "                   @id, " +
                    "                   @ip " +
                    ");";
            return MySql;
        }

        public string select_category1_by_code()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, categoryname1, displayflag, displaynum " +
                    "from               products_category1 " +
                    "where              categorycode1 = @categorycode1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category2_by_code()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, categorycode2, categoryname2, displayflag, displaynum " +
                    "from               products_category2 " +
                    "where              categorycode1 = @categorycode1 " +
                    "and                categorycode2 = @categorycode2; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string update_category1()
        {
            string MySql = "" +
                    "update products_category1 set " +
                    "       categoryname1 = @categoryname1, " +
                    "       displayflag = @displayflag, " +
                    "       updateid = @id, " +
                    "       updateip = @ip " +
                    "where categorycode1 = @categorycode1;";
            return MySql;
        }

        public string update_category2()
        {
            string MySql = "" +
                    "update products_category2 set " +
                    "       categoryname2 = @categoryname2, " +
                    "       displayflag = @displayflag, " +
                    "       updateid = @id, " +
                    "       updateip = @ip " +
                    "where  categorycode1 = @categorycode1 " +
                    "and    categorycode2 = @categorycode2";
            return MySql;
        }

        public string select_category1_for_diplaynum_up()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, displaynum " +
                    "from               products_category1 " +
                    "where              displaynum < @displaynum " +
                    "order by           displaynum desc " +
                    "limit              1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category1_for_diplaynum_down()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, displaynum " +
                    "from               products_category1 " +
                    "where              displaynum > @displaynum " +
                    "order by           displaynum asc " +
                    "limit              1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category2_for_diplaynum_up()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, categorycode2, displaynum " +
                    "from               products_category2 " +
                    "where              categorycode1 = @categorycode1 " +
                    "and                displaynum < @displaynum " +
                    "order by           displaynum desc " +
                    "limit              1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string select_category2_for_diplaynum_down()
        {
            string MySql = "" +
                    "set session transaction isolation level read uncommitted; " +
                    "select " +
                    "                   categorycode1, categorycode2, displaynum " +
                    "from               products_category2 " +
                    "where              categorycode1 = @categorycode1 " +
                    "and                displaynum > @displaynum " +
                    "order by           displaynum asc " +
                    "limit              1; " +
                    "set session transaction isolation level repeatable read;";
            return MySql;
        }

        public string update_category1_for_displaynum()
        {
            string MySql = "" +
                    "update products_category1 set " +
                    "       displaynum = @displaynum, " +
                    "       updateid = @id, " +
                    "       updateip = @ip " +
                    "where categorycode1 = @categorycode1;";
            return MySql;
        }

        public string update_category2_for_displaynum()
        {
            string MySql = "" +
                    "update products_category2 set " +
                    "       displaynum = @displaynum, " +
                    "       updateid = @id, " +
                    "       updateip = @ip " +
                    "where  categorycode1 = @categorycode1 " +
                    "and    categorycode2 = @categorycode2";
            return MySql;
        }
    }
}