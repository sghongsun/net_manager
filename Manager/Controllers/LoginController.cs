using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Manager.Common;
using Manager.Service.Manage;
using Manager.Request;

namespace Manager.Controllers
{
    public class LoginController : Controller
    {
        private AdminService adminService;

        public LoginController() { 
            this.adminService = new AdminService();
        }

        // GET: Login
        [Route("login")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidationFilter]        
        [Route("login")]
        public ActionResult LoginOk(LoginRequest loginRequest)
        {
            string result = adminService.LoginProc(loginRequest);
            if (result.Equals("OK"))
            {                
                if (loginRequest.ProgID.IsEmpty())
                {
                    loginRequest.ProgID = "/";
                }

                Response.Redirect(loginRequest.ProgID);
            }
            return MessageConfig.AlertMessage(result, "history.back();");            
        }

        [Route("logout")]
        public void logout()
        {
            Func.SetCookie("adminid", "", -1);
            Func.SetCookie("adminname", "", -1);
            Func.SetCookie("admingroup", "", -1);
            Func.SetCookie("menuchoice", "", -1);
            Func.SetCookie("ip", "", -1);

            Response.Redirect("/");
        }
    }
}