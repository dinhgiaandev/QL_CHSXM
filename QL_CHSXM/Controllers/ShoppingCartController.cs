using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_CHSXM.Models;
using QL_CHSXM.Models.EF;

namespace QL_CHSXM.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }


        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Kiểm tra số lượng sản phẩm có đủ hay không
                            foreach (var item in cart.Items)
                            {
                                var product = db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                                if (product == null || product.Quantity < item.Quantity)
                                {
                                    // Nếu sản phẩm không đủ số lượng, thông báo lỗi
                                    return Json(new { Success = false, msg = "Sản phẩm " + item.ProductName + " không đủ số lượng." });
                                }
                            }

                            // Tạo đơn hàng
                            Order order = new Order
                            {
                                CustomerName = req.CustomerName,
                                Phone = req.Phone,
                                Address = req.Address,
                                Email = req.Email,
                                TypePayment = req.TypePayment,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                CreatedBy = req.Phone,
                                Code = "DH" + new Random().Next(1000, 9999)
                            };

                            foreach (var item in cart.Items)
                            {
                                order.OrderDetails.Add(new OrderDetail
                                {
                                    ProductId = item.ProductId,
                                    Quantity = item.Quantity,
                                    Price = item.Price
                                });

                                // Trừ số lượng sản phẩm trong kho
                                var product = db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                                if (product != null)
                                {
                                    product.Quantity -= item.Quantity;
                                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                                }
                            }

                            order.TotalAmount = cart.Items.Sum(x => x.Price * x.Quantity);

                            db.Orders.Add(order);
                            db.SaveChanges();

                            // Gửi email
                            var strSanPham = cart.Items.Aggregate("", (current, sp) => current + $"<tr><td>{sp.ProductName}</td><td>{sp.Quantity}</td><td>&nbsp;{QL_CHSXM.Common.Common.FormatNumber(sp.TotalPrice, 0)}&nbsp;<span>đ</span></td></tr>");
                            var thanhtien = cart.Items.Sum(sp => sp.Price * sp.Quantity);
                            var TongTien = thanhtien;

                            var contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"))
                                .Replace("{{MaDon}}", order.Code)
                                .Replace("{{SanPham}}", strSanPham)
                                .Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"))
                                .Replace("{{TenKhachHang}}", order.CustomerName)
                                .Replace("{{Phone}}", order.Phone)
                                .Replace("{{Email}}", req.Email)
                                .Replace("{{DiaChiNhanHang}}", order.Address)
                                .Replace("{{ThanhTien}}", QL_CHSXM.Common.Common.FormatNumber(thanhtien, 0))
                                .Replace("{{TongTien}}", QL_CHSXM.Common.Common.FormatNumber(TongTien, 0));

                            QL_CHSXM.Common.Common.SendMail("SBTC Store", $"Đơn hàng #{order.Code}", contentCustomer, req.Email);

                            var contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"))
                                .Replace("{{MaDon}}", order.Code)
                                .Replace("{{SanPham}}", strSanPham)
                                .Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"))
                                .Replace("{{TenKhachHang}}", order.CustomerName)
                                .Replace("{{Phone}}", order.Phone)
                                .Replace("{{Email}}", req.Email)
                                .Replace("{{DiaChiNhanHang}}", order.Address)
                                .Replace("{{ThanhTien}}", QL_CHSXM.Common.Common.FormatNumber(thanhtien, 0))
                                .Replace("{{TongTien}}", QL_CHSXM.Common.Common.FormatNumber(TongTien, 0));

                            QL_CHSXM.Common.Common.SendMail("SBTC Store", $"Đơn hàng mới #{order.Code}", contentAdmin, ConfigurationManager.AppSettings["EmailAdmin"]);

                            transaction.Commit();

                            cart.ClearCart();
                            return RedirectToAction("CheckOutSuccess");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            // Xử lý lỗi nếu cần
                            return Json(new { Success = false, msg = ex.Message });
                        }
                    }
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"] ?? new ShoppingCart();
                var item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity,
                    Price = checkProduct.PriceSale > 0 ? (decimal)checkProduct.PriceSale : checkProduct.Price,
                    ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault)?.Image,
                    TotalPrice = quantity * (checkProduct.PriceSale > 0 ? (decimal)checkProduct.PriceSale : checkProduct.Price)
                };
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                if (cart.Items.Any(x => x.ProductId == id))
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}