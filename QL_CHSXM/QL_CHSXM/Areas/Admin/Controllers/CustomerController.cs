using System.Linq;
using System.Web.Mvc;
using PagedList;
using QL_CHSXM.Models;
using QL_CHSXM.Models.EF; // Adjust the namespace as per your actual model namespace

namespace QL_CHSXM.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext(); // Adjust this to your DbContext

        // GET: Admin/Customer
        public ActionResult Index(int? page)
        {
            //int pageSize = 10; // Adjust the page size as needed
            //int pageNumber = (page ?? 1);

            //var customersWithOrders = db.BookServices.OrderBy(o => o.FullName).ToPagedList(pageNumber, pageSize);

            //ViewBag.Page = pageNumber;
            //ViewBag.PageSize = pageSize;

            //return View(customersWithOrders);
            var pageNumber = page ?? 1;
            var pageSize = 10; // Thay đổi kích thước trang theo yêu cầu

            var customers = db.BookServices.ToList(); // Lấy dữ liệu từ DB

            var groupedCustomers = customers
                .GroupBy(c => new { c.FullName, c.Phone })
                .Select(g => new BookService
                {
                    FullName = g.Key.FullName,
                    Phone = g.Key.Phone,
                    Email = g.First().Email, // Có thể tùy chỉnh để lấy email nào đó hoặc gộp các email
            NameCar = string.Join(", ", g.Select(c => c.NameCar).Distinct()), // Gộp chung các tên phương tiện và loại bỏ trùng lặp
            VisitCount = g.Count()
                })
                .ToPagedList(pageNumber, pageSize);

            ViewBag.Page = pageNumber;
            ViewBag.PageSize = pageSize;

            return View(groupedCustomers);
        }
    }
}