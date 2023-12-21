using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using Manager.Common;
using Manager.Query.Manage;
using Manager.Request.Manage;
using MySql.Data.MySqlClient;

namespace Manager.Service.Manage
{
    public class MyMenuChoiceService
    {
        private MyMenuChoiceQuery myMenuChoiceQuery;

        public MyMenuChoiceService() 
        { 
            this.myMenuChoiceQuery = new MyMenuChoiceQuery();
        }

        public void delete_MenuChoiceMenuCodeAll(string  menuCode)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(myMenuChoiceQuery.delete_admin_menu_choice_by_menucode_For_All(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@menucode", menuCode));

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