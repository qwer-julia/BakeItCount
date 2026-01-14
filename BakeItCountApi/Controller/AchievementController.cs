using BakeItCountApi.Cn.Achievements;
using BakeItCountApi.Cn.UserAchievements;
using Microsoft.AspNetCore.Mvc;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementController : ControllerBase
    {
        private readonly CnUserAchievement _cnUserAchievement;
        private readonly CnAchievement _cnAchievement;

        public AchievementController(CnUserAchievement cnUserAchievement, CnAchievement cnAchievement)
        {
            _cnUserAchievement = cnUserAchievement;
            _cnAchievement = cnAchievement;
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

        [HttpGet("UpdatePurchaseAchievementsByUserId/{userId}")]
        public async Task<IActionResult> UpdatePurchaseAchievementsByUserId(int userId)
        {
            try
            {
                var achievements = await _cnAchievement.CheckPurchaseAchievementsAsync(userId);
                return Ok(achievements);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("UpdateSwapAchievementsByUserId/{userId}")]
        public async Task<IActionResult> UpdateSwapAchievementsByUserId(int userId)
        {
            try
            {
                var achievements = await _cnAchievement.CheckSwapAchievementsAsync(userId);
                return Ok(achievements);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("GetAchievementById/{achievementId}")]
        public async Task<IActionResult> GetAchievementById(int achievementId)
        {
            try
            {
                var achievement = await _cnAchievement.GetAchievementAsync(achievementId);
                return Ok(achievement);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAchievements()
        {
            try
            {
                var achievements = await _cnAchievement.GetAllAchievementsAsync();
                return Ok(achievements);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
