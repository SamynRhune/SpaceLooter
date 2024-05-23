using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ActionCommandGame.Security.Model.Abstractions;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Ui.WebApp.Models;





namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class IdentityController(IdentitySdk identitySdk, ITokenStore tokenStore) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> SignIn(string? returnUrl)
        {
            await HttpContext.SignOutAsync();

            ViewBag.ReturnUrl = returnUrl ?? "/";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model, string? returnUrl)
        {
            returnUrl ??= "/";

            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var request = new UserSignInRequest
            {
                UserName = model.Username,
                Password = model.Password
            };

            var result = await identitySdk.SignIn(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            tokenStore.SaveToken(result.Token);
            var principal = CreatePrincipalFromToken(result.Token);
            var claims = principal.Claims.ToList();
            await HttpContext.SignInAsync(principal);

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string? returnUrl)
        {
            returnUrl ??= "/";

            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var request = new UserRegisterRequest()
            {
                UserName = model.Username,
                Password = model.Password
            };

            var result = await identitySdk.Register(request);
            if (!result.IsSuccess)
            {
                foreach (var error in result.Messages)
                {
                    ModelState.AddModelError("", error.Message);
                }

                ViewBag.ReturUrl = returnUrl;
                return View(model);
            }

            tokenStore.SaveToken(result.Token);
            var principal = CreatePrincipalFromToken(result.Token);
            await HttpContext.SignInAsync(principal);

            return LocalRedirect(returnUrl);
        }

        private ClaimsPrincipal CreatePrincipalFromToken(string? bearerToken)
        {
            var identity = CreateIdentityFromToken(bearerToken);
            return new ClaimsPrincipal(identity);
        }

        private ClaimsIdentity CreateIdentityFromToken(string? bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return new ClaimsIdentity();
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(bearerToken);

            var claims = new List<Claim>();
            foreach (var claim in token.Claims)
            {
                claims.Add(claim);
            }

            //HttpContext required a "Name" claim to display a User Name
            var usernameClaim = token.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (usernameClaim is not null)
            {
                claims.Add(new Claim(ClaimTypes.Name, usernameClaim.Value));
            }

            var roleClaim = token.Claims.SingleOrDefault(c => c.Type == "role");
            if (roleClaim is not null)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
            }



            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> SetUserAsUser(string username)
        {
            var returnUrl =  "/";
            var result = await identitySdk.SetUserAsUser(username);
            tokenStore.SaveToken(result.Token);
            var principal = CreatePrincipalFromToken(result.Token);
            await HttpContext.SignInAsync(principal);
            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> SetUserAsAdmin(string username)
        {
            var returnUrl = "/";
            var result = await identitySdk.SetUserAsAdmin(username);
            tokenStore.SaveToken(result.Token);
            var principal = CreatePrincipalFromToken(result.Token);
            await HttpContext.SignInAsync(principal);
            return LocalRedirect(returnUrl);
        }

        
    }
}
