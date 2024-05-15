using ActionCommandGame.Security.Model;
using ActionCommandGame.Security.Model.Abstractions;
using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Security.Claims;

namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenStore _tokenStore;

        public HomeController(ILogger<HomeController> logger, ITokenStore tokenstore)
        {
            _logger = logger;
            _tokenStore = tokenstore;
            
        }

        
        public async Task<IActionResult> Index()
        {
            var token = _tokenStore.GetToken;
            
            var claims = User.Claims.ToList();
            var roleClaims = User.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role") // check both common role claim types
                .ToList();

            var isAdmin = User.Claims.Any(c =>
                    (c.Type == ClaimTypes.Role || c.Type == "role") && c.Value == "Admin");
            Console.WriteLine($"Is in Admin role: {isAdmin}");
            // Controleer expliciet op de rol
            var isAdmin2 = User.IsInRole( "Admin");
            //User.AddIdentity(User, IdentityUserRole.Admin<> );


            var rolelist = User.FindAll(ClaimTypes.Role).ToList();

            

            Console.WriteLine($"Is in Admin role: {isAdmin}");
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
