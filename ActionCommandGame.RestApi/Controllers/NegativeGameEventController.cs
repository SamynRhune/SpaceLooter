using ActionCommandGame.Services;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NegativeGameEventController : ControllerBase
    {
        private readonly NegativeGameEventService _negativeGameEventService;

        public NegativeGameEventController(NegativeGameEventService negativeGameEventService)
        {
            _negativeGameEventService = negativeGameEventService;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _negativeGameEventService.Find();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _negativeGameEventService.Get(id);
            return Ok(result);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomNegativeGameEvent()
        {
            var result = await _negativeGameEventService.GetRandomNegativeGameEvent();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NegativeGameEventRequest request)
        {
            var result = await _negativeGameEventService.Create(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NegativeGameEventRequest request)
        {
            var result = await _negativeGameEventService.Update(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _negativeGameEventService.Delete(id);
            return Ok();
        }
    }
}
