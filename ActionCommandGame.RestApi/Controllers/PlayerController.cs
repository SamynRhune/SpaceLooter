using ActionCommandGame.Model;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _playerService.Find();
            return Ok(result);
        }

        [HttpGet("by-id/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _playerService.Get(id);
            return Ok(result);
        }

        [HttpGet("by-identity/{id}")]
        public async Task<IActionResult> GetIdentityId(string id)
        {
            var result = await _playerService.GetIdentityId(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerRequest request)
        {
            var result = await _playerService.Create(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PlayerRequest request)
        {
            var result = await _playerService.Update(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _playerService.Delete(id);
            return Ok();
        }

        
    }
}
