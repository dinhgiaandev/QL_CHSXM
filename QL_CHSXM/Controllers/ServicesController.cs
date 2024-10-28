using QL_CHSXM.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using QL_CHSXM.Models.EF;

namespace QL_CHSXM.Controllers
{
    public class ServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Services/Index
        public ActionResult Index()
        {
            var items = db.Services.ToList();
            return View(items);
        }

        // GET: Services/Detail
        public ActionResult Detail(string alias, int id)
        {
            var item = db.Services.Find(id);
            if (item != null)
            {
                item.ViewCount++;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            return View(item);
        }

        // GET: Services/ServiceCategory
        public ActionResult ServiceCategory(string alias, int id)
        {
            var items = db.Services.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ServiceCategoryId == id).ToList();
            }
            var cate = db.ServiceCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);
        }

        // GET: Services/Partial_ItemsByCateId
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Services.Take(12).ToList();
            return PartialView(items);
        }

        // GET: Services/Partial_ServiceSales
        public ActionResult Partial_ServiceSales()
        {
            var items = db.Services.Take(12).ToList();
            return PartialView(items);
        }

        // GET: Services/BookService
        public ActionResult BookService()
        {
            var services = db.Services.ToList();
            var serviceList = services.Select(s => new
            {
                Id = s.Id,
                Name = $"{s.Title} - {s.Price.ToString("N0")} đ"
            }).ToList();

            ViewBag.Services = new SelectList(serviceList, "Id", "Name");

            return View(new BookServiceViewModel());
        }

        // POST: Services/BookService
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookService(BookServiceViewModel model)
        {
            // Retrieve all services from the database
            var services = db.Services.ToList();

            if (ModelState.IsValid)
            {
                // Retrieve service details from the database based on model.ServiceId
                var service = db.Services.Find(model.ServiceId);

                if (service == null)
                {
                    ModelState.AddModelError("", "Dịch vụ không tồn tại.");
                    ViewBag.Services = new SelectList(services, "Id", "Title", model.ServiceId);
                    return View(model);
                }

                // Create a new BookServiceItem instance (assuming BookServiceItem is used in the context)
                var item = new BookServiceViewModel
                {
                    ServiceId = service.Id,
                    ServiceName = service.Title,
                    Description = service.Description,
                    Price = service.Price,
                    BookingDate = model.BookingDate,
                    CustomerName = model.CustomerName,
                    CustomerPhone = model.CustomerPhone,
                    CustomerEmail = model.CustomerEmail,
                };

                // Calculate VAT and total price (if needed)
                item.VAT = item.CalculateVAT();

                // Perform necessary business logic here
                // Example: Save data to database or perform calculations

                // Send confirmation email to customer
                var contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/sendServiceCus.html"))
                    .Replace("{{BookingDate}}", model.BookingDate.ToString("dd/MM/yyyy"))
                    .Replace("{{ServiceName}}", service.Title)
                    .Replace("{{CustomerName}}", model.CustomerName)
                    .Replace("{{CustomerPhone}}", model.CustomerPhone)
                    .Replace("{{CustomerEmail}}", model.CustomerEmail);

                QL_CHSXM.Common.Common.SendMail("SBTC Store", $"Xác nhận đặt lịch dịch vụ {service.Title}", contentCustomer, model.CustomerEmail);

                // Redirect to success page or do further processing
                return RedirectToAction("BookServiceSuccess");
            }

            // If model state is not valid, re-populate dropdown and return to the view
            ViewBag.Services = new SelectList(services, "Id", "Title", model.ServiceId);
            return View(model);
        }
        // Partial view for selecting booking date
        public ActionResult _BookingDatePartial()
        {
            // Here you can implement logic to fetch available booking dates or provide a date picker
            return PartialView();
        }

        // GET: Services/BookServiceSuccess
        public ActionResult BookServiceSuccess()
        {
            return View();
        }
    }
}
