using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Helpers;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Mvc.Razor;
using System.Web.WebPages;
using Manager.Common;
using Manager.Models.Auth;
using Manager.Models.Manage;
using Manager.Service.Manage;
using Microsoft.Ajax.Utilities;

namespace Manager.Common
{
    public class AuthorizeFilter : AuthorizeAttribute
    {
        private AdminGroupService adminGroupService;
        private MenuService menuService;

        public AuthorizeFilter()
        {
            this.adminGroupService = new AdminGroupService();
            this.menuService = new MenuService();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContextBase httpContext = filterContext.HttpContext;
            string sProgID = httpContext.Request.ServerVariables["URL"];
            string thisPage = httpContext.Request.ServerVariables["SCRIPT_NAME"];
            string returnPageType = Func.getReturnPageType(httpContext, thisPage);
            string pageType= Func.getPageType(httpContext, thisPage);
            bool isRead = false;
            bool isWrite = false;

            if (Func.GetCookie("adminid").IsEmpty())
            {
                string sProgQueryString;
                try
                {
                    sProgQueryString = httpContext.Request.QueryString.ToString();
                }
                catch
                {
                    sProgQueryString = "";
                }

                if (sProgQueryString.Length > 0)
                {
                    sProgID = HttpUtility.UrlEncode(sProgID + '?' + sProgQueryString);
                }
                else
                {
                    sProgID = HttpUtility.UrlEncode(sProgID);
                }                

                httpContext.Response.Redirect("/login?ProgID="+sProgID);
                return;
            }
            else
            {   
                if (!Func.GetUserIP().Equals(Func.GetCookie("ip")))
                {
                    filterContext.Result = MessageConfig.AlertMessage("정상적인 접근이 아닙니다.", "location.replace('/login');");
                }

                string adminGroup = Func.GetCookie("admingroup");
                if (adminGroup.IsEmpty())
                {
                    filterContext.Result = MessageConfig.AlertMessage("유효하지 않은 경로 입니다.", returnPageType);
                }

                AdminGroupModel adminGroupModel = adminGroupService.getInfoByGroupCode(Convert.ToInt32(adminGroup));
                if (adminGroupModel.groupread.IsEmpty())
                {
                    filterContext.Result = MessageConfig.AlertMessage("유효하지 않은 경로 입니다.", returnPageType);
                }

                string[] thisUri = thisPage.Split('/');
                if (thisUri.Length > 2)
                {
                    thisPage = "/" + thisUri[1] + "/" + thisUri[2];
                }
                MenuModel menuModel = menuService.getMenuInfoByMenuUrl(thisPage);

                if (menuModel.menucode.IsEmpty() && !thisPage.Contains("/common"))
                {
                    filterContext.Result = MessageConfig.AlertMessage("유효하지 않은 경로 입니다.", returnPageType);
                } 
                else
                {
                    if (!menuModel.menucode.IsEmpty())
                    {
                        isRead = adminGroupModel.groupread.Equals("0") || adminGroupModel.groupread.Contains(menuModel.menucode);
                        isWrite = adminGroupModel.groupwrite.Equals("0") || adminGroupModel.groupwrite.Contains(menuModel.menucode);
                    }
                }

                if (httpContext.Request.HttpMethod.Equals("GET"))
                {
                    if (thisPage.Equals("/"))
                    {
                        PermissionModel mainPermissionModel = new PermissionModel();
                        mainPermissionModel.MenuCode1 = "0000";
                        mainPermissionModel.MenuCode2 = "0000";
                        mainPermissionModel.isWrite = false;
                        mainPermissionModel.MenuChoice = "N";
                        mainPermissionModel.TopMenuList = getTopMenu(adminGroupModel.groupread);                        
                        filterContext.Controller.ViewBag.Permission = mainPermissionModel;

                        return;
                    }

                    if (thisPage.Contains("/common"))
                    {
                        return;
                    }

                    if (pageType.Equals("ajax"))
                    {
                        ValidateRequestHeader(httpContext);
                        if (httpContext.Request.ServerVariables["HTTP_REFERER"].IsEmpty())
                        {
                            filterContext.Result = MessageConfig.AlertMessage("유효하지 않은 경로 입니다.", returnPageType);
                        }
                        else
                        {
                            if (!httpContext.Request.ServerVariables["HTTP_REFERER"].Contains(SetInfo.domain))
                            {
                                filterContext.Result = MessageConfig.AlertMessage("유효하지 않은 경로 입니다.", returnPageType);
                            }
                        }
                    }

                    if (!isRead)
                    {
                        filterContext.Result = MessageConfig.AlertMessage("권한이 없습니다.", returnPageType);
                        return;
                    }

                    PermissionModel permissionModel = new PermissionModel();
                    if (pageType.Equals("general"))
                    {
                        permissionModel.MenuCode1 = menuModel.menupcode;
                        permissionModel.MenuCode2 = menuModel.menucode;
                        permissionModel.MenuChoice = menuModel.menuchoice;
                        permissionModel.isWrite = isWrite;
                        permissionModel.TopMenuList = getTopMenu(adminGroupModel.groupread);
                        permissionModel.LeftMenuList = getLeftMenu(menuModel.menupcode);
                        permissionModel.TopMenuName = getMenuName(menuModel.menupcode, permissionModel.TopMenuList);
                        permissionModel.LeftMenuName = getMenuName(menuModel.menucode, permissionModel.LeftMenuList);
                    }
                    else
                    {
                        permissionModel.MenuCode1 = menuModel.menupcode;
                        permissionModel.MenuCode2 = menuModel.menucode;
                        permissionModel.MenuChoice = menuModel.menuchoice;
                        permissionModel.isWrite = isWrite;
                    }

                    filterContext.Controller.ViewBag.Permission = permissionModel;

                    return;
                }
                else
                {
                    if (pageType.Equals("ajax"))
                    {
                        ValidateRequestHeader(httpContext);
                    }

                    if (httpContext.Request.ServerVariables["HTTP_REFERER"].IsEmpty())
                    {                        
                        filterContext.Result = MessageConfig.AlertMessage("유효하지 않은 경로 입니다.", returnPageType);
                    }
                    else
                    {   
                        if (!httpContext.Request.ServerVariables["HTTP_REFERER"].Contains(SetInfo.domain))
                        {
                            filterContext.Result = MessageConfig.AlertMessage("유효하지 않은 경로 입니다.", returnPageType);
                        }
                    }

                    if (!isWrite && !thisPage.Contains("/common"))
                    {
                        filterContext.Result = MessageConfig.AlertMessage("권한이 없습니다.", returnPageType);
                    }

                    return;
                }                              
            }
        }
               

