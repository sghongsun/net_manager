using Manager.Common;
using Manager.Models.Manage;
using Manager.Request.Manage;
using Manager.Service.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]
    public class TermsController : Controller
    {
        private TermsService termsService;

        public TermsController()
        {
            this.termsService = new TermsService();
        }

        // GET: Terms
        [HttpGet]
        [Route("manage/terms/list")]
        public ActionResult List(TermsSearch termsSearch)
        {
            return View(termsService.getList(termsSearch));
        }
    }
}