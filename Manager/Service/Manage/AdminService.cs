using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using Manager.Common;
using Manager.Models.Manage;
using Manager.Query.Manage;
using Manager.Request.Manage;
using Manager.Request;
using MySqlX.XDevAPI.Common;
using System.Security.Cryptography;
using System.Diagnostics.Metrics;

namespace Manager.Service.Manage
{
    public class AdminService
    {
        private AdminQuery adminQuery;
        private MyMenuChoiceService myMenuChoiceService;

        public AdminService() { 
            this.adminQuery = new AdminQuery();
            this.myMenuChoiceService = new MyMenuChoiceService();
        }

        public string LoginProc(LoginRequest loginRequest)
        {           
            string adminid;
            string adminname;
            string adminpwd;
            string groupcode;
            string authflag;
            int pwderrcnt;
            string hp;
            string groupwrite;
            string groupread;

            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(adminQuery.select_by_adminId_for_group(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@adminId", loginRequest.mid));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        adminid = dr["adminid"].ToString();
                        adminname = dr["adminname"].ToString();
                        adminpwd = dr["adminpwd"].ToString();
                        groupcode = dr["groupcode"].ToString();
                        authflag = dr["authflag"].ToString();
                        pwderrcnt = Convert.ToInt32(dr["pwderrcnt"].ToString()) + 1;
                        hp = dr["hp"].ToString();
                        groupwrite = dr["groupwrite"].ToString();
                        groupread = dr["groupread"].ToString();
                    } else
                    {
                        dr.Close();
                        dbcon.Close();
                        dbcon.Dispose();

                        return "아이디와 비밀번호를 다시 확인 하여 주세요.";
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            if (!SHA.Sha512_Encrypt(loginRequest.mpwd).Equals(adminpwd))
            {
                if (pwderrcnt <= 5)
                {
                    LoginForPwdErrorUpdate(loginRequest.mid);
                    return "비밀번호가 " + pwderrcnt + "번 틀렸습니다. 5번 틀리면 계정이 잠깁니다.";
                } else
                {
                    return "비밀번호가 5번 틀렸습니다. 관리자에게 문의 하시기 바랍니다.";
                }
            }

            Func.SetCookie("adminid", adminid);
            Func.SetCookie("adminname", adminname);
            Func.SetCookie("admingroup", groupcode);
            Func.SetCookie("menuchoice", myMenuChoiceService.getMenuChoice(loginRequest.mid));
            Func.SetCookie("ip", Func.GetUserIP());            

            if (!loginRequest.saveid.IsEmpty() && loginRequest.saveid.Equals("Y"))
            {
                Func.SetCookie("saveid", loginRequest.saveid, 365);
                Func.SetCookie("saveadminid", loginRequest.mid, 365);
            } else
            {
                Func.SetCookie("saveid", "", -1);
                Func.SetCookie("saveadminid", "", -1);
            }
            
            LoginSuccess(loginRequest.mid);
            return "OK";
        }
        
        public void LoginForPwdErrorUpdate(string mid)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();
                    
                    using (var cmd = new MySqlCommand(adminQuery.update_for_login_fail(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", mid));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetUserIP()));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }            
        }

        public void LoginSuccess(string mid)
        {
            using (TransactionScope ts = new TransactionScope ())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(adminQuery.update_for_login_success(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", mid));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetUserIP()));

                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new MySqlCommand(adminQuery.login_insert(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", mid));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetUserIP()));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public List<AdminModel> getAdminListByGroupCode(int groupCode)
        {
            List<AdminModel> adminList = new List<AdminModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(adminQuery.select_admin_by_groupcode(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@groupcode", groupCode));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    
                    while (dr.Read())
                    {
                        AdminModel admin = new AdminModel();
                        admin.adminid = dr["adminid"].ToString();
                        admin.adminname = dr["adminname"].ToString() ;
                        admin.groupname = dr["groupname"].ToString();
                        admin.hp = dr["hp"].ToString();
                        admin.createdt = Convert.ToDateTime(dr["createdt"].ToString());
                        adminList.Add(admin);
                    }                     

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return adminList;
        }

        public AdminSearchModel getAdminList(AdminSearch adminSearch)
        {            
            string where = "";            
            List<SearchParam> searchParam = new List<SearchParam>();
            if (!adminSearch.keyword.IsEmpty())
            {
                if (adminSearch.searchtype.Equals("adminname"))
                {
                    where += "and adminname LIKE CONCAT('%', @keyword, '%') ";
                    searchParam.Add(new SearchParam() { name = "@keyword", value = adminSearch.keyword });
                }
                else if (adminSearch.searchtype.Equals("adminid"))
                {
                    where += "and adminid LIKE CONCAT('%', @keyword, '%') ";
                    searchParam.Add(new SearchParam() { name = "@keyword", value = adminSearch.keyword });
                }
            }

            if (!adminSearch.groupcode.IsEmpty())
            {
                where += "and groupcode = @groupcode ";
                searchParam.Add(new SearchParam() { name = "@groupcode", value = adminSearch.groupcode });
            }

            int count = 0;
            Pagination pagination;
            List<AdminModel> adminList = new List<AdminModel>();            
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                string query = "";
                query = adminQuery.select_admin_by_list_for_totalcount().Replace("#where", where);

                using (var cmd = new MySqlCommand(query, dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    foreach (var param in searchParam) {
                        cmd.Parameters.Add(new MySqlParameter(param.name, param.value));
                    }

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        count = Convert.ToInt32(dr["totalcount"]);
                    }

                    dr.Close();
                }

                if (count < 0) 
                {
                    dbcon.Close();
                    dbcon.Dispose();

                    return null;
                }

                pagination = new Pagination(count, adminSearch);
                
                query = adminQuery.select_admin_by_list().Replace("#where", where);

                using (var cmd = new MySqlCommand(query, dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    foreach (var param in searchParam)
                    {
                        cmd.Parameters.Add(new MySqlParameter(param.name, param.value));
                    }
                    cmd.Parameters.Add(new MySqlParameter("@startlimit", pagination.limitStart));
                    cmd.Parameters.Add(new MySqlParameter("@endlimit", adminSearch.recordsize));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {                        
                        AdminModel adminModel = new AdminModel();
                        adminModel.adminid = dr["adminid"].ToString();
                        adminModel.adminname = dr["adminname"].ToString();
                        adminModel.adminpwd = dr["adminpwd"].ToString();
                        adminModel.groupcode = Convert.ToInt32(dr["groupcode"]);
                        adminModel.hp = dr["hp"].ToString();
                        adminModel.authflag = dr["authflag"].ToString();
                        adminModel.pwderrcnt = Convert.ToInt32(dr["pwderrcnt"]);
                        adminModel.createdt = Convert.ToDateTime(dr["createdt"]);
                        adminList.Add(adminModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            AdminSearchModel searchModel = new AdminSearchModel() {
                pagination = pagination,
                AdminList = adminList
            };
            
            return searchModel;
        }

        public string AdminID_DuplicationCheck(AdminDuplicationRequest adminDuplicationRequest)
        {
            string result = "OK";
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(adminQuery.select_admin_by_adminid_for_count(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@adminid", adminDuplicationRequest.adminid));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (Convert.ToInt32(dr["admincount"]) > 0)
                        {
                            result = "EXISTS";
                        }
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return result;
        }

        public string insert_Admin(AdminAddRequest adminAddRequest)
        {
            string result = "OK";

            if (!adminAddRequest.adminid.Equals(adminAddRequest.checkid))
            {
                return "중복체크 한 아이디가 상이 합니다.";
            }

            if (!adminAddRequest.pwd.Equals(adminAddRequest.pwd1)) 
            {
                return "비밀번호가 일치 하지 않습니다.";
            }

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(adminQuery.insert_admin(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", adminAddRequest.adminid));
                        cmd.Parameters.Add(new MySqlParameter("@groupcode", adminAddRequest.groupcode));
                        cmd.Parameters.Add(new MySqlParameter("@adminname", adminAddRequest.name));
                        cmd.Parameters.Add(new MySqlParameter("@adminpwd", SHA.Sha512_Encrypt(adminAddRequest.pwd)));
                        cmd.Parameters.Add(new MySqlParameter("@hp", adminAddRequest.hp1 + "-" + adminAddRequest.hp2 + "-" + adminAddRequest.hp3));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }

            return result;
        }

        public AdminModel getAdminIfo(string adminId)
        {
            AdminModel adminModel = new AdminModel();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(adminQuery.select_admin_by_adminid(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@adminid", adminId));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        adminModel.adminid = dr["adminid"].ToString();
                        adminModel.adminname = dr["adminname"].ToString();
                        adminModel.groupcode = Convert.ToInt32(dr["groupcode"]);
                        adminModel.hp = dr["hp"].ToString();
                        adminModel.authflag = dr["authflag"].ToString();
                        adminModel.pwderrcnt = Convert.ToInt32(dr["pwderrcnt"]);
                        adminModel.createid = dr["createid"].ToString();
                        adminModel.createip = dr["createip"].ToString();
                        adminModel.createdt = Convert.ToDateTime(dr["createdt"]);
                        adminModel.updateid = dr["updateid"].ToString();
                        adminModel.updateip = dr["updateip"].ToString();
                        if (!adminModel.updateid.IsEmpty()) 
                        {
                            adminModel.updatedt = Convert.ToDateTime(dr["updatedt"]);
                        }                        
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return adminModel;
        }

        public string updateAdminGroup(AdminModifyGroupRequest adminModifyGroupRequest)
        {
            string[] AdminId = adminModifyGroupRequest.adminid.Split(',');
            string[] GroupCode = adminModifyGroupRequest.groupcode.Split(',');
            if (AdminId.Length != GroupCode.Length)
            {
                return "FAIL|||||값이 상이 합니다.";
            }

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    for (int i = 0; i < AdminId.Length; i++)
                    {
                        using (var cmd = new MySqlCommand(adminQuery.update_admin_for_groupcode(), dbcon))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new MySqlParameter("@adminId", AdminId[i]));
                            cmd.Parameters.Add(new MySqlParameter("@groupcode", Convert.ToInt32(GroupCode[i])));
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

            return "OK|||||";
        }

        public string updateAdminPwd(AdminPwdModifyRequest adminPwdModifyRequest)
        {
            if (!adminPwdModifyRequest.pwd.Equals(adminPwdModifyRequest.pwd1))
            {
                return "FAIL|||||비밀번호가 상이 합니다.";
            }

            AdminModel adminModel = getAdminIfo(adminPwdModifyRequest.adminid);
            if (adminModel == null)
            {
                return "FAIL|||||정보가 없습니다.";
            }

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();
                    
                    using (var cmd = new MySqlCommand(adminQuery.update_admin_for_pwd(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", adminPwdModifyRequest.adminid));
                        cmd.Parameters.Add(new MySqlParameter("@adminpwd", SHA.Sha512_Encrypt(adminPwdModifyRequest.pwd)));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }                    

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }

            return "OK|||||";
        }

        public string updateAdminHp(AdminHpModifyRequest adminHpModifyRequest)
        {            
            AdminModel adminModel = getAdminIfo(adminHpModifyRequest.adminid);
            if (adminModel == null)
            {
                return "정보가 없습니다.";
            }

            if (adminModel.hp.Equals(adminHpModifyRequest.hp1+"-"+adminHpModifyRequest.hp2+"-"+adminHpModifyRequest.hp3))
            {
                return "현재 핸드폰 번호와 동일합니다..";
            }

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(adminQuery.update_admin_for_hp(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", adminHpModifyRequest.adminid));
                        cmd.Parameters.Add(new MySqlParameter("@hp", adminHpModifyRequest.hp1 + "-" + adminHpModifyRequest.hp2 + "-" + adminHpModifyRequest.hp3));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }

            return "OK";
        }

        public string updateAdminInfo(AdminInfoModifyRequest adminInfoModifyRequest)
        {
            AdminModel adminModel = getAdminIfo(adminInfoModifyRequest.adminid);
            if (adminModel == null)
            {
                return "정보가 없습니다.";
            }

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(adminQuery.update_admin_for_groupcode_adminname(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", adminInfoModifyRequest.adminid));
                        cmd.Parameters.Add(new MySqlParameter("@groupcode", adminInfoModifyRequest.groupcode));
                        cmd.Parameters.Add(new MySqlParameter("@adminname", adminInfoModifyRequest.name));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }

            return "OK";
        }

        public string deleteAdminInfo(AdminDeleteRequest adminDeleteRequest)
        {
            AdminModel adminModel = getAdminIfo(adminDeleteRequest.adminid);
            if (adminModel == null)
            {
                return "정보가 없습니다.";
            }

            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(adminQuery.update_admin_for_delflag(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@adminId", adminDeleteRequest.adminid));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }

            return "OK";
        }

        public AdminLoginListModel getLoginList(string adminId, Search search)
        {
            int count = 0;
            Pagination pagination;
            List<AdminLoginModel> adminLoginList = new List<AdminLoginModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(adminQuery.select_admin_login_by_list_for_totalcount(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@adminid", adminId));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        count = Convert.ToInt32(dr["totalcount"]);
                    }

                    dr.Close();
                }

                if (count < 0)
                {
                    dbcon.Close();
                    dbcon.Dispose();

                    return null;
                }

                pagination = new Pagination(count, search);
                search.pagination = pagination;

                using (var cmd = new MySqlCommand(adminQuery.select_admin_login_by_list(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@adminid", adminId));
                    cmd.Parameters.Add(new MySqlParameter("@startlimit", pagination.limitStart));
                    cmd.Parameters.Add(new MySqlParameter("@endlimit", search.recordsize));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminLoginModel adminLoginModel = new AdminLoginModel();
                        adminLoginModel.adminid = dr["adminid"].ToString();
                        adminLoginModel.ip = dr["ip"].ToString();
                        adminLoginModel.logindt = Convert.ToDateTime(dr["logindt"]);
                        adminLoginList.Add(adminLoginModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            AdminLoginListModel model = new AdminLoginListModel()
            {
                AdminLoginList = adminLoginList,
                Search = search
            };

            return model;
        }

        public List<AdminModel> getListForGroupSearch(AdminMenuAuthRequest adminMenuAuthRequest)
        {
            List<AdminModel> adminList = new List<AdminModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();
                
                using (var cmd = new MySqlCommand(adminQuery.select_admin_by_list_for_group_search(adminMenuAuthRequest.authType), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@MCode2", adminMenuAuthRequest.MCode2));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminModel adminModel = new AdminModel();
                        adminModel.adminid = dr["adminid"].ToString();
                        adminModel.adminname = dr["adminname"].ToString();
                        adminModel.hp = dr["hp"].ToString();
                        adminModel.groupname = dr["groupname"].ToString();
                        adminModel.createdt = Convert.ToDateTime(dr["createdt"]);
                        adminList.Add(adminModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            return adminList;
        }
    }
}