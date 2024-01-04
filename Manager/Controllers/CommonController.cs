using Manager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Service.Manage;
using Manager.Request.Manage;
using Manager.Models.Manage;
using System.Web.WebPages;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]
    public class CommonController : Controller
    {
        private MyMenuChoiceService myMenuChoiceService;

        public CommonController()
        {
            this.myMenuChoiceService = new MyMenuChoiceService();
        }

        // GET: Common
        [HttpPost]
        [Route("common/ajax/mymenuadd")]
        public void MyMenuAddOk(MyMenuChoiceAddDeleteRequest myMenuChoiceAddDeleteRequest)
        {            
            Response.Write(myMenuChoiceService.insert_MyMenu(myMenuChoiceAddDeleteRequest));
            Response.End();
        }

        [HttpPost]
        [Route("common/ajax/mymenulist")]
        public ActionResult MyMenuList()
        {
            MyMenuChoiceListModel model = new MyMenuChoiceListModel()
            {
                MyMenuChoiceList = myMenuChoiceService.getList()
            };

            return View(model);
        }
        
        [HttpPost]
        [Route("common/ajax/mymenudelete")]
        public void MyMenuDeleteOk(MyMenuChoiceAddDeleteRequest myMenuChoiceAddDeleteRequest)
        {
            Response.Write(myMenuChoiceService.delete_MyMenu(myMenuChoiceAddDeleteRequest));
            Response.End();
        }

        [HttpPost]
        [Route("common/ajax/mymenudisplaynummodify")]
        public void MyMenuDisplayNumUpdateOk(MyMenuChoiceDispNumUpdateRequest myMenuChoiceDispNumUpdateRequest)
        {
            Response.Write(myMenuChoiceService.update_MyMenu_DispNum(myMenuChoiceDispNumUpdateRequest));
            Response.End();
        }

        [HttpGet]
        [Route("common/editorfileupload")]
        public ActionResult editorFileUpload() {
            ViewBag.ID = Func.getRequestQueryStringToString("I1");
            ViewBag.DetailFolder = Func.getRequestQueryStringToString("DetailFolder");            

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("common/editorfileupload")]
        public ActionResult eidtorFileUploadOk()
        {
            string ID = Func.getRequestFormToString("ID");
            string DetailFolder = Func.getRequestFormToString("DetailFolder");
            HttpPostedFileBase uploadFile = Request.Files["UpdateImage"];
            
            if (!FileUtil.validFileMimeType(uploadFile, "image"))
            {
                MessageConfig.AlertMessage("이미지 파일만 등록해 주세요.", "history.back();");
            }

            string uploadFolder = "editor";
            if (!DetailFolder.IsEmpty())
            {
                uploadFolder += "/" + DetailFolder;
            }

            string saveFileName = FileUtil.uploadFile(uploadFile, uploadFolder);            

            string retVal = "";
            retVal += "parent.parent.insertIMG('" + ID + "','" + saveFileName + "');\r\n";
            retVal += "parent.parent.oEditors.getById['" + ID + "'].exec('SE_TOGGLE_IMAGEUPLOAD_LAYER');\r\n";

            return MessageConfig.AlertMessage("", retVal);

        }        
    }
}