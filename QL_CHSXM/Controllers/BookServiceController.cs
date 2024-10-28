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

        // GET: Admin/BookService/Add
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

            ViewBag.Services = new MultiSelectList(services, "Id", "Title");
            ViewBag.Products = new MultiSelectList(products, "Id", "Title");
            ViewBag.ServicesList = services;
            ViewBag.ProductsList = products;
            PopulateViewBagForAddEdit();

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

                // Thêm từng dịch vụ được chọn
                if (model.ServiceIds != null && model.ServiceIds.Any())
                {
                    foreach (var serviceId in model.ServiceIds)
                    {
                        var service = db.Services.Find(serviceId);
                        if (service != null)
                        {
                            model.Services.Add(service);
                        }
                    }
                }

                // Thêm từng sản phẩm được chọn
                if (model.ProductIds != null && model.ProductIds.Any())
                {
                    foreach (var productId in model.ProductIds)
                    {
                        var product = db.Products.Find(productId);
                        if (product != null)
                        {
                            model.Products.Add(product);
                        }
                    }
                }

                db.BookServices.Add(model);
                db.SaveChanges();
                return RedirectToAction("BookServiceSuccess");
            }

            // Nếu ModelState không hợp lệ, tái tạo ViewBag
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
            ViewBag.Services = new MultiSelectList(services, "Id", "Title", model.ServiceIds);
            ViewBag.Products = new MultiSelectList(products, "Id", "Title", model.ProductIds);
            ViewBag.ServicesList = services;
            ViewBag.ProductsList = products;
            PopulateViewBagForAddEdit(model);
            return View(model);
        }

        private void PopulateViewBagForAddEdit(BookService model = null)
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

            ViewBag.Services = new MultiSelectList(services, "Id", "Title", model?.ServiceIds);
            ViewBag.Products = new MultiSelectList(products, "Id", "Title", model?.ProductIds);
            ViewBag.ServicesList = services;
            ViewBag.ProductsList = products;
        }

        public ActionResult BookServiceSuccess()
        {
            return View();
        }
    }
}
