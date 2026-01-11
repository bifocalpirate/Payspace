using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Payspace.Domain.Configuration;
using Payspace.Messaging.Http;
using Payspace.UI.Facades;
using Payspace.UI.Models;
using Payspace.UI.Models.DTO;
using Payspace.Utilities;

namespace Payspace.UI.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IRepositoryFacade _apiFacade;

        public LoginController(IRepositoryFacade apiFacade,
            IOptionsSnapshot<AppSettings> appSettings,
            IMessager messager,
            IHttpContextAccessor contextAccessor) : base(appSettings, messager, contextAccessor)
        {
            _apiFacade = apiFacade;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            AuthenticatedUser authenticatedUser = null;
            try
            {
                authenticatedUser = _apiFacade.LoginUser(loginModel.EmailAddress, PasswordFunctions.GetSHA256(loginModel.Password));
            }
            catch (Exception ex) {
                if (!ex.Message.Contains("Unauthorized"))
                {
                    ModelState.AddModelError("Authenitcation Error", "There was an error logging you in: " + ex.Message);
                    return View();
                }
            }
            if (authenticatedUser != null && !string.IsNullOrWhiteSpace(authenticatedUser.JsonToken))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, loginModel.EmailAddress));   
                identity.AddClaim(new Claim(ClaimTypes.Sid, authenticatedUser.JsonToken));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id.ToString()));
                
                SetRoles(authenticatedUser, identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        IsPersistent = loginModel.RememberMe,
                        ExpiresUtc = DateTime.UtcNow.AddHours(_appSettings.Value.TokenTimeOutInHours)
                    });
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewBag.Message = "Password or Email address is invalid.";
                return View();
            }
        }

        private void SetRoles(AuthenticatedUser user, ClaimsIdentity identity)
        {            
            foreach (var role in user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Calculation","Home");
        }
    }
}
