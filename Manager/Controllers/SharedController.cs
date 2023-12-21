using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Models.Auth;

namespace Manager.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared        
        [HttpGet]
        public ActionResult Header()
        {   
            return View();
        }

        [HttpGet]
        [Route("error")]
        public ActionResult Error()
        {
            return View();
        }
    }
}