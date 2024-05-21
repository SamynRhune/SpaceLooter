using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private AccountSdk _accountSdk;
        public AccountController(AccountSdk accountSdk) { 
            _accountSdk = accountSdk;
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAccount(int playerId)
        {
            var account = await _accountSdk.GetAccount(playerId);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(int playerId, AccountRequest request)
        {
            var account = await _accountSdk.UpdateAccount(playerId, request);
            return View(account);
        }

        
        
    }
}
