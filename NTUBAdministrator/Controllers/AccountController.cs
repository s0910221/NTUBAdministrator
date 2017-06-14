using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NTUBAdministrator.Models;
using NTUBAdministrator.ViewModels;

namespace NTUBAdministrator.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Logout()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 要求重新導向至外部登入提供者
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();


            // 若使用者已經有登入資料，請使用此外部登入提供者登入使用者
            if (loginInfo != null)
            {


                //// TODO: 加上你的驗證邏輯，或是註冊會員邏輯                
                string email = loginInfo.Email;
                string name = loginInfo.DefaultUserName;


                var id = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(id);

                return RedirectToLocal(returnUrl);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // 新增外部登入時用來當做 XSRF 保護
        private const string XsrfKey = "XsrfId";

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }



        //private Entities db = new Entities();


        //public AccountController()
        //{
        //}

        //// GET: /Account/Login
        //[AllowAnonymous]
        //public ActionResult Login()
        //{
        //    return View();
        //}

        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginViewModel user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
        //        byte[] src = Encoding.Default.GetBytes(user.Password);//將字串轉為Byte[]
        //        byte[] crypt = sha256.ComputeHash(src);//進行SHA256加密
        //        string result = Convert.ToBase64String(crypt);//把加密後的字串從Byte[]轉為字串
        //        user.Password = result;
        //        var details = (from userList in db.UserAccount
        //                       where userList.AccountID == user.AccountID && userList.Password == user.Password
        //                       select new { userList.AccountID, userList.UserName });
        //        if (details.FirstOrDefault() != null)
        //        {
        //            Session["AccountID"] = details.FirstOrDefault().AccountID;
        //            Session["UserName"] = details.FirstOrDefault().UserName;
        //            return RedirectToAction("ActivityManagement", "Activity");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "帳號或密碼錯誤!");
        //    }
        //    return View(user);
        //}

        ////
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // 要求重新導向至外部登入提供者
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        ////
        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

        //    // 若使用者已經有登入資料，請使用此外部登入提供者登入使用者
        //    if (loginInfo != null)
        //    {

        //        //A方法
        //        var claims = new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, "myname"),
        //            new Claim(ClaimTypes.Email, "myemail@gmail.com")
        //        };
        //        var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        //        AuthenticationManager.SignIn(identity);

        //        //B方法
        //        var id = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims,
        //                                DefaultAuthenticationTypes.ApplicationCookie);

        //        // TODO: 加上你的驗證邏輯，或是註冊會員邏輯                

        //        AuthenticationManager.SignIn(id);

        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login");
        //    }


        //    /*
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // 若使用者沒有帳戶，請提示使用者建立帳戶
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, UserName = loginInfo.DefaultUserName });
        //    }
        //    */
        //}

        //public ActionResult Logout()
        //{
        //    this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

        //    return RedirectToAction("Login");
        //}

        ///*
        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // 從外部登入提供者處取得使用者資訊
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new UserAccount { UserName = model.UserName, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //// POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOff()
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //    return RedirectToAction("Index", "Home");
        //}




        //// GET: /Account/Logout
        //[AllowAnonymous]
        //public ActionResult Logout()
        //{
        //    Session.Clear();
        //    return RedirectToAction("Login", "Account");
        //}


        ////
        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        if (_signInManager != null)
        //        {
        //            _signInManager.Dispose();
        //            _signInManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}
        //*/

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //#region Helper
        //// 新增外部登入時用來當做 XSRF 保護
        //private const string XsrfKey = "XsrfId";

        ///*
        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}
        //*/

        //internal class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}
        //#endregion
    }
}