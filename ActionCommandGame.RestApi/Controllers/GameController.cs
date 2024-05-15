using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services;
using Microsoft.AspNetCore.Mvc;
using ActionCommandGame.RestApi.Service;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ActionCommandGame.Model;

namespace ActionCommandGame.RestApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("{playerId:int}")]
        public async Task<IActionResult> PerformAction(int playerId)
        {
            var result = await _gameService.PerformAction(playerId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Buy")]
        public async Task<IActionResult> Buy(int playerId,int itemId)
        {
            var result = await _gameService.Buy(playerId, itemId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Activate")]
        public async Task ActivateItemId(int playerId, int playerItemId)
        {
            await _gameService.activateItem(playerId, playerItemId);
           
        }

        
    }
}
