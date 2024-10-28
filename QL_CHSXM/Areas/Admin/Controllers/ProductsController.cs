using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_CHSXM.Models;
using QL_CHSXM.Models.EF;

namespace QL_CHSXM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(int? page)
        {
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.Image = Images[i];
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.Alias))
                    if (!string.IsNullOrEmpty(model.Title))
                    {
                        model.Alias = QL_CHSXM.Models.Common.Filter.FilterChar(model.Title);
                    }

                // Kiểm tra giá trị của Quantity trước khi lưu
                if (model.Quantity < 1)
                {
                    ModelState.AddModelError("Quantity", "Số lượng phải lớn hơn hoặc bằng 1.");
                    ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
                    return View(model);
                }

                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            // Lấy danh sách các danh mục sản phẩm để đưa vào dropdown list
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");

            // Tìm sản phẩm theo id
            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound(); // Hoặc xử lý trường hợp không tìm thấy sản phẩm
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Thiết lập ngày chỉnh sửa và filter alias
                    model.ModifiedDate = DateTime.Now;
                    model.Alias = QL_CHSXM.Models.Common.Filter.FilterChar(model.Title);

                    // Đánh dấu sản phẩm là đã chỉnh sửa và lưu vào cơ sở dữ liệu
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu thay đổi: " + ex.Message);
                }
            }

            // Nếu ModelState không hợp lệ, tải lại danh sách danh mục sản phẩm cho dropdown list
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");

            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                var checkImg = item.ProductImage.Where(x => x.ProductId == item.Id);
                if (checkImg != null)
                {
                    foreach(var img in checkImg)
                    {
                        db.ProductImages.Remove(img);
                        db.SaveChanges();
                    }
                }
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}