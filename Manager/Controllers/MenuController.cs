using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Manager.Common;
using Manager.Models.Manage;
using Manager.Service.Manage;
using Manager.Request.Manage;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]
    public class MenuController : Controller
    {
        private MenuService menuService;

        public MenuController()
        {
            this.menuService = new MenuService();
        }

        // GET: Menu
        [HttpGet]
        [Route("manage/menu/list")]
        [Route("manage/menu/list/{menuPCode}")]
        public ActionResult List(string menuPCode = "")
        {
            List<MenuModel> menuDepth1List = menuService.getMenuDepth1ForAll();
            if (menuPCode.IsEmpty())
            {
                menuPCode = menuDepth1List[0].menucode;
            }

            ViewBag.MenuPCode = menuPCode;

            MenuListModel menuModel = new MenuListModel()
            {
                Depth1List = menuDepth1List,
                Depth2List = menuService.getMenuDepth2ForAll(menuPCode)
            };            

            return View(menuModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/menu/add")]
        public ActionResult AddOk(MenuAddRequest menuAddRequest)
        {
            string result = menuService.insertMenu(menuAddRequest);
            if (result.Equals("OK"))
            {
                return MessageConfig.AlertMessage("추가 되었습니다.", "location.replace('/manage/menu/list');");
                
            } 
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }            
        }

        [HttpGet]
        [Route("manage/menu/modify/{menuPCode}/{menuCode}")]
        public ActionResult Modify(string menuPCode, string menuCode)
        {
            MenuModel menuModel = menuService.getMenuInfo(menuPCode, menuCode);
            
            return View(menuModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/menu/modify")]
        public ActionResult ModifyOk(MenuModifyRequest menuModifyRequest)
        {
            string result = menuService.updateMenu(menuModifyRequest);
            if (result.Equals("OK"))
            {
                return MessageConfig.AlertMessage("수정 되었습니다.", "location.replace('/manage/menu/modify/"+menuModifyRequest.menuPCode+"/"+menuModifyRequest.menuCode+"');");
            }
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }            
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/menu/delete")]
        public ActionResult DeleteOk(MenuDeleteRequest menuDeleteRequest)
        {
            string result = menuService.deleteMenu(menuDeleteRequest);
            if (result.Equals ("OK"))
            {
                string retUrl = "/manage/menu/list";
                if (!menuDeleteRequest.menuPCode.Equals("0000"))
                {
                    retUrl += "/" + menuDeleteRequest.menuPCode;
                }
                return MessageConfig.AlertMessage("삭제 되었습니다.", "location.replace('" + retUrl + "');");
            }
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/menu/dispnummodify")]
        public ActionResult updateDisplayNum(MenuDisplayNumModifyRequest menuDisplayNumModifyRequest)
        {
            string result = menuService.updateMenuDisplayNum(menuDisplayNumModifyRequest);
            if (result.Equals("OK"))
            {
                string retUrl = "/manage/menu/list";
                if (!menuDisplayNumModifyRequest.menuPCode.Equals("0000"))
                {
                    retUrl += "/" + menuDisplayNumModifyRequest.menuPCode;
                }
                return MessageConfig.AlertMessage("", "location.replace('" + retUrl + "');");
            }
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }
        }

        [HttpGet]
        [Route("manage/menu/ajax/getmcode2list")]
        public void menuDepth2List()
        {
            string MenuPCode = Func.getRequestQueryStringToString("MCode1");
            List<MenuModel> menuList = menuService.getMenuDepth2ForUse(MenuPCode);
            StringBuilder MenuCode = new StringBuilder();
            StringBuilder MenuName = new StringBuilder();

            foreach (MenuModel menuModel in menuList)
            {
                MenuCode.Append(menuModel.menucode).Append(",");
                MenuName.Append(menuModel.menuname).Append(",");
            }

            MenuCode.Remove(MenuCode.Length - 1, 1);
            MenuName.Remove(MenuName.Length - 1, 1);

            Response.Write("OK|||||"+MenuCode+"|||||"+MenuName);
            Response.End();
        }
    }
}