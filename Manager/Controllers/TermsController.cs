using Manager.Common;
using Manager.Models.Manage;
using Manager.Request.Manage;
using Manager.Service.Manage;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
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

        [HttpGet]
        [Route("manage/terms/add")]
        public ActionResult Add() 
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        [Route("manage/terms/add")]
        public ActionResult AddOk(TermsAddRequest termsAddRequest)
        {
            termsService.insertTerms(termsAddRequest);
            return MessageConfig.AlertMessage("등록 되었습니다.", "location.replace('/manage/terms/list');");
        }

        [HttpGet]
        [Route("manage/terms/modify/{idx}")]
        public ActionResult Modify(TermsSearch termsSearch, int idx)
        {
            TermsModifyModel model = new TermsModifyModel()
            {
                Terms = termsService.getTermsInfo(idx),
                Search = termsSearch
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        [Route("manage/terms/modify")]
        public ActionResult ModifyOk(TermsModifyRequest termsModifyRequest, TermsSearch termsSearch)
        {            
            termsService.updateTmers(termsModifyRequest);

            var dic = termsSearch.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(termsSearch, null));

            ViewBag.message = "수정 되었습니다.";
            ViewBag.redirectUri = "/manage/terms/list";
            ViewBag.method = "GET";
            ViewBag.data = dic;
            
            return View("messageRedirect");
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        [Route("manage/terms/delete")]
        public ActionResult DeleteOk(TermsDeleteRequest termsDeleteRequest, TermsSearch termsSearch)
        {
            termsService.deleteTmers(termsDeleteRequest);

            var dic = termsSearch.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(termsSearch, null));

            ViewBag.message = "삭제 되었습니다.";
            ViewBag.redirectUri = "/manage/terms/list";
            ViewBag.method = "GET";
            ViewBag.data = dic;

            return View("messageRedirect");
        }
    }
}