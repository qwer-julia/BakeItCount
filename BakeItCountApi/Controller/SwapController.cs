using BakeItCountApi.Cn.Swaps;
using Microsoft.AspNetCore.Mvc;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwapController : ControllerBase
    {
        private readonly CnSwap _cnSwap;

        public SwapController(CnSwap cnSwap)
        {
            _cnSwap = cnSwap;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestSwap([FromBody] SwapRequest request)
        {
            var swap = await _cnSwap.CreateSwapAsync(request.SourceScheduleId, request.TargetScheduleId, request.RequestingUserId);
            return Ok(swap);
        }

        [HttpPost("{swapId}/respond")]
        public async Task<IActionResult> RespondSwap(int swapId, [FromBody] SwapResponseRequest request)
        {
            var swap = await _cnSwap.RespondSwapAsync(swapId, request.RespondingUserId, request.Approve);
            return Ok(swap);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _cnSwap.GetAllAsync());

        [HttpGet("{swapId}")]
        public async Task<IActionResult> GetById(int swapId) => Ok(await _cnSwap.GetByIdAsync(swapId));

        [HttpGet("getBySchedule/{scheduleId}")]
        public async Task<IActionResult> GetBySchedule(int scheduleId)
        {
            var swap = await _cnSwap.GetByScheduleIdAsync(scheduleId);

            if (swap == null)
                return Ok(new { });

            return Ok(swap);
        }

        [HttpDelete("{swapId}")]
        public async Task<IActionResult> Delete(int swapId)
        {
            await _cnSwap.DeleteAsync(swapId);
            return NoContent();
        }
    }

    public class SwapRequest
    {
        public int SourceScheduleId { get; set; }
        public int TargetScheduleId { get; set; }
        public int RequestingUserId { get; set; }
    }

    public class SwapResponseRequest
    {
        public int RespondingUserId { get; set; }
        public bool Approve { get; set; }
    }
}
