using System;
using System.Linq;
using System.Web.Mvc;
using QL_CHSXM.Models;
using QL_CHSXM.Models.EF;
using System.Data.Entity;
using PagedList;
using System.Collections.Generic;
using System.Web;

namespace QL_CHSXM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookServiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/BookService
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;

            var items = db.BookServices
                .Include(bs => bs.Services)
                .Include(bs => bs.Products)
                .OrderBy(bs => bs.Id)
                .ToPagedList(pageNumber, pageSize);

            return View(items);
        }


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
                return RedirectToAction("Index");
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

        // GET: Admin/BookService/Edit/5
        public ActionResult Edit(int id)
        {
            var bookService = db.BookServices
                .Include(bs => bs.Services)
                .Include(bs => bs.Products)
                .FirstOrDefault(bs => bs.Id == id);
            if (bookService == null)
            {
                return HttpNotFound();
            }

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

            ViewBag.Services = new MultiSelectList(services, "Id", "Title", bookService.Services.Select(s => s.Id));
            ViewBag.Products = new MultiSelectList(products, "Id", "Title", bookService.Products.Select(p => p.Id));
            ViewBag.ServicesList = services;
            ViewBag.ProductsList = products;

            return View(bookService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookService model)
        {
            if (ModelState.IsValid)
            {
                var existingBookService = db.BookServices
                    .Include(bs => bs.Services)
                    .Include(bs => bs.Products)
                    .FirstOrDefault(bs => bs.Id == model.Id);

                if (existingBookService == null)
                {
                    return HttpNotFound();
                }

                existingBookService.ModifiedDate = DateTime.Now;
                existingBookService.FullName = model.FullName;
                existingBookService.Phone = model.Phone;
                existingBookService.Email = model.Email;
                existingBookService.NameCar = model.NameCar;
                existingBookService.BookingDate = model.BookingDate;

                // Cập nhật Services
                existingBookService.Services.Clear();
                if (model.ServiceIds != null && model.ServiceIds.Any())
                {
                    foreach (var serviceId in model.ServiceIds)
                    {
                        var service = db.Services.Find(serviceId);
                        if (service != null)
                        {
                            existingBookService.Services.Add(service);
                        }
                    }
                }

                // Cập nhật Products
                existingBookService.Products.Clear();
                if (model.ProductIds != null && model.ProductIds.Any())
                {
                    foreach (var productId in model.ProductIds)
                    {
                        var product = db.Products.Find(productId);
                        if (product != null)
                        {
                            existingBookService.Products.Add(product);
                        }
                    }
                }

                db.Entry(existingBookService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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

            return View(model);
        }


        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.BookServices.Find(id);
            if (item != null)
            {
                db.BookServices.Attach(item);
                item.TypePayment = trangthai;
                db.Entry(item).Property(x => x.TypePayment).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
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

        public ActionResult Invoice(int id)
        {
            var item = db.BookServices
                .Include(bs => bs.Products)
                .FirstOrDefault(bs => bs.Id == id);

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
    }
}