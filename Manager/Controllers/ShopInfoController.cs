using Manager.Common;
using Manager.Request.Manage;
using Manager.Service.Manage;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]
    public class ShopInfoController : Controller
    {
        private ShopInfoService shopInfoService;
        public ShopInfoController()
        {
            this.shopInfoService = new ShopInfoService();
        }

        // GET: ShopInfo

        [HttpGet]
        [Route("manage/shopinfo/info")]
        public ActionResult Info()
        {
            return View(shopInfoService.getShopInfo());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("manage/shopinfo/modify")]
        public ActionResult ModifyOk(ShopInfoRequest shopInfoRequest)
        {
            shopInfoService.updateShopInfo(shopInfoRequest);
            return MessageConfig.AlertMessage("수정 되었습니다.", "location.replace('/manage/shopinfo/info');");
        }
    }
}