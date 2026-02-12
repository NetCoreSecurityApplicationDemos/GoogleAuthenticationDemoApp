using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoogleAuthenticationDemoApp.Controllers
{
    public class AccountController
        : Controller
    {

        private readonly SignInManager<IdentityUser>  _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
                                 UserManager<IdentityUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }


        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);

        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false
            );

            if (result.Succeeded)
                return LocalRedirect(returnUrl ?? "/");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = new IdentityUser
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(returnUrl ?? "/");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
