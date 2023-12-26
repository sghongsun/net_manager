using Manager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Service.Manage;
using Manager.Request.Manage;
using Manager.Models.Manage;

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
    }
}