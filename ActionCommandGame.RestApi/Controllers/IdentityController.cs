using ActionCommandGame.RestApi.Security;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;


namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;

        public IdentityController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(UserSignInRequest request)
        {
            var result = await _identityService.SignIn(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            var result = await _identityService.Register(request);
            return Ok(result);
        }

        [HttpGet("GetIdentityUserFromId")]
        public async Task<IActionResult> GetIdentityUserFromId(string userId)
        {
            var result = await _identityService.GetIdentityUserFromId(userId);
            return Ok(result);
        }
    }
}
