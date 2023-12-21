using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Manager.Common;
using Manager.Service.Manage;

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

        [HttpPost, ValidateAntiForgeryToken]        
        [Route("login")]
        public ActionResult LoginOk(string returnUrl)
        {
            string result = adminService.LoginProc();
            if (result.Equals("OK"))
            {
                string ProgID = Func.getRequestFormToString("ProgID");
                if (ProgID.IsEmpty())
                {
                    ProgID = "/";
                }

                Response.Redirect(ProgID);
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