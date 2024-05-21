using ActionCommandGame.RestApi.Security;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController : Controller
    {
        private readonly UserRoleService _userRoleService;
        public UserRoleController(UserRoleService roleService)
        {
            _userRoleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _userRoleService.Find();
            return Ok(result);
        }

        [HttpGet("GetRoleFromUser")]
        public async Task<IActionResult> GetRoleFromUser(string userId)
        {
            var result = await _userRoleService.GetRoleFromUser(userId);
            return Ok(result);
        }

        [HttpGet("SetRoleFromUser")]
        public async Task<IActionResult> SetRoleFromUser(string userId, string roleName)
        {
            var result = await _userRoleService.SetRoleFromUser(userId,roleName);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string userId, string roleId)
        {
            var result = await _userRoleService.Delete(userId,roleId);
            return Ok(result);
        }
    }
}
