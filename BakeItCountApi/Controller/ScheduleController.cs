using BakeItCountApi.Cn.Schedules;
using Microsoft.AspNetCore.Mvc;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly CnSchedule _cnSchedule;

        public ScheduleController(CnSchedule cnSchedule)
        {
            _cnSchedule = cnSchedule;
        }

        [HttpPost("confirm/{scheduleId}")]
        public async Task<IActionResult> Confirm(int scheduleId)
        {
            var schedule = await _cnSchedule.ConfirmAsync(scheduleId);
            return Ok(schedule);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _cnSchedule.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("next")]
        public async Task<IActionResult> GetNextSchedule()
        {
            try
            {
                var schedule = await _cnSchedule.GetNextSchedule();
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{scheduleId}")]
        public async Task<IActionResult> GetScheduleById(int scheduleId)
        {
            return Ok(await _cnSchedule.GetScheduleById(scheduleId));
        }

        [HttpPost("regenerate")]
        public async Task<IActionResult> RegenerateSchedules()
        {
            await _cnSchedule.RegenerateSchedulesAsync();
            return Ok(new { message = "Schedules regenerados com sucesso para os próximos 3 meses." });
        }
    }
}
