using System;
using System.Linq;
using System.Web.Mvc;
using QL_CHSXM.Models;
using QL_CHSXM.Models.EF;
using System.Data.Entity;

namespace QL_CHSXM.Controllers
{
    public class BookServiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookService/Add
        public ActionResult Add()
        {
            var servicesFromDB = db.Services.ToList();
            var productsFromDB = db.Products.ToList();

            var services = servicesFromDB.Select(s => new
            {
                Id = s.Id,
                Title = $"{s.Title} - {s.Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}",
                Price = s.Price
            }).ToList();

            var products = productsFromDB.Select(p => new
            {
                Id = p.Id,
                Title = $"{p.Title} - {p.Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}",
                Price = p.Price
            }).ToList();

            ViewBag.Services = new SelectList(services, "Id", "Title");
            ViewBag.Products = new SelectList(products, "Id", "Title");
            ViewBag.ServicesList = services;
            ViewBag.ProductsList = products;

            return View();
        }

        // POST: Admin/BookService/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(BookService model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                db.BookServices.Add(model);
                db.SaveChanges();
                return RedirectToAction("BookServiceSuccess");
            }

            // Thiết lập lại ViewBag nếu model state không hợp lệ
            var servicesFromDB = db.Services.ToList();
            var serviceCategoriesFromDB = db.ServiceCategories.ToList();
            var productsFromDB = db.Products.ToList();
            var productCategoriesFromDB = db.ProductCategories.ToList();

            var services = servicesFromDB.Select(s => new
            {
                Id = s.Id,
                Title = $"{s.Title} - {s.Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}",
                Price = s.Price
            }).ToList();

            var serviceCategories = serviceCategoriesFromDB.Select(sc => new
            {
                Id = sc.Id,
                Title = sc.Title
            }).ToList();

            var products = productsFromDB.Select(p => new
            {
                Id = p.Id,
                Title = $"{p.Title} - {p.Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}",
                Price = p.Price
            }).ToList();

            var productCategories = productCategoriesFromDB.Select(pc => new
            {
                Id = pc.Id,
                Title = pc.Title
            }).ToList();

            ViewBag.Services = new SelectList(services, "Id", "Title", model.ServiceId);
            ViewBag.ServiceCategories = new SelectList(serviceCategories, "Id", "Title", model.ServiceCategoryId);
            ViewBag.Products = new SelectList(products, "Id", "Title", model.ProductId);
            ViewBag.ProductCategories = new SelectList(productCategories, "Id", "Title", model.ProductCategoryId);

            ViewBag.ServicesList = services;
            ViewBag.ProductsList = products;

            return View(model);
        }

        public ActionResult BookServiceSuccess()
        {
            return View();
        }
    }
}
