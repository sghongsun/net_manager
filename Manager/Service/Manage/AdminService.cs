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
using System.Security.Cryptography;

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

        public string LoginProc()
        {            
            string mid = Func.getRequestFormToString("mid");
            string mpwd = Func.getRequestFormToString("mpwd");
            string saveid = Func.getRequestFormToString("saveid");

            if (mid.IsEmpty() || mpwd.IsEmpty())
            {
                return "아이디와 비밀번호를 입력하여 주세요. ";
            }

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
                    cmd.Parameters.Add(new MySqlParameter("@adminId", mid));

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

            if (!SHA.Sha512_Encrypt(mpwd).Equals(adminpwd))
            {
                if (pwderrcnt <= 5)
                {
                    LoginForPwdErrorUpdate(mid);
                    return "비밀번호가 " + pwderrcnt + "번 틀렸습니다. 5번 틀리면 계정이 잠깁니다.";
                } else
                {
                    return "비밀번호가 5번 틀렸습니다. 관리자에게 문의 하시기 바랍니다.";
                }
            }

            Func.SetCookie("adminid", adminid);
            Func.SetCookie("adminname", adminname);
            Func.SetCookie("admingroup", groupcode);
            Func.SetCookie("menuchoice", myMenuChoiceService.getMenuChoice(mid));
            Func.SetCookie("ip", Func.GetUserIP());            

            if (saveid.Equals("Y"))
            {
                Func.SetCookie("saveid", saveid, 365);
                Func.SetCookie("saveadminid", mid, 365);
            } else
            {
                Func.SetCookie("saveid", "", -1);
                Func.SetCookie("saveadminid", "", -1);
            }

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
    }
}