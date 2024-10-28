using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QL_CHSXM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //liên hệ
            routes.MapRoute(
              name: "Contact",
              url: "lien-he",
              defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
              namespaces: new[] { "QL_CHSXM.Controllers" }
          );

            //thanh toán
            routes.MapRoute(
                 name: "CheckOut",
                 url: "thanh-toan",
                 defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
                 namespaces: new[] { "QL_CHSXM.Controllers" }
          );

            //giỏ hàng
            routes.MapRoute(
             name: "ShoppingCart",
             url: "gio-hang",
             defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
             namespaces: new[] { "QL_CHSXM.Controllers" }
          );

            //danh mục sản phẩm
            routes.MapRoute(
              name: "CategoryProduct",
              url: "danh-muc-phu-tung/{alias}-{id}",
              defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
              namespaces: new[] { "QL_CHSXM.Controllers" }
          );

            //danh mục dịch vụ
            routes.MapRoute(
              name: "CategoryService",
              url: "danh-muc-dich-vu/{alias}-{id}",
              defaults: new { controller = "Services", action = "ServiceCategory", id = UrlParameter.Optional },
              namespaces: new[] { "QL_CHSXM.Controllers" }
          );

            //chi tiết
            routes.MapRoute(
              name: "detailProduct",
              url: "chi-tiet/{alias}-p{id}",
              defaults: new { controller = "Products", action = "Detail", alias = UrlParameter.Optional },
              namespaces: new[] { "QL_CHSXM.Controllers" }
          );

            //sản phẩm
            routes.MapRoute(
               name: "Products",
               url: "phu-tung",
               defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "QL_CHSXM.Controllers" }
           );

            //dịch vụ
            routes.MapRoute(
               name: "Serivces",
               url: "dich-vu",
               defaults: new { controller = "Services", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "QL_CHSXM.Controllers" }
           );

            //đặt lịch dịch vụ
            routes.MapRoute(
               name: "BookService",
               url: "dat-lich-dich-vu",
               defaults: new { controller = "Services", action = "BookService", alias = UrlParameter.Optional },
               namespaces: new[] { "QL_CHSXM.Controllers" }
            );

            //đặt lịch hẹn
            routes.MapRoute(
               name: "Add",
               url: "dat-lich-hen",
               defaults: new { controller = "BookService", action = "BookService", alias = UrlParameter.Optional },
               namespaces: new[] { "QL_CHSXM.Controllers" }
            );

            //chi tiết tin tức
            routes.MapRoute(
              name: "DetailNew",
              url: "{alias}-n{id}",
              defaults: new { controller = "News", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "QL_CHSXM.Controllers" }
           );

            //tin tức
            routes.MapRoute(
             name: "NewsList",
             url: "tin-tuc",
             defaults: new { controller = "News", action = "Index", alias = UrlParameter.Optional },
             namespaces: new[] { "QL_CHSXM.Controllers" }
           );

            //trang chủ
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "QL_CHSXM.Controllers" }
            );
        }
    }
}
