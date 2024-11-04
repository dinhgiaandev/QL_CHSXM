using System.Linq;
using System.Web.Mvc;
using PagedList;
using QL_CHSXM.Models;
using PagedList;
using QL_CHSXM.Models.EF;

namespace QL_CHSXM.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Customer
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;

            var customers = db.BookServices.ToList();

            var groupedCustomers = customers
                .GroupBy(c => new { c.FullName, c.Phone })
                .Select(g => new BookService
                {
                    FullName = g.Key.FullName,
                    Phone = g.Key.Phone,
                    Email = g.First().Email,
                    NameCar = string.Join(", ", g.Select(c => c.NameCar).Distinct()),
                    VisitCount = g.Count()
                })
                .ToPagedList(pageNumber, pageSize);

            ViewBag.Page = pageNumber;
            ViewBag.PageSize = pageSize;

            return View(groupedCustomers);
        }
    }
}