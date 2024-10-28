using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QL_CHSXM.Models;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;

namespace QL_CHSXM.Areas.Admin.Controllers
{
    //tắt dòng này + bên Role controller để có thể đăng nhập vào trang admin nha
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult SidebarUserInfo()
        {
            var userName = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == userName);
            string fullName = user != null ? user.FullName : "Guest"; // Nếu không tìm thấy user, hiển thị là Guest

            ViewBag.FullName = fullName;
            return PartialView("_SidebarUserInfo");
        }


        // GET: Admin/Account
        public ActionResult Index()
        {
            var users = db.Users.ToList(); // Retrieve users from your database
            return View(users);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai!");
                    return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        //
        [AllowAnonymous]
        public ActionResult Create()
        {
            // Lấy danh sách vai trò từ cơ sở dữ liệu
            var roles = db.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.Phone
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.Roles != null)
                    {
                        foreach (var role in model.Roles)
                        {
                            await UserManager.AddToRoleAsync(user.Id, role);
                        }
                    }
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }
            // Lấy danh sách vai trò từ cơ sở dữ liệu để hiển thị lại view nếu có lỗi
            var roles = db.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View(model);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = UserManager.GetRoles(user.Id);
            var rolesList = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = userRoles.Contains(r.Name)
            }).ToList();

            ViewBag.Roles = rolesList;

            var model = new EditAccountViewModel
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                SelectedRole = userRoles.FirstOrDefault()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var userRoles = await UserManager.GetRolesAsync(user.Id);
                    if (!userRoles.Contains(model.SelectedRole))
                    {
                        await UserManager.RemoveFromRolesAsync(user.Id, userRoles.ToArray());
                        await UserManager.AddToRoleAsync(user.Id, model.SelectedRole);
                    }

                    return RedirectToAction("Index");
                }

                AddErrors(result);
            }

            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", model.SelectedRole);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult>  DeleteAccount(string id, string user)
        {
            var code = new {Success = false }; //mặc định là không xóa thành công
            var item = UserManager.FindByName(user);
            if (item != null)
            {
                var rolesForUser = UserManager.GetRoles(id);
                if (rolesForUser != null)
                {
                    foreach (var role in rolesForUser)
                    {
                        //roles.Add(role);
                        await UserManager.RemoveFromRoleAsync(id, role);
                    }
                }
                var res = await UserManager.DeleteAsync(item);
                code = new { Success = res.Succeeded };
            }
            return Json(code);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}