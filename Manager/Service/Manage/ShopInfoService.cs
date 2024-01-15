using Manager.Common;
using Manager.Models.Manage;
using Manager.Query.Manage;
using Manager.Request.Manage;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Transactions;
using System.Web;

namespace Manager.Service.Manage
{
    public class ShopInfoService
    {
        private ShopInfoQuery shopInfoQuery;

        public ShopInfoService()
        {
            this.shopInfoQuery = new ShopInfoQuery();
        }

        public ShopInfoModel getShopInfo()
        {
            ShopInfoModel model = new ShopInfoModel();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(shopInfoQuery.select_shopinfo(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        model.standardprice = Convert.ToInt32(dr["standardprice"]);
                        model.deliveryprice = Convert.ToInt32(dr["deliveryprice"]);
                        model.returndeliveryprice = Convert.ToInt32(dr["returndeliveryprice"]);
                        model.changedeliveryprice = Convert.ToInt32(dr["changedeliveryprice"]);
                        model.foreignstandardprice = Convert.ToInt32(dr["foreignstandardprice"]);
                        model.foreigndeliveryprice = Convert.ToInt32(dr["foreigndeliveryprice"]);
                        model.foreignreturndeliveryprice = Convert.ToInt32(dr["foreignreturndeliveryprice"]);
                        model.foreignchangedeliveryprice = Convert.ToInt32(dr["foreignchangedeliveryprice"]);

                        model.rzipcode = dr["rzipcode"].ToString();
                        model.raddr1 = dr["raddr1"].ToString();
                        model.raddr2 = dr["raddr2"].ToString();

                        model.foreignrzipcode = dr["foreignrzipcode"].ToString();
                        model.foreignraddr1 = dr["foreignraddr1"].ToString();
                        model.foreignraddr2 = dr["foreignraddr2"].ToString();

                        model.txtreviewpoint = Convert.ToInt32(dr["txtreviewpoint"]);
                        model.imgreviewpoint = Convert.ToInt32(dr["imgreviewpoint"]);
                    }
                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public void updateShopInfo(ShopInfoRequest shopInfoRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(shopInfoQuery.update_shopinfo(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@standardprice", shopInfoRequest.standardprice));
                        cmd.Parameters.Add(new MySqlParameter("@deliveryprice", shopInfoRequest.deliveryprice));
                        cmd.Parameters.Add(new MySqlParameter("@returndeliveryprice", shopInfoRequest.returndeliveryprice));
                        cmd.Parameters.Add(new MySqlParameter("@changedeliveryprice", shopInfoRequest.changedeliveryprice));
                        cmd.Parameters.Add(new MySqlParameter("@foreignstandardprice", shopInfoRequest.foreignstandardprice));
                        cmd.Parameters.Add(new MySqlParameter("@foreigndeliveryprice", shopInfoRequest.foreigndeliveryprice));
                        cmd.Parameters.Add(new MySqlParameter("@foreignreturndeliveryprice", shopInfoRequest.foreignreturndeliveryprice));
                        cmd.Parameters.Add(new MySqlParameter("@foreignchangedeliveryprice", shopInfoRequest.foreignchangedeliveryprice));
                        cmd.Parameters.Add(new MySqlParameter("@rzipcode", shopInfoRequest.rzipcode));
                        cmd.Parameters.Add(new MySqlParameter("@raddr1", shopInfoRequest.raddr1));
                        cmd.Parameters.Add(new MySqlParameter("@raddr2", shopInfoRequest.raddr2));
                        cmd.Parameters.Add(new MySqlParameter("@foreignrzipcode", shopInfoRequest.foreignrzipcode));
                        cmd.Parameters.Add(new MySqlParameter("@foreignraddr1", shopInfoRequest.foreignraddr1));
                        cmd.Parameters.Add(new MySqlParameter("@foreignraddr2", shopInfoRequest.foreignraddr2));
                        cmd.Parameters.Add(new MySqlParameter("@txtreviewpoint", shopInfoRequest.txtreviewpoint));
                        cmd.Parameters.Add(new MySqlParameter("@imgreviewpoint", shopInfoRequest.imgreviewpoint));
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
    }
}