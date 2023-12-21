using Manager.Common;
using Manager.Service.Manage;
using Manager.Models.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]

    public class AdminGroupController : Controller
    {
        private AdminGroupService adminGroupService;
        private MenuService menuService;

        public AdminGroupController()
        {
            this.adminGroupService = new AdminGroupService();
            this.menuService = new MenuService();
        }

        // GET: AdminGroup
        [HttpGet]
        [Route("manage/group/list")]
        public ActionResult List()
        {
            AdminGroupListModel model = new AdminGroupListModel()
            {
                AdminGroupList = adminGroupService.getList()
            };
            return View(model);
        }

        [HttpGet]
        [Route("manage/group/add")]
        public ActionResult Add()
        {
            MenuListModel model = new MenuListModel()
            {
                Depth1List = menuService.getMenuDepth1ForAll(),
                Depth2List = menuService.getMenuDepth2ForAll()

            };
            return View(model);
        }
    }
}