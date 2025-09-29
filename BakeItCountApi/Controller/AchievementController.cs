using BakeItCountApi.Cn.Achievements;
using BakeItCountApi.Cn.UserAchievements;
using Microsoft.AspNetCore.Mvc;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementController : ControllerBase
    {
        public CnUserAchievement _cnUserAchievement { get; set; }

        public AchievementController(CnUserAchievement cnUserAchievement)
        {
            _cnUserAchievement = cnUserAchievement;
        }

        [HttpGet("GetAchievementsByUserId/{userId}")]
        public async Task<IActionResult> GetAchievementsByUserId(int userId)
        {
            try
            {
                var userAchievements = await _cnUserAchievement.GetAchievementsByUserAsync(userId);
                return Ok(userAchievements);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
