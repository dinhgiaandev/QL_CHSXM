using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QL_CHSXM.Models;
using QL_CHSXM.Models.EF;

namespace QL_CHSXM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class WarehouseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Warehouse
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1 nếu không có page được truyền vào
            var pageSize = 5;


            // Lấy danh sách sản phẩm từ database và áp dụng phân trang trực tiếp
            var items = db.Products.Include("ProductCategory")
                                   .OrderBy(x => x.Id)
                                   .ToPagedList(pageNumber, pageSize);

            // Truyền IPagedList sang view để hiển thị và phân trang
            return View(items);
        }
    }
}
