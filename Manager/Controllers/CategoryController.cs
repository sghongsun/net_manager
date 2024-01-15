using Manager.Common;
using Manager.Models.Product;
using Manager.Request.Product;
using Manager.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Manager.Controllers
{
    [AuthorizeFilter, ValidationFilter]
    public class CategoryController : Controller
    {
        private CategoryService categoryService;

        public CategoryController()
        {
            this.categoryService = new CategoryService();
        }

        // GET: Category

        [HttpGet]
        [Route("product/category/list")]
        public ActionResult List()
        {
            CategoryListModel model = new CategoryListModel()
            {
                Category1List = categoryService.getCategory1ListInProductCnt(),
                Category2List = categoryService.getCategory2ListInProductCnt()
            };
            return View(model);
        }

        [HttpGet]
        [Route("product/category/list1")]
        public ActionResult List1()
        {
            return View(categoryService.getCategory1List());
        }

        [HttpGet]
        [Route("product/category/list2")]
        public ActionResult List2()
        {
            string categoryCode1 = Func.getRequestQueryStringToString("categorycode1");

            List<CategoryModel> category1List = categoryService.getCategory1List();

            if (categoryCode1.IsEmpty())
            {
                categoryCode1 = category1List[0].categorycode1;
            }

            ViewBag.CategoryCode1 = categoryCode1;
            CategoryListModel model = new CategoryListModel()
            {
                Category1List = category1List,
                Category2List = categoryService.getCategory2List(categoryCode1)
            };

            return View(model);
        }

        [HttpPost]
        [Route("product/category/ajax/addform")]
        public ActionResult Add()
        {
            ViewBag.Depth = Func.getRequestFormToString("depth");
            return View(categoryService.getCategory1List());
        }

        [HttpPost]
        [Route("product/category/ajax/add1")]
        public void Category1AddOk(Category1AddRequest category1AddRequest)
        {
            categoryService.insertCategory1(category1AddRequest);
            Response.Write("OK|||||");
            Response.End();
        }

        [HttpPost]
        [Route("product/category/ajax/add2")]
        public void Category2AddOk(Category2AddRequest category2AddRequest)
        {
            categoryService.insertCategory2(category2AddRequest);
            Response.Write("OK|||||");
            Response.End();
        }

        [HttpPost]
        [Route("product/category/ajax/modifyform")]
        public ActionResult Modify()
        {
            ViewBag.Depth = Func.getRequestFormToString("Depth");
            CategoryModel model;
            if (ViewBag.Depth.Equals("1"))
            {
                model = categoryService.getCategory1Info(Func.getRequestFormToString("CategoryCode1"));
            }
            else
            {
                model = categoryService.getCategory2Info(Func.getRequestFormToString("CategoryCode1"), Func.getRequestFormToString("CategoryCode2"));
            }

            return View(model);
        }

        [HttpPost]
        [Route("product/category/ajax/modify1")]
        public void Category1ModifyOk(Category1ModifyRequest category1ModifyRequest)
        {
            categoryService.updateCategory1(category1ModifyRequest);
            Response.Write("OK|||||");
            Response.End();
        }

        [HttpPost]
        [Route("product/category/ajax/modify2")]
        public void Category2ModifyOk(Category2ModifyRequest category2ModifyRequest)
        {
            categoryService.updateCategory2(category2ModifyRequest);
            Response.Write("OK|||||");
            Response.End();
        }

        [HttpPost]
        [Route("product/category/ajax/displaynum_modify1")]
        public void Category1DisplayNumModify(Category1DisplayNumModifyRequest category1DisplayNumModifyRequest)
        {
            categoryService.updateCategory1DiplayNum(category1DisplayNumModifyRequest);
            Response.Write("OK|||||");
            Response.End();
        }

        [HttpPost]
        [Route("product/category/ajax/displaynum_modify2")]
        public void Category2DisplayNumModify(Category2DisplayNumModifyRequest category2DisplayNumModifyRequest)
        {
            categoryService.updateCategory2DiplayNum(category2DisplayNumModifyRequest);
            Response.Write("OK|||||");
            Response.End();
        }

    }
}