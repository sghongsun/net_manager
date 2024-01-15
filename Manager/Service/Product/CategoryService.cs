using Manager.Common;
using Manager.Models.Product;
using Manager.Query.Manage;
using Manager.Query.Product;
using Manager.Request.Manage;
using Manager.Request.Product;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.WebPages;

namespace Manager.Service.Product
{
    public class CategoryService
    {
        private CategoryQuery categoryQuery;

        public CategoryService() 
        {
            this.categoryQuery = new CategoryQuery();
        }

        public List<CategoryModel> getCategory1ListInProductCnt()
        {
            List<CategoryModel> model = new List<CategoryModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(categoryQuery.select_category1_by_list_for_productcnt(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        CategoryModel categoryModel = new CategoryModel();
                        categoryModel.categorycode1 = dr["categorycode1"].ToString();
                        categoryModel.categoryname1 = dr["categoryname1"].ToString();
                        categoryModel.displayflag = dr["displayflag"].ToString();
                        categoryModel.displaynum = Convert.ToInt32(dr["displaynum"]);
                        categoryModel.productcnt = Convert.ToInt32(dr["productcnt"]);
                        categoryModel.productsalecnt = Convert.ToInt32(dr["productsalecnt"]);
                        model.Add(categoryModel);                        
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public List<CategoryModel> getCategory2ListInProductCnt()
        {
            List<CategoryModel> model = new List<CategoryModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(categoryQuery.select_category2_by_list_for_productcnt(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        CategoryModel categoryModel = new CategoryModel();
                        categoryModel.categorycode1 = dr["categorycode1"].ToString();
                        categoryModel.categorycode2 = dr["categorycode2"].ToString();
                        categoryModel.categoryname2 = dr["categoryname2"].ToString();
                        categoryModel.displayflag = dr["displayflag"].ToString();
                        categoryModel.displaynum = Convert.ToInt32(dr["displaynum"]);
                        categoryModel.productcnt = Convert.ToInt32(dr["productcnt"]);
                        categoryModel.productsalecnt = Convert.ToInt32(dr["productsalecnt"]);
                        model.Add(categoryModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public List<CategoryModel> getCategory1List()
        {
            List<CategoryModel> model = new List<CategoryModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(categoryQuery.select_category1_by_list(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        CategoryModel categoryModel = new CategoryModel();
                        categoryModel.categorycode1 = dr["categorycode1"].ToString();
                        categoryModel.categoryname1 = dr["categoryname1"].ToString();
                        categoryModel.displayflag = dr["displayflag"].ToString();
                        categoryModel.displaynum = Convert.ToInt32(dr["displaynum"]);
                        model.Add(categoryModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public List<CategoryModel> getCategory2List(string categorycode1)
        {
            List<CategoryModel> model = new List<CategoryModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(categoryQuery.select_category2_by_list(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@categorycode1", categorycode1));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        CategoryModel categoryModel = new CategoryModel();
                        categoryModel.categorycode1 = dr["categorycode1"].ToString();
                        categoryModel.categorycode2 = dr["categorycode2"].ToString();
                        categoryModel.categoryname2 = dr["categoryname2"].ToString();
                        categoryModel.displayflag = dr["displayflag"].ToString();
                        categoryModel.displaynum = Convert.ToInt32(dr["displaynum"]);
                        model.Add(categoryModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public void insertCategory1(Category1AddRequest category1AddRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    string categoryCode1 = "";
                    using (var cmd = new MySqlCommand(categoryQuery.select_category1_for_max_code(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;

                        MySqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            categoryCode1 = Func.MaketoZero(dr[0].ToString(), 2);
                        }
                        dr.Close();
                    }

                    int displayNum = 0;
                    using (var cmd = new MySqlCommand(categoryQuery.select_category1_for_max_displaynum(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;

                        MySqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            displayNum = Convert.ToInt32(dr[0]);
                        }
                        dr.Close();
                    }

                    using (var cmd = new MySqlCommand(categoryQuery.insert_category1(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@categorycode1", categoryCode1));
                        cmd.Parameters.Add(new MySqlParameter("@categoryname1", category1AddRequest.categoryName));
                        cmd.Parameters.Add(new MySqlParameter("@displayflag", category1AddRequest.displayFlag));
                        cmd.Parameters.Add(new MySqlParameter("@displaynum", displayNum));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public void insertCategory2(Category2AddRequest category2AddRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    string categoryCode2 = "";
                    using (var cmd = new MySqlCommand(categoryQuery.select_category2_for_max_code(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@categorycode1", category2AddRequest.categoryCode1));

                        MySqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            categoryCode2 = Func.MaketoZero(dr[0].ToString(), 2);
                        }
                        dr.Close();
                    }

                    int displayNum = 0;
                    using (var cmd = new MySqlCommand(categoryQuery.select_category2_for_max_displaynum(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@categorycode1", category2AddRequest.categoryCode1));

                        MySqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            displayNum = Convert.ToInt32(dr[0]);
                        }
                        dr.Close();
                    }

                    using (var cmd = new MySqlCommand(categoryQuery.insert_category2(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@categorycode1", category2AddRequest.categoryCode1));
                        cmd.Parameters.Add(new MySqlParameter("@categorycode2", categoryCode2));
                        cmd.Parameters.Add(new MySqlParameter("@categoryname2", category2AddRequest.categoryName));
                        cmd.Parameters.Add(new MySqlParameter("@displayflag", category2AddRequest.displayFlag));
                        cmd.Parameters.Add(new MySqlParameter("@displaynum", displayNum));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public CategoryModel getCategory1Info(string categoryCode1)
        {
            CategoryModel model = new CategoryModel();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(categoryQuery.select_category1_by_code(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@categorycode1", categoryCode1));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        model.categorycode1 = dr["categorycode1"].ToString();
                        model.categoryname1 = dr["categoryname1"].ToString();
                        model.displayflag = dr["displayflag"].ToString();
                        model.displaynum = Convert.ToInt32(dr["displaynum"]);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public CategoryModel getCategory2Info(string categoryCode1, string categoryCode2)
        {
            CategoryModel model = new CategoryModel();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(categoryQuery.select_category2_by_code(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@categorycode1", categoryCode1));
                    cmd.Parameters.Add(new MySqlParameter("@categorycode2", categoryCode2));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        model.categorycode1 = dr["categorycode1"].ToString();
                        model.categorycode2 = dr["categorycode2"].ToString();
                        model.categoryname2 = dr["categoryname2"].ToString();
                        model.displayflag = dr["displayflag"].ToString();
                        model.displaynum = Convert.ToInt32(dr["displaynum"]);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public void updateCategory1(Category1ModifyRequest category1ModifyRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(categoryQuery.update_category1(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@categorycode1", category1ModifyRequest.categoryCode1));
                        cmd.Parameters.Add(new MySqlParameter("@categoryname1", category1ModifyRequest.categoryName));
                        cmd.Parameters.Add(new MySqlParameter("@displayflag", category1ModifyRequest.displayFlag));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public void updateCategory2(Category2ModifyRequest category2ModifyRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();
                    
                    using (var cmd = new MySqlCommand(categoryQuery.update_category2(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@categorycode1", category2ModifyRequest.categoryCode1));
                        cmd.Parameters.Add(new MySqlParameter("@categorycode2", category2ModifyRequest.categoryCode2));
                        cmd.Parameters.Add(new MySqlParameter("@categoryname2", category2ModifyRequest.categoryName));
                        cmd.Parameters.Add(new MySqlParameter("@displayflag", category2ModifyRequest.displayFlag));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public void updateCategory1DiplayNum(Category1DisplayNumModifyRequest category1DisplayNumModifyRequest)
        {
            CategoryModel nowModel = getCategory1Info(category1DisplayNumModifyRequest.categoryCode1);

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    CategoryModel chgModel = new CategoryModel();
                    string query = "";
                    switch (category1DisplayNumModifyRequest.modType)
                    {
                        case "up":
                            query = categoryQuery.select_category1_for_diplaynum_up();
                            break;
                        case "down":
                            query = categoryQuery.select_category1_for_diplaynum_down();
                            break;
                    }

                    using (var cmd = new MySqlCommand(query, dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@displaynum", nowModel.displaynum));

                        MySqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            chgModel.categorycode1 = dr["categorycode1"].ToString();
                            chgModel.displaynum = Convert.ToInt32(dr["displaynum"]);
                        }

                        dr.Close();
                    }

                    if (!chgModel.categorycode1.IsEmpty())
                    {
                        using (var cmd = new MySqlCommand(categoryQuery.update_category1_for_displaynum(), dbcon))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new MySqlParameter("@categorycode1", nowModel.categorycode1));
                            cmd.Parameters.Add(new MySqlParameter("@displaynum", chgModel.displaynum));
                            cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                            cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new MySqlCommand(categoryQuery.update_category1_for_displaynum(), dbcon))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new MySqlParameter("@categorycode1", chgModel.categorycode1));
                            cmd.Parameters.Add(new MySqlParameter("@displaynum", nowModel.displaynum));
                            cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                            cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                            cmd.ExecuteNonQuery();
                        }

                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public void updateCategory2DiplayNum(Category2DisplayNumModifyRequest category2DisplayNumModifyRequest)
        {
            CategoryModel nowModel = getCategory2Info(category2DisplayNumModifyRequest.categoryCode1, category2DisplayNumModifyRequest.categoryCode2);

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();                   

                    CategoryModel chgModel = new CategoryModel();
                    string query = "";
                    switch (category2DisplayNumModifyRequest.modType)
                    {
                        case "up":
                            query = categoryQuery.select_category2_for_diplaynum_up();
                            break;
                        case "down":
                            query = categoryQuery.select_category2_for_diplaynum_down();
                            break;
                    }

                    using (var cmd = new MySqlCommand(query, dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@categorycode1", nowModel.categorycode1));
                        cmd.Parameters.Add(new MySqlParameter("@displaynum", nowModel.displaynum));

                        MySqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            chgModel.categorycode1 = dr["categorycode1"].ToString();
                            chgModel.categorycode2 = dr["categorycode2"].ToString();
                            chgModel.displaynum = Convert.ToInt32(dr["displaynum"]);
                        }

                        dr.Close();
                    }

                    if (!chgModel.categorycode1.IsEmpty())
                    {
                        using (var cmd = new MySqlCommand(categoryQuery.update_category2_for_displaynum(), dbcon))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new MySqlParameter("@categorycode1", nowModel.categorycode1));
                            cmd.Parameters.Add(new MySqlParameter("@categorycode2", nowModel.categorycode2));
                            cmd.Parameters.Add(new MySqlParameter("@displaynum", chgModel.displaynum));
                            cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                            cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new MySqlCommand(categoryQuery.update_category2_for_displaynum(), dbcon))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new MySqlParameter("@categorycode1", chgModel.categorycode1));
                            cmd.Parameters.Add(new MySqlParameter("@categorycode2", chgModel.categorycode2));
                            cmd.Parameters.Add(new MySqlParameter("@displaynum", nowModel.displaynum));
                            cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                            cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                            cmd.ExecuteNonQuery();
                        }
                    }
                    
                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }
    }    
}