using Manager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Common;
using Manager.Models.Manage;
using Manager.Service.Manage;
using Manager.Request.Manage;
using Manager.Request;
using System.Runtime.Remoting.Messaging;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]
    public class AdminController : Controller
    {
        private AdminService adminService;
        private AdminGroupService adminGroupService;
        private MenuService menuService;

        public AdminController() { 
            this.adminService = new AdminService();
            this.adminGroupService = new AdminGroupService();
            this.menuService = new MenuService();
        }

        // GET: Admin
        [HttpGet]
        [Route("manage/admin/list")]
        public ActionResult List(AdminSearch adminSearch)
        {
            AdminSearchModel adminSearchModel = adminService.getAdminList(adminSearch);
            adminSearch.pagination = adminSearchModel.pagination;

            AdminSearchListModel model = new AdminSearchListModel() {
                Search = adminSearch,
                AdminList = adminSearchModel.AdminList,
                AdminGroupList = adminGroupService.getList()
            };

            return View(model);
        }

        [HttpGet]
        [Route("manage/admin/add")]
        public ActionResult Add()
        {
            AdminGroupListModel model = new AdminGroupListModel()
            {
                AdminGroupList = adminGroupService.getList()
            };
            return View(model);
        }

        [HttpPost]
        [Route("manage/admin/ajax/adminidduplicatecheck")]
        public void AdminIDDuplicationCheck(AdminDuplicationRequest adminDuplicationRequest)
        {
            string result = adminService.AdminID_DuplicationCheck(adminDuplicationRequest);
            Response.Write(result + "|||||");
            Response.End();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/admin/add")]
        public ActionResult AddOk(AdminAddRequest adminAddRequest)
        {
            string result = adminService.insert_Admin(adminAddRequest);

            if (result.Equals("OK"))
            {
                return MessageConfig.AlertMessage("등록 되었습니다.", "location.replace('/manage/admin/list');");
            } 
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }
        }

        [HttpGet]
        [Route("manage/admin/popup/admininfo/{adminId}")]
        public ActionResult Info(string adminId)
        {
            AdminInfoModel model = new AdminInfoModel()
            {
                AdminModel = adminService.getAdminIfo(adminId),
                AdminGroupList = adminGroupService.getList()
            };
            return View(model);
        }

        [HttpPost]
        [Route("manage/admin/ajax/admingroupcodemodify")]
        public void AdminModifyGroupCode(AdminModifyGroupRequest adminModifyGroupRequest)
        {
            Response.Write(adminService.updateAdminGroup(adminModifyGroupRequest));
            Response.End();
        }

        [HttpPost]
        [Route("manage/admin/ajax/adminpwdmodify/{adminId}")]
        public ActionResult AdminPwdModify(string adminId)
        {
            ViewBag.adminId = adminId;
            return View();
        }

        [HttpPost]
        [Route("manage/admin/ajax/adminpwdmodify")]
        public void AdminPwdModifyOk(AdminPwdModifyRequest adminPwdModifyRequest)
        {
            string result = adminService.updateAdminPwd(adminPwdModifyRequest);
            Response.Write(result);
            Response.End();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/admin/adminhpmodify")]
        public ActionResult AdminHPModifyOk(AdminHpModifyRequest adminHpModifyRequest)
        {
            string result = adminService.updateAdminHp(adminHpModifyRequest);
            if (result.Equals("OK"))
            {
                return MessageConfig.AlertMessage("핸드폰 번호가 수정 되었습니다.", "location.replace('/manage/admin/popup/admininfo/" + adminHpModifyRequest.adminid + "');");
            }
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/admin/adminmodify")]
        public ActionResult AdminInfoModifyOk(AdminInfoModifyRequest adminInfoModifyRequest)
        {
            string result = adminService.updateAdminInfo(adminInfoModifyRequest);
            if (result.Equals("OK"))
            {
                return MessageConfig.AlertMessage("수정 되었습니다.", "location.replace('/manage/admin/popup/admininfo/" + adminInfoModifyRequest.adminid + "');");
            }
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/admin/admindelete")]
        public ActionResult AdminDeleteOk(AdminDeleteRequest adminDeleteRequest)
        {
            string result = adminService.deleteAdminInfo(adminDeleteRequest);
            if (result.Equals("OK"))
            {
                return MessageConfig.AlertMessage("삭제 되었습니다.", "opener.location.reload();self.close();");
            }
            else
            {
                return MessageConfig.AlertMessage(result, "history.back();");
            }
        }

        [HttpGet]
        [Route("manage/admin/popup/loginlist/{adminId}")]
        public ActionResult AdminLoginList(string adminId, Search search)
        {
            ViewBag.adminid = adminId;
            return View(adminService.getLoginList(adminId, search));
        }

        [HttpGet]
        [Route("manage/admin/menuauth")]
        public ActionResult AdminMenuAuthList()
        {
            MenuAuthListModel model = new MenuAuthListModel()
            {
                MenuAuthList = menuService.getMenuAuthList()
            };
            return View(model);
        }

        [HttpGet]
        [Route("manage/admin/popup/authgrouplist")]
        public ActionResult MenuAuthGroupList(AdminMenuAuthRequest adminMenuAuthRequest)
        {
            MenuAuthGroupListModel model = new MenuAuthGroupListModel()
            {
                adminMenuAuthRequest = adminMenuAuthRequest,
                Depth1List = menuService.getMenuDepth1ForUse(),
                Depth2List = menuService.getMenuDepth2ForUse(adminMenuAuthRequest.MCode1),
                GroupList = adminGroupService.getListForAdminList(adminMenuAuthRequest)
            };

            return View(model);
        }

        [HttpGet]
        [Route("manage/admin/popup/authadminlist")]
        public ActionResult MenuAuthAdminList(AdminMenuAuthRequest adminMenuAuthRequest)
        {
            MenuAuthAdminListModel model = new MenuAuthAdminListModel()
            {
                adminMenuAuthRequest = adminMenuAuthRequest,
                Depth1List = menuService.getMenuDepth1ForUse(),
                Depth2List = menuService.getMenuDepth2ForUse(adminMenuAuthRequest.MCode1),
                AdminList = adminService.getListForGroupSearch(adminMenuAuthRequest)
            };

            return View(model);
        }
    }
}