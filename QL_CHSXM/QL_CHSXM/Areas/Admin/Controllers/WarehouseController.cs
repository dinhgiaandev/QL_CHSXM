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
            var pageSize = 5; // Số lượng sản phẩm trên mỗi trang

            // Lấy danh sách sản phẩm từ database
            var items = db.Products.Include("ProductCategory").OrderBy(x => x.Id).ToList();

            // Chuyển danh sách items thành đối tượng IPagedList để phân trang
            IPagedList<Product> pagedItems = items.ToPagedList(pageNumber, pageSize);

            // Truyền IPagedList sang view để hiển thị và phân trang
            return View(pagedItems);
        }
    }
}
