using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Manager.Common
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                HttpContextBase httpContext = filterContext.HttpContext;
                string thisPage = httpContext.Request.ServerVariables["SCRIPT_NAME"];
                string returnPageType = Func.getPageType(httpContext, thisPage).Equals("ajax") ? "ajax" : "history.back();";

                string errMsg = "";

                foreach (ModelState modelState in filterContext.Controller.ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errMsg = error.ErrorMessage.ToString();
                    }
                }
                filterContext.Result = MessageConfig.AlertMessage(errMsg, returnPageType);
            }            
            base.OnActionExecuting(filterContext);
        }
    }
}