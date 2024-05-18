using ActionCommandGame.RestApi.Service;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService) 
        { 
            _accountService = accountService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var result = await _accountService.GetAccount(id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountRequest account)
        {
            var result = await _accountService.UpdateAccount(id, account);
            return Ok(result);
        }
    }
}
