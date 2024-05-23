using ActionCommandGame.Security.Model.Abstractions;
using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


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
