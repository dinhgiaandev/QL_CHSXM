using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_CHSXM.Models;

namespace QL_CHSXM.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop()
        {
            var items = db.Categories.ToList();
            return PartialView("_MenuTop", items);
        }

        //sản phẩm
        public ActionResult MenuProductCategory()
        {
            var items = db.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", items);
        }

        //dịch vụ
        public ActionResult MenuServiceCategory()
        {
            var items = db.ServiceCategories.ToList();
            return PartialView("_MenuServiceCategory", items);
        }

        //sản phẩm
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = db.ProductCategories.ToList();
            return PartialView("_MenuLeft", items);
        }

        //dịch vụ
        public ActionResult MenuLeftService(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = db.ServiceCategories.ToList();
            return PartialView("_MenuLeftService", items);
        }

        //sản phẩm
        public ActionResult MenuArrivals()
        {
            var items = db.ProductCategories.ToList();
            return PartialView("_MenuArrivals", items);
        }
    }
}