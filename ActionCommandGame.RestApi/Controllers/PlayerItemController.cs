using ActionCommandGame.Services;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerItemController : ControllerBase
    {
        private readonly PlayerItemService _playerItemService;

        public PlayerItemController(PlayerItemService playerItemService)
        {
            _playerItemService = playerItemService;
        }

        [HttpGet]
        public async Task<IActionResult> Find(int? playerId = null)
        {
            var result = await _playerItemService.Find(playerId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _playerItemService.Get(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int playerId, int itemId)
        {
            var result = await _playerItemService.Create(playerId, itemId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PlayerItemRequest request)
        {
            var result = await _playerItemService.Update(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _playerItemService.Delete(id);
            return Ok();
        }
    }
}
