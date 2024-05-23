using ActionCommandGame.Services;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PositiveGameEventController : ControllerBase
    {
        private readonly PositiveGameEventService _positiveGameEventService;

        public PositiveGameEventController(PositiveGameEventService positiveGameEventService)
        {
            _positiveGameEventService = positiveGameEventService;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _positiveGameEventService.Find();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _positiveGameEventService.Get(id);
            return Ok(result);
        }

        [HttpGet("random/{hasAttack:bool}")]
        public async Task<IActionResult> GetRandomPositiveGameEvent(bool hasAttack)
        {
            var result = await _positiveGameEventService.GetRandomPositiveGameEvent(hasAttack);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PositiveGameEventRequest request)
        {
            var result = await _positiveGameEventService.Create(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PositiveGameEventRequest request)
        {
            var result = await _positiveGameEventService.Update(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _positiveGameEventService.Delete(id);
            return Ok();
        }
    }
}