        public List<MenuModel> getTopMenu(string groupRead)
        {
            List<MenuModel> topMenuList = new List<MenuModel>();
            foreach (var menu in menuService.getMenuDepth1ForUse())
            {
                if (groupRead.Contains(menu.menucode) || groupRead.Equals("0"))
                {
                    menu.menuurl = getChildMenuUrl(groupRead, menu.menucode);
                    topMenuList.Add(menu);
                }
            }
            return topMenuList;
        }

        public List<MenuModel> getLeftMenu(string menuPCode)
        {
            return menuService.getMenuDepth2ForUse(menuPCode);            
        }

        public string getChildMenuUrl(string groupRead, string menuPCode)
        {
            string retUrl = "";

            foreach (var menu in menuService.getMenuDepth2ForUse(menuPCode))
            {
                if (groupRead.Equals("0"))
                {
                    retUrl = menu.menuurl;
                    break;
                }
                else
                {
                    if (groupRead.Contains(menu.menucode))
                    {
                        retUrl = menu.menuurl;
                        break;
                    }
                }
            }

            return retUrl;
        }
        
        public string getMenuName(string menucode, List<MenuModel> MenuList)
        {
            string retVal = "";

            foreach(var menu in MenuList)
            {
                if (menucode.Equals(menu.menucode))
                {
                    retVal = menu.menuname;
                    break;
                }
            }

            return retVal;
        }

        public void ValidateRequestHeader(HttpContextBase httpContext)
        {   
            string cookieToken = "";
            string formToken = "";
                        
            if (httpContext.Request.Headers["RequestVerificationToken"] != null)
            {
                string[] tokens = httpContext.Request.Headers["RequestVerificationToken"].Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }            
            AntiForgery.Validate(cookieToken, formToken);
        }
    }
}