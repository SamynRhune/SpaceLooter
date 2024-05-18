using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private PlayerSdk _playerSdk;
        private IdentitySdk _identitySdk;
        public AccountController(PlayerSdk playerSdk, IdentitySdk identitySdk) { 
            _playerSdk = playerSdk;
            _identitySdk = identitySdk;
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAccount(int playerId)
        {
            var account = await _playerSdk.GetAccount(playerId);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(int playerId, AccountRequest request)
        {
            var account = await _playerSdk.UpdateAccount(playerId, request);
            return View(account);
        }

        
    }
}
