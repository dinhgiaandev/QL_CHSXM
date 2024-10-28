using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QL_CHSXM.Models;

namespace QL_CHSXM.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var items = db.Products.ToList();
            return View(items);
        }

        public ActionResult Detail(string alias, int id)
        {
            var item = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductImage).FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                item.ViewCount++;
                db.SaveChanges();
            }
            return View(item);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.Products.Include(p => p.ProductCategory).Where(p => p.ProductCategoryId == id && p.ProductCategory.IsActive).ToList();
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Include(p => p.ProductImage).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Take(12).ToList();
            return PartialView(items);
        }
    }
}
