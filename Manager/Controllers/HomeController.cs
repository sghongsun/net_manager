using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI;
using Manager.Common;
using Manager.Models.Auth;

namespace Manager.Controllers
{    
    public class HomeController : Controller
    {
        [AuthorizeFilter]
        [HttpGet]
        public ActionResult Index()
        {
            return View();            
        }
        
     
        public string test()
        {
            using (SqlConnection dbcon = new SqlConnection(SetInfo.dbconn))
            {
                dbcon.Open();
                /*
                List<CompanyModel> companyModel = new List<CompanyModel>();
                using (var cmd = new SqlCommand("USP_Admin_TB_Company_Select", dbcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Page", SqlDbType.Int)).Value = Page;
                    cmd.Parameters.Add(new SqlParameter("@Page_Size", SqlDbType.Int)).Value = PageSize;
                    cmd.Parameters.Add(new SqlParameter("@WQUERY", SqlDbType.VarChar)).Value = wQuery;
                    cmd.Parameters.Add(new SqlParameter("@SQUERY", SqlDbType.VarChar)).Value = sQuery;

                    SqlDataReader oDR = cmd.ExecuteReader();

                    if (oDR.Read())
                    {
                        try
                        {
                            TotalCount = Convert.ToInt32(oDR[0].ToString());
                        }
                        catch
                        {
                            TotalCount = 0;
                        }
                    }

                    oDR.NextResult();

                    while (oDR.Read())
                    {
                        CompanyModel company = new CompanyModel();
                        company.IDX = Convert.ToInt32(oDR["Idx"].ToString());
                        company.CompanyID = oDR["CompanyID"].ToString();
                        company.CompanyName = oDR["CompanyName"].ToString();
                        company.CreateDT = Convert.ToDateTime(oDR["CreateDT"].ToString());
                        companyModel.Add(company);
                    }

                    oDR.Close();
                }
                

                using (var cmd = new SqlCommand("USP_Admin_TB_Company_Insert", dbcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.VarChar)).Value = CompanyID;
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.VarChar)).Value = CompanyName;
                    cmd.Parameters.Add(new SqlParameter("@CreateID", SqlDbType.VarChar)).Value = fnc.GetCookie("MID");
                    cmd.Parameters.Add(new SqlParameter("@CreateIP", SqlDbType.VarChar)).Value = fnc.GetCookie("MIP");

                    SqlParameter pOutput = new SqlParameter("@RET_VAL", SqlDbType.Int);
                    pOutput.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pOutput);

                    cmd.ExecuteNonQuery();

                    retValue = (int)pOutput.Value;
                }

                */      
                
                dbcon.Close();
            }


            return "a";
        }
    }
}