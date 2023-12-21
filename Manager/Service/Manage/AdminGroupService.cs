using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Manager.Common;
using System.Web.DynamicData;
using Manager.Models.Manage;
using Manager.Query.Manage;
using MySql.Data.MySqlClient;

namespace Manager.Service.Manage
{
    public class AdminGroupService
    {
        private AdminGroupQuery adminGroupQuery;

        public AdminGroupService()
        {
            this.adminGroupQuery = new AdminGroupQuery();
        }

        public AdminGroupModel getInfoByGroupCode(int groupCode)
        {
            AdminGroupModel adminGroupModel = new AdminGroupModel();

            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(adminGroupQuery.select_admin_group_by_groupcode(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@groupcode", groupCode));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        adminGroupModel.groupcode = Convert.ToInt32(dr["groupcode"].ToString());
                        adminGroupModel.groupname = dr["groupname"].ToString();
                        adminGroupModel.groupdesc = dr["groupdesc"].ToString();
                        adminGroupModel.groupwrite = dr["groupwrite"].ToString();
                        adminGroupModel.groupread = dr["groupread"].ToString();
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return adminGroupModel;
        }

        public List<AdminGroupModel> getList()
        {
            List<AdminGroupModel> adminGroupModelList = new List<AdminGroupModel>();

            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(adminGroupQuery.select_admin_group_by_list(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;                    

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminGroupModel adminGroupModel = new AdminGroupModel();
                        adminGroupModel.groupcode = Convert.ToInt32(dr["groupcode"].ToString());
                        adminGroupModel.groupname = dr["groupname"].ToString();
                        adminGroupModel.groupdesc = dr["groupdesc"].ToString();
                        adminGroupModel.admincnt = Convert.ToInt32(dr["admincnt"].ToString());                        
                        adminGroupModelList.Add(adminGroupModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return adminGroupModelList;
        }

    }
}