using Manager.Common;
using Manager.Service.Manage;
using Manager.Models.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Request.Manage;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]

    public class AdminGroupController : Controller
    {
        private AdminGroupService adminGroupService;
        private MenuService menuService;
        private AdminService adminService;

        public AdminGroupController()
        {
            this.adminGroupService = new AdminGroupService();
            this.menuService = new MenuService();
            this.adminService = new AdminService();
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

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/group/add")]
        public ActionResult AddOk(AdminGroupRequest adminGroupRequest)
        {
            adminGroupService.insertAdminGroup(adminGroupRequest);
            return MessageConfig.AlertMessage("등록 되었습니다.", "location.replace('/manage/group/list');");
        }

        [HttpGet]
        [Route("manage/group/modify/{groupCode}")]
        public ActionResult Modify(int groupCode)
        {
            AdminGroupModifyModel model = new AdminGroupModifyModel()
            {
                Depth1List = menuService.getMenuDepth1ForAll(),
                Depth2List = menuService.getMenuDepth2ForAll(),
                AdminGroup = adminGroupService.getInfoByGroupCode(groupCode)
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/group/modify/{groupCode}")]
        public ActionResult ModifyOk(int groupCode, AdminGroupRequest adminGroupRequest)
        {
            adminGroupService.updateAdminGroup(groupCode, adminGroupRequest);
            return MessageConfig.AlertMessage("수정 되었습니다.", "location.replace('/manage/group/list');");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/group/delete/{groupCode}")]
        public ActionResult DeleteOk(int groupCode)
        {
            adminGroupService.deleteAdminGroup(groupCode);
            return MessageConfig.AlertMessage("삭제 되었습니다.", "location.replace('/manage/group/list');");
        }

        [HttpGet]
        [Route("manage/group/popup/adminlist/{groupCode}")]
        public ActionResult GroupInAdminList(int groupCode) 
        {
            AdminGroupInAdminListModel model = new AdminGroupInAdminListModel()
            {
                groupcode = groupCode,
                AdminGroupList = adminGroupService.getList(),
                AdminList = adminService.getAdminListByGroupCode(groupCode)

            };
            return View(model);
        }
    }
}